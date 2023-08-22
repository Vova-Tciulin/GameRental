using System.ComponentModel.DataAnnotations;

namespace GameRental.Web.Models.User;

public class ForgotPassworddModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}