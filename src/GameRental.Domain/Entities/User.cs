using Microsoft.AspNetCore.Identity;

namespace GameRental.Domain.Entities;

public class User:IdentityUser
{
    public string LinkToNetwork { get; set; }
    public List<Order> Orders { get; set; } = new();
}