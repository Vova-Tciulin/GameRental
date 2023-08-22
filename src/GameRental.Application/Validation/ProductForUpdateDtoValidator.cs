using FluentValidation;
using GameRental.Application.DTO;
using GameRental.Application.DTO.Product;

namespace GameRental.Application.Validation;

public class ProductForUpdateDtoValidator:AbstractValidator<ProductForUpdateDto>
{
    public ProductForUpdateDtoValidator()
    {
        RuleFor(u => u.Id).Must(u => u > 0);
        RuleFor(u => u.Description).NotNull();
        RuleFor(u => u.Name)
            .NotNull()
            .MaximumLength(50);
        RuleFor(u => u.ProductCategoriesId).NotEmpty();
        RuleFor(u => u.TransitTime).Must(u => u > 0);
        RuleFor(u => u.Year).Must(u => u > 0);
        RuleFor(u => u.Translate).NotNull();
    }
}