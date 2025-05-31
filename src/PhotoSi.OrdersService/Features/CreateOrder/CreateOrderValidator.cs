using FluentValidation;

namespace PhotoSi.OrdersService.Features.CreateOrder
{
    public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderValidator()
        {
            RuleFor(command => command.userId)
                .NotEmpty()
                .WithMessage("User id must not be empty.");

            RuleFor(command => command.orderLines)
                .NotEmpty()
                .WithMessage("Order lines must not be empty.");

            RuleForEach(command => command.orderLines)
                .ChildRules(orderLine =>
                {
                    orderLine.RuleFor(line => line.productId)
                        .NotEmpty()
                        .WithMessage("Product id must not be empty.");
                    orderLine.RuleFor(line => line.quantity)
                        .GreaterThan(0)
                        .WithMessage("Quantity must be greater than zero.");
                });
        }
    }
}
