using OrderProcessingBackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace OrderProcessingBackEnd.Data
{
    public class OrderProcessingDbContext : DbContext
    {
        public OrderProcessingDbContext(DbContextOptions<OrderProcessingDbContext> options) : base(options) { }

        public DbSet<Users> Users { get; set; }
        public DbSet<Customers> Customers { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<OrderProducts> OrderProduct { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<Transactions> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // One-to-one: User ↔ Customer
            modelBuilder.Entity<Users>()
                .HasOne(u => u.Customer)
                .WithOne(c => c.User)
                .HasForeignKey<Customers>(c => c.UserID)
                .OnDelete(DeleteBehavior.SetNull);

            // One-to-many: Customer → Orders
            modelBuilder.Entity<Orders>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Order)
                .HasForeignKey(o => o.CustomerID)
                .OnDelete(DeleteBehavior.Cascade);

            // Property config for TotalAmount in Orders
            modelBuilder.Entity<Orders>()
                .HasKey(o => o.OrderID);

            modelBuilder.Entity<Orders>()
                .Property(o => o.TotalAmount)
                .HasColumnType("decimal(10,2)")
                .IsRequired()
                .HasDefaultValue(0);



            // One-to-many: Order → Transactions
            modelBuilder.Entity<Transactions>()
                .HasOne(t => t.Order)
                .WithMany(o => o.Transaction)
                .HasForeignKey(t => t.OrderID)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-many: Category → Products
            modelBuilder.Entity<Category>()
                .HasMany(ct => ct.Product)
                .WithOne(p => p.Category)
                .OnDelete(DeleteBehavior.SetNull);

            

            // Product table key and index
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.ProductID);

                entity.Property(p => p.Price)
                      .HasPrecision(10, 2);
            });

            // OrderProducts config
            modelBuilder.Entity<OrderProducts>()
                .HasKey(op => op.OrderProductID);

            modelBuilder.Entity<OrderProducts>()
                .HasOne(op => op.Order)
                .WithMany(o => o.OrderProducts)
                .HasForeignKey(op => op.OrderID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderProducts>()
                .HasOne(op => op.Product)
                .WithMany(p => p.OrderProducts)
                .HasForeignKey(op => op.ProductID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderProducts>()
                .Property(op => op.PriceAtPurchase)
                .HasPrecision(10, 2);
        }
    }
}
