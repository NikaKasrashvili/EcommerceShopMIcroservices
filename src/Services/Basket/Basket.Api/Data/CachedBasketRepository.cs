using Microsoft.Extensions.Caching.Distributed;

namespace Basket.API.Data;

/// <summary>
/// works as a proxy. Implemented Decorator pattern.
/// </summary>
/// <param name="_repository"></param>
public class CachedBasketRepository(
    IBasketRespository _repository, 
    IDistributedCache _cache) 
    : IBasketRespository
{
    public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
    {
        var cachedBasket = await _cache.GetStringAsync(userName, cancellationToken);
        if (!string.IsNullOrEmpty(cachedBasket))
            return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;
        
        var basket = await _repository.GetBasket(userName, cancellationToken);
        await _cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket), cancellationToken);
        return basket;
    }

    public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
    {
        await _repository.StoreBasket(basket, cancellationToken);
        await _cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket), cancellationToken);

        return basket;
    }

    public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
    {
        await _repository.DeleteBasket(userName, cancellationToken);
        await _cache.RemoveAsync(userName, cancellationToken);

        return true;
    }
}
