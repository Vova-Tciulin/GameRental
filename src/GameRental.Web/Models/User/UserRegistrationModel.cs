using System.ComponentModel.DataAnnotations;

namespace GameRental.Web.Models.User;

public class UserRegistrationModel
{
    [Required(ErrorMessage = "Обязательное поле")]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Обязательное поле")]
    public string LinkToNetwork { get; set; }
    
    [Required(ErrorMessage = "Обязательное поле")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    public string ConfirmPassword { get; set; }
}