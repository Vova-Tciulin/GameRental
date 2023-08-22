using System.Linq.Expressions;
using GameRental.Domain.Entities;
using Console = GameRental.Domain.Entities.Console;

namespace GameRental.Domain.IRepository;

public interface IConsoleRepository
{
    Task<List<Console>> GetConsolesAsync(Expression<Func<Console,bool>>? filter = null);
}