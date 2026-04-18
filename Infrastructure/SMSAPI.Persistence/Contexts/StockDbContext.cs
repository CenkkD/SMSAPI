using Microsoft.EntityFrameworkCore;
using SMSAPI.Domain.Entities;

namespace SMSAPI.Persistence.Contexts
{
    public class StockDbContext : DbContext
    {
        public StockDbContext(DbContextOptions<StockDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vehicle>().HasQueryFilter(v => !v.IsDeleted);
            modelBuilder.Entity<CarPart>().HasQueryFilter(cp => !cp.IsDeleted);
            modelBuilder.Entity<Order>().HasQueryFilter(o => !o.IsDeleted);
            modelBuilder.Entity<OrderItem>().HasQueryFilter(oi => !oi.IsDeleted);
            modelBuilder.Entity<Customer>().HasQueryFilter(c => !c.IsDeleted);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Vehicle)
                .WithMany(v => v.OrderItems)
                .HasForeignKey(oi => oi.VehicleId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.CarPart)
                .WithMany(cp => cp.OrderItems)
                .HasForeignKey(oi => oi.CarPartId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<CarPart> CarParts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Customer> Customers { get; set; }
    }
}
