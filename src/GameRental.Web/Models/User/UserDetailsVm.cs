namespace GameRental.Web.Models.User;

public class UserDetailsVm
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string LinkToNetwork { get; set; }
    public List<string> Roles { get; set; } = new List<string>();
}