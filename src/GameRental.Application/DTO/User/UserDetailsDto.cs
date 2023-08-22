namespace GameRental.Application.DTO.User;

public class UserDetailsDto
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string LinkToNetwork { get; set; }
    public List<string> Roles { get; set; } = new List<string>();
}