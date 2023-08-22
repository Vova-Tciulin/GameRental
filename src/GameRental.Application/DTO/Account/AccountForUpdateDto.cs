namespace GameRental.Application.DTO.Account;

public class AccountForUpdateDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    
    public int AccountNumber { get; set; }
    public decimal Price { get; set; }
    public bool IsRented { get; set; }
    public int ProductId { get; set; }
    public int[] SelectedConsoles { get; set; }
    
}