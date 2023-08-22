using System.Linq.Expressions;
using GameRental.Domain.Entities;
using GameRental.Domain.IRepository;
using GameRental.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using Console = GameRental.Domain.Entities.Console;

namespace GameRental.Infrastructure.Repository;

public class ConsoleRepository:IConsoleRepository
{
    private readonly GameRentalDbContext _db;

    public ConsoleRepository(GameRentalDbContext db)
    {
        _db = db;
    }

    public async Task<List<Console>> GetConsolesAsync(Expression<Func<Console,bool>>? filter = null)
    {
        if (filter!=null)
        {
            return await _db.Consoles.Where(filter).ToListAsync();
        }
        
        return await _db.Consoles.ToListAsync();
    }
}