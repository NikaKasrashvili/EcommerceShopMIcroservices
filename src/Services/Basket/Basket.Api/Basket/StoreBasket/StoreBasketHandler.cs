using Discount.Grpc;

namespace Basket.API.Basket.StoreBasket;

public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketresult>;
public record StoreBasketresult(string UserName);

public class StoreBasketCommandHandler(IBasketRespository _repository, DiscountProtoService.DiscountProtoServiceClient _discountProto) 
    : ICommandHandler<StoreBasketCommand, StoreBasketresult>
{
    public async Task<StoreBasketresult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        foreach (var item in command.Cart.Items)
        {
            var coupon = await _discountProto.GetDiscountAsync(
                new GetDiscountRequest { ProductName = item.ProductName },
                cancellationToken: cancellationToken);

            item.Price -= coupon.Amount;
        }

        await _repository.StoreBasket(command.Cart, cancellationToken);
        return new StoreBasketresult(command.Cart.UserName);
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