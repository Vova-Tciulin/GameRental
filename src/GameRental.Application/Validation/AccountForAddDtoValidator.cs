using FluentValidation;
using GameRental.Application.DTO;
using GameRental.Application.DTO.Account;

namespace GameRental.Application.Validation;

public class AccountForAddDtoValidator:AbstractValidator<AccountForAddDto>
{
    public AccountForAddDtoValidator()
    {
        RuleFor(u => u.ProductId).NotNull();
        RuleFor(u => u.Login).NotNull();
        RuleFor(u => u.Password).NotNull();
        RuleFor(u => u.SelectedConsoles).NotEmpty();
        RuleFor(u => u.Price).Must(u => u > 0);
    }
}