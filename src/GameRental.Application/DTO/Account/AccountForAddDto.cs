namespace GameRental.Application.DTO.Account;

public class AccountForAddDto
{
    
    public string Login { get; set; }
    public string Password { get; set; }
    public decimal Price { get; set; }
    public int AccountNumber { get; set; }
    public int ProductId { get; set; }
    public int[] SelectedConsoles { get; set; }
}