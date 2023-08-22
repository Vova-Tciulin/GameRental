namespace GameRental.Application.DTO.Order;

public class OrderAddDto
{
    public string UserEmail { get; set; }
    public int AccountId { get; set; }
    public int  DayOfRent { get; set; }
    public decimal Cost { get; set; }
}