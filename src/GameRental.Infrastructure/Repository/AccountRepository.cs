using System.Linq.Expressions;
using GameRental.Domain.Entities;
using GameRental.Domain.IRepository;
using GameRental.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

namespace GameRental.Infrastructure.Repository;

public class AccountRepository:IAccountRepository
{
    private readonly GameRentalDbContext _db;

    
    public AccountRepository(GameRentalDbContext db)
    {
        _db = db;
    }


    public async Task<Account?> GetByIdAsync(int id,bool asTracking=false,params Expression<Func<Account, object>>[] includeProperties)
    {
        QueryTrackingBehavior track = asTracking switch
        {
            true => QueryTrackingBehavior.TrackAll,
            false => QueryTrackingBehavior.NoTracking
        };
        
        IQueryable<Account> query = _db.Accounts;
        
        foreach (var includeProperty in includeProperties)
        {
            query=query.Include(includeProperty);
        }
        query=query.AsTracking(track);

        return await query.FirstOrDefaultAsync(u=>u.Id==id);
    }

    public async Task<List<Account>> GetAccountsAsync(Expression<Func<Account,bool>>? filter = null,params Expression<Func<Account, object>>[] includeProperties)
    {
        IQueryable<Account> query = _db.Accounts;
        
        if (filter!=null)
        {
            query=query.Where(filter);
        }
        
        foreach (var includeProperty in includeProperties)
        {
            query=query.Include(includeProperty);
        }

        return await query.ToListAsync();
    }

    public void Add(Account account)
    {
        _db.Accounts.Add(account);
    }

    public void Remove(Account account)
    {
        _db.Accounts.Remove(account);
    }

    public void Update(Account account)
    {
        _db.Accounts.Update(account);
    }
}