using FluentValidation;

namespace PhotoSi.UsersService.Features.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(command => command.email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Invalid email format.");

            RuleFor(command => command.username)
                .NotEmpty()
                .WithMessage("Username is required.")
                .MaximumLength(50)
                .WithMessage("Username must not exceed 50 characters.");

            RuleFor(command => command.firstname)
                .NotEmpty()
                .WithMessage("First name is required.")
                .MaximumLength(50)
                .WithMessage("First name must not exceed 50 characters.");

            RuleFor(command => command.lastname)
                .NotEmpty()
                .WithMessage("Last name is required.")
                .MaximumLength(50)
                .WithMessage("Last name must not exceed 50 characters.");

            RuleFor(command => command.phoneNumber)
                .Matches(@"^\+?\d+$")
                .WithMessage("Phone number must contain only numbers and may start with '+'.");

            RuleFor(command => command.profilePictureUrl)
                .MaximumLength(500)
                .WithMessage("Profile picture must not exceed 50 characters.");

            RuleFor(command => command.shipmentAddress.city)
                .NotEmpty()
                .WithMessage("City is required.")
                .MaximumLength(100)
                .WithMessage("City must not exceed 100 characters.");

            RuleFor(command => command.shipmentAddress.country)
                .NotEmpty()
                .WithMessage("Country is required.")
                .MaximumLength(100)
                .WithMessage("Country must not exceed 100 characters.");

            RuleFor(command => command.shipmentAddress.postalCode)
                .NotEmpty()
                .WithMessage("Postal code is required.")
                .MaximumLength(20)
                .WithMessage("Postal code must not exceed 20 characters.");

            RuleFor(command => command.shipmentAddress.street)
                .NotEmpty()
                .WithMessage("Street is required.")
                .MaximumLength(200)
                .WithMessage("Street must not exceed 200 characters.");
        }
    }
}
