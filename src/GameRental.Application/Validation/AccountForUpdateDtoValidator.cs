using FluentValidation;
using GameRental.Application.DTO.Account;

namespace GameRental.Application.Validation;

public class AccountForUpdateDtoValidator:AbstractValidator<AccountForUpdateDto>
{
    public AccountForUpdateDtoValidator()
    {
        RuleFor(u=>u.Id).Must(u => u > 0);
        RuleFor(u => u.ProductId).Must(u => u > 0);
        RuleFor(u => u.Name).NotNull();
        RuleFor(u => u.Login).NotNull();
        RuleFor(u => u.Password).NotNull();
        RuleFor(u => u.AccountNumber).Must(u => u > 0);
        RuleFor(u => u.SelectedConsoles).NotEmpty();
        RuleFor(u => u.Price).Must(u => u > 0);
    }
}