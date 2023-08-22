using GameRental.Application.DTO.Account;
using GameRental.Application.DTO.User;

namespace GameRental.Application.DTO.Order;

public class OrderDto
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public int AccountId { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime EndOrderDate { get; set; }
    public bool IsActive { get; set; }
    public decimal Cost { get; set; }
    public UserDto? User { get; set; }
    public AccountDto? Account { get; set; }
}