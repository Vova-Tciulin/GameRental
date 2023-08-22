using FluentValidation;
using GameRental.Application.DTO.Order;

namespace GameRental.Application.Validation;

public class OrderForAddDtoValidator:AbstractValidator<OrderAddDto>
{
    public OrderForAddDtoValidator()
    {
        RuleFor(u => u.UserEmail).NotNull();
        RuleFor(u => u.AccountId).Must(u => u > 0);
        RuleFor(u => u.DayOfRent).Must(u => u > 0);
        RuleFor(u => u.Cost).Must(u => u > 0);
    }
}