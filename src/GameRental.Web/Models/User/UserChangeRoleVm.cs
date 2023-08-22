namespace GameRental.Web.Models.User;

public class UserChangeRoleVm
{
    public string Id { get; set; }
    public List<string> AllRoles { get; set; } = new List<string>();
    public string[] NewRoles { get; set; }
}