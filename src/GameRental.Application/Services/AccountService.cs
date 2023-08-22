using System.Linq.Expressions;
using AutoMapper;
using FluentValidation;
using GameRental.Application.DTO.Account;
using GameRental.Application.Exceptions;
using GameRental.Application.Interfaces;
using GameRental.Domain.Entities;
using GameRental.Domain.Exceptions;
using GameRental.Domain.IRepository;


namespace GameRental.Application.Services;

public class AccountService:IAccountService
{
    private readonly IUnitOfWork _db;
    private readonly IMapper _map;
    private readonly IValidator<AccountForAddDto> _accountForAddDtoValidator;
    private readonly IValidator<AccountForUpdateDto> _accountForUpdateDtoValidator;
    

    public AccountService(IUnitOfWork db, IMapper map, IValidator<AccountForAddDto> accountForAddDtoValidator, IValidator<AccountForUpdateDto> accountForUpdateDtoValidator)
    {
        _db = db;
        _map = map;
        _accountForAddDtoValidator = accountForAddDtoValidator;
        _accountForUpdateDtoValidator = accountForUpdateDtoValidator;
    }

    public async Task<AccountDto> GetByIdAsync(int id,params Expression<Func<Account, object>>[] includeProperties)
    {
        var account=await _db.AccountRepository.GetByIdAsync(id,false,includeProperties);

        if (account==null)
        {
            throw new AccountNotFoundException(id);
        }

        var accountDto = _map.Map<AccountDto>(account);
        
        return accountDto;
    }
    
    public async Task<List<AccountDto>> GetAllAsync(Expression<Func<Account,bool>>? filter = null,params Expression<Func<Account, object>>[] includeProperties)
    {
        var accounts = await _db.AccountRepository.GetAccountsAsync(filter,includeProperties);
        var accountsDto = _map.Map<List<AccountDto>>(accounts);

        return accountsDto;
    }

    public async Task AddAccountAsync(AccountForAddDto accountForAddDto)
    {
        var validationResult = await _accountForAddDtoValidator.ValidateAsync(accountForAddDto);
        
        if (!validationResult.IsValid)
        {
            throw new NotValidateModelException(nameof(AccountForAddDto), validationResult.ToString("\n"));
        }

        var product = await _db.ProductRepository.GetByIdAsync(accountForAddDto.ProductId);
        
        if (product==null)
        {
            throw new ProductNotFoundExeption(accountForAddDto.ProductId);
        }
        
        var account = _map.Map<Account>(accountForAddDto);
        account.Name = product.Name;
        account.Consoles = await _db.ConsoleRepository.GetConsolesAsync(u=>accountForAddDto.SelectedConsoles.Contains(u.Id));
        account.AccountNumber = await GetAccountNumberAsync(product.Id);
        
        _db.AccountRepository.Add(account);
        await _db.SaveChangesAsync();
    }

    public async Task RemoveAsync(int id)
    {
        var account = await _db.AccountRepository.GetByIdAsync(id);
        if (account==null)
        {
            throw new AccountNotFoundException(id);
        }
        
        _db.AccountRepository.Remove(account);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(AccountForUpdateDto accountForUpdateDto)
    {
        
        var validationResult = await _accountForUpdateDtoValidator.ValidateAsync(accountForUpdateDto);
        
        if (!validationResult.IsValid)
        {
            throw new NotValidateModelException(nameof(AccountForAddDto), validationResult.ToString("\n"));
        }

        var account = await _db.AccountRepository.GetByIdAsync(accountForUpdateDto.Id,true,u=>u.Consoles);

        if (account == null)
        {
            throw new AccountNotFoundException(accountForUpdateDto.Id);
        }

        if (account.ProductId!=accountForUpdateDto.ProductId)
        {
            var product = await _db.ProductRepository.GetByIdAsync(accountForUpdateDto.ProductId);
            if (product==null)
            {
                throw new ProductNotFoundExeption(accountForUpdateDto.ProductId);
            }

            account.Name = product.Name;
            account.AccountNumber = await GetAccountNumberAsync(product.Id);
        }
        
        account.ProductId=accountForUpdateDto.ProductId;
        account.Price=accountForUpdateDto.Price;
        account.Login=accountForUpdateDto.Login;
        account.Password=accountForUpdateDto.Password;
        account.Consoles = await _db.ConsoleRepository
            .GetConsolesAsync(u => accountForUpdateDto.SelectedConsoles.Contains(u.Id));;
        
        if (accountForUpdateDto.IsRented==false&&account.IsRented)
        {
            var order = await _db.OrderRepository.FirstOrDefaultAsync(u => u.AccountId == account.Id && u.IsActive,true);
            if (order!=null)
            {
                order.IsActive = false;
            }

            account.IsRented = false;
            await _db.SaveChangesAsync();
        }
        else
        {
            await _db.SaveChangesAsync();
        }
    }

    private async Task<int> GetAccountNumberAsync(int productId)
    {
        var accCount = await _db.AccountRepository.GetAccountsAsync(u => u.ProductId == productId);
        var acc = accCount.OrderByDescending(u => u.AccountNumber).ToList();

        if (accCount.Count==0)
        {
            return 1;
        }
        return acc[0].AccountNumber +1;
    }
}