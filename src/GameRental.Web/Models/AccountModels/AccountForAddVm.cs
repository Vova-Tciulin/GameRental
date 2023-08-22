using System.ComponentModel.DataAnnotations;
using GameRental.Web.Models.ProductModels;

namespace GameRental.Web.Models.AccountModels;

public class AccountForAddVm
{
    
    
    [Required(ErrorMessage = "Введите логин")]
    public string Login { get; set; }
    
    [Required(ErrorMessage = "Введите пароль")]
    public string Password { get; set; }
    
    [Required(ErrorMessage = "Введите цену аренды")]
    public decimal Price { get; set; }
    
    [Required(ErrorMessage = "выберите игру для аккаунта")]
    public int? ProductId { get; set; }
    
    [Required(ErrorMessage = "Выберите тип консолей")]
    public int[] SelectedConsoles { get; set; }
    public List<ProductVM> Products { get; set; } = new();
    public List<ConsoleVm> Consoles { get; set; } = new();
}