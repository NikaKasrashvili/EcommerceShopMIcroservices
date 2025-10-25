using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services;

public class DiscountService
    (DiscountDbContext _dbContext,
    ILogger<DiscountService> _logger
    ) : DiscountProtoService.DiscountProtoServiceBase
{
    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await _dbContext.Coupons
            .FirstOrDefaultAsync(c => c.ProductName == request.ProductName);

        coupon ??= new Coupon
        {
            ProductName = "No Discount",
            Amount = 0,
            Description = "No Discount for this product"
        };

        _logger.LogInformation("Discount is retrieved for ProductName : {ProductName}, Amount : {Amount}",
            coupon.ProductName, coupon.Amount);

        return coupon.Adapt<CouponModel>();
    }

    public override Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        return base.CreateDiscount(request, context);
    }

    public override Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        return base.UpdateDiscount(request, context);
    }

    public override Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        return base.DeleteDiscount(request, context);
    }
}
