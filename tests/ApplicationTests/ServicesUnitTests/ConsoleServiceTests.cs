using AutoMapper;
using FluentValidation;
using GameRental.Application.DTO.Account;
using GameRental.Application.DTO.Console;
using GameRental.Application.Services;
using GameRental.Domain.IRepository;
using Moq;
using Console = GameRental.Domain.Entities.Console;

namespace ApplicationTests.ServicesUnitTests;

[TestFixture]
public class ConsoleServiceTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock = null!;
    private Mock<IMapper> _mapperMock = null!;
    private Mock<IConsoleRepository> _consoleRepository = null!;
    
    private ConsoleService _consoleService=null!;

    [SetUp]
    public void SetUp()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _consoleRepository = new Mock<IConsoleRepository>();
        
        _consoleService = new ConsoleService(_unitOfWorkMock.Object, _mapperMock.Object);

        _unitOfWorkMock.Setup(u => u.ConsoleRepository).Returns(_consoleRepository.Object);
    }

    [Test]
    public async Task GetAllAsync_ReturnsListOfConsolesDto()
    {
        //arrange
        List<Console> consoles = new List<Console>() { new Console(), new Console() };
        List<ConsoleDto> consolesDto = new List<ConsoleDto>() { new ConsoleDto(), new ConsoleDto()};
        _unitOfWorkMock.Setup(u => u.ConsoleRepository.GetConsolesAsync(null)).ReturnsAsync(consoles);
        _mapperMock.Setup(u => u.Map<List<ConsoleDto>>(consoles)).Returns(consolesDto);
        
        //act
        var result = await _consoleService.GetAllAsync();
        
        //assert
        Assert.That(result,Is.EqualTo(consolesDto));
    }
}