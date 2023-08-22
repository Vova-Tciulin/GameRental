using GameRental.Application.DTO;
using GameRental.Application.DTO.Console;

namespace GameRental.Application.Interfaces;

public interface IConsoleService
{
    Task<List<ConsoleDto>> GetAllAsync();
}