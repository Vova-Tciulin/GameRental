using System.Linq.Expressions;
using GameRental.Application.DTO;
using GameRental.Application.DTO.Account;
using GameRental.Domain.Entities;

namespace GameRental.Application.Interfaces;

public interface IAccountService
{
    Task<AccountDto> GetByIdAsync(int id,params Expression<Func<Account, object>>[] includeProperties);
    Task<List<AccountDto>> GetAllAsync(Expression<Func<Account,bool>>? filter = null,params Expression<Func<Account, object>>[] includeProperties);
    Task AddAccountAsync(AccountForAddDto accountForAddDto);
    public Task RemoveAsync(int id);
    public Task UpdateAsync(AccountForUpdateDto accountForUpdateDto);
    
}