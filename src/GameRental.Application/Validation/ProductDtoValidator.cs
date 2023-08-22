using FluentValidation;
using GameRental.Application.DTO;
using GameRental.Application.DTO.Product;

namespace GameRental.Application.Validation;

public class ProductAddDtoValidator:AbstractValidator<ProductAddDto>
{
    public ProductAddDtoValidator()
    {
        RuleFor(u => u.Name)
            .NotNull()
            .MaximumLength(50);
        RuleFor(u => u.Description).NotNull();
        RuleFor(u => u.ProductCategoriesId).NotEmpty();
        RuleFor(u => u.ImageCollection).NotEmpty();
        RuleFor(u => u.Translate).NotNull();
        RuleFor(u => u.TransitTime).Must(u => u > 0);
        RuleFor(u => u.Year).Must(u => u > 0);
    }
}