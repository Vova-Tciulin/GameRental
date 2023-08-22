using AutoMapper;
using GameRental.Application.DTO;
using GameRental.Application.DTO.Console;
using GameRental.Application.Interfaces;
using GameRental.Domain.IRepository;

namespace GameRental.Application.Services;

public class ConsoleService:IConsoleService
{
    private readonly IUnitOfWork _db;
    private readonly IMapper _map;

    public ConsoleService(IUnitOfWork db, IMapper map)
    {
        _db = db;
        _map = map;
    }

    public async Task<List<ConsoleDto>> GetAllAsync()
    {
        var consoles = await _db.ConsoleRepository.GetConsolesAsync();

        return _map.Map<List<ConsoleDto>>(consoles);
    }
}