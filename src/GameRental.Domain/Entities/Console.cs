namespace GameRental.Domain.Entities;

public class Console
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Account> Accounts { get; set; } = new();
}