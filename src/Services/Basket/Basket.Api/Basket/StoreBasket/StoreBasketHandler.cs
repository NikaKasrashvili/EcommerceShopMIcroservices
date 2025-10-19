
namespace Basket.API.Basket.StoreBasket;

public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketresult>;
public record StoreBasketresult(string UserName);

public class StoreBasketCommandHandler : ICommandHandler<StoreBasketCommand, StoreBasketresult>
{
    public async Task<StoreBasketresult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

#region validator

public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
{
    public StoreBasketCommandValidator()
    {
        RuleFor(x => x.Cart).NotNull().WithMessage("Cart can not be null");        
        RuleFor(x => x.Cart.UserName).NotNull().WithMessage("UserName is required");        
    }
}
#endregion