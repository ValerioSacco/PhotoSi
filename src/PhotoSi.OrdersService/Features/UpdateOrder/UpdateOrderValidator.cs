using FluentValidation;

namespace PhotoSi.OrdersService.Features.UpdateOrder
{
    public class UpdateOrderValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderValidator()
        {
            RuleFor(command => command.orderLines)
                .NotEmpty()
                .WithMessage("Order lines must not be empty.");

            RuleForEach(command => command.orderLines)
                .ChildRules(orderLine =>
                {
                    orderLine.RuleFor(line => line.orderLineId)
                        .NotEmpty()
                        .WithMessage("Order line id must not be empty");
                    orderLine.RuleFor(line => line.productId)
                        .NotEmpty()
                        .WithMessage("Product id must not be empty.");
                    orderLine.RuleFor(line => line.quantity)
                        .GreaterThan(0)
                        .WithMessage("Quantity must be greater than zero.");
                    orderLine.RuleFor(line => line.notes)
                        .MaximumLength(500)
                        .WithMessage("Notes can be maximum 500 charcters long");

                });
        }
    }
}
