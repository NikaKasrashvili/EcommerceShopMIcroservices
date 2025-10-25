using Discount.Grpc.Models;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data;

public class DiscountDbContext : DbContext
{
    public DiscountDbContext(DbContextOptions<DiscountDbContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Coupon>().HasData(
            new Coupon {Id = 1, Amount = 150, ProductName = "IPhone XVII", Description = "IPhone Discount" },
            new Coupon {Id = 2, Amount = 123, ProductName = "Sony", Description = "Sony Discount" }
            );

    }

    public DbSet<Coupon> Coupons { get; set; } = default!;
}
