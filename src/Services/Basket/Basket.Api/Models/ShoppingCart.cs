using Marten.Schema;

namespace Basket.API.Models;

public class ShoppingCart
{
    public string UserName { get; set; } = default!;
    public List<ShoppingCartItem> Items { get; set; } = [];
    public decimal ToTalPrice => Items.Sum(x => x.Price * x.Quantity);

    public ShoppingCart(string userName)
    {
        UserName = userName;
    }

    public ShoppingCart()
    {
            
    }
}
