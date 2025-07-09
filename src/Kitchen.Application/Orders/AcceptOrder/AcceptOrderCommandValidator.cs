using FluentValidation;

namespace Kitchen.Application.Orders.AcceptOrder;

public class AcceptOrderCommandValidator : AbstractValidator<AcceptOrderCommand>
{
    public AcceptOrderCommandValidator()
    {
        RuleFor(command => command.OrderId)
            .NotEmpty().WithMessage("Order ID is required.");
    }
}
