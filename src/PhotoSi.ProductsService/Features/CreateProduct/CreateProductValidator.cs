﻿using FluentValidation;

namespace PhotoSi.ProductsService.Features.CreateProduct
{
    public class CreateProductValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductValidator()
        {
            RuleFor(command => command.name)
                .NotEmpty()
                .WithMessage("Product name is required.")
                .MaximumLength(100)
                .WithMessage("Product name must not exceed 100 characters.");

            RuleFor(command => command.description)
                .NotEmpty()
                .WithMessage("Product description is required.")
                .MaximumLength(500)
                .WithMessage("Product description must not exceed 500 characters.");

            RuleFor(command => command.price)
                .GreaterThan(0)
                .WithMessage("Product price must be greater than zero.");

            RuleFor(command => command.imageUrl)
                .NotEmpty()
                .WithMessage("Product image URL is required.")
                .MaximumLength(500)
                .WithMessage("Product image URL must not exceed 500 characters.");

            RuleFor(command => command.categoryId)
                .NotEmpty()
                .WithMessage("Category ID is required.")
                .Must(categoryId => categoryId != Guid.Empty)
                .WithMessage("Category ID must be a valid GUID.");
        }
    }
}
