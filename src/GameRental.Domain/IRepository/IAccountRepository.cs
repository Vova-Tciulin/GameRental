using System.Linq.Expressions;
using GameRental.Domain.Entities;

namespace GameRental.Domain.IRepository;

public interface IAccountRepository
{
    Task<Account?> GetByIdAsync(int id,bool asTracking=false,params Expression<Func<Account, object>>[] includeProperties);
    Task<List<Account>> GetAccountsAsync(Expression<Func<Account,bool>>? filter = null,params Expression<Func<Account, object>>[] includeProperties);
    void Add(Account account);
    void Remove(Account account);
    
    public void Update(Account account);
}