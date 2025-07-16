using Microsoft.EntityFrameworkCore;
using Ordering_App.Models;
using Ordering_App.Models.Domain;

namespace Ordering_App.Context
{
    public class DataContext : DbContext
    {
        public DataContext( DbContextOptions<DataContext> options) : base(options) { 
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //1-1 relation between Restaurant and ManuItem
            modelBuilder.Entity<Restaurant>()
                .HasMany(x => x.MenuItems)
                .WithOne(x => x.Restaurant)
                .HasForeignKey(x => x.RestaurantId)
                .IsRequired();

            //1-1 relation between Emoloyee and Order
            modelBuilder.Entity<Employee>()
                .HasMany(x => x.Orders)
                .WithOne(x => x.Employee)
                .HasForeignKey(x => x.EmployeeId)
                .IsRequired();

            //1-1 relation between OrderItem and Order
            modelBuilder.Entity<Order>()
                .HasMany(x => x.OrderItems)
                .WithOne(x => x.Order)
                .HasForeignKey(x => x.OrderId)
                .IsRequired();

            ////Or
            //modelBuilder.Entity<OrderItem>()
            //    .HasOne(x => x.Order)
            //    .WithMany(x => x.OrderItems)
            //    .HasForeignKey(x => x.OrderId)
            //    .IsRequired();

            //1-1 relation between OrderItem and MenuItem
            modelBuilder.Entity<MenuItem>()
                .HasMany(x => x.OrderItems)
                .WithOne(x => x.MenuItem)
                .HasForeignKey(x => x.MenuItemId)
                .IsRequired();
        }
    }
}
