using Microsoft.EntityFrameworkCore;
using Ordering_App.Models;
using Ordering_App.Models.Domain;

namespace Ordering_App.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
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



            // --- Seeding 5 Employees ---
            modelBuilder.Entity<Employee>().HasData(
                Enumerable.Range(1, 5).Select(i => new Employee
                {
                    Id = i,
                    Name = $"Employee {i}",
                    EmployeeNumber = Guid.NewGuid(),
                    Balance = 1000 + i * 250,
                    LastDepositMonth = "January 2025",
                    MonthlyDepositTotal = i * 250
                }).ToArray()
            );

            // --- Seeding 5 Restaurants ---
            modelBuilder.Entity<Restaurant>().HasData(
                Enumerable.Range(1, 5).Select(i => new Restaurant
                {
                    Id = i,
                    Name = $"Restaurant {i}",
                    LocationDescription = $"Location {i}",
                    ContactNumber = $"011000000{i}"
                }).ToArray()
            );

            // --- Seeding 5 MenuItems ---
            modelBuilder.Entity<MenuItem>().HasData(
                Enumerable.Range(1, 5).Select(i => new MenuItem
                {
                    Id = i,
                    Name = $"Dish {i}",
                    Description = $"Tasty dish number {i}",
                    Price = 100 + i * 25,
                    RestaurantId = (i % 5) + 1
                }).ToArray()
            );

            // --- Seeding 5 Orders ---
            modelBuilder.Entity<Order>().HasData(
                Enumerable.Range(1, 5).Select(i => new Order
                {
                    OrderId = i,
                    EmployeeId = (i % 5) + 1,
                    OrderDate = new DateTime(2025, 7, 16).AddDays(i),
                    TotalAmount = 250 * i,
                    Status = "Pending"
                }).ToArray()
            );

            // --- Seeding 5 OrderItems ---
            modelBuilder.Entity<OrderItem>().HasData(
                Enumerable.Range(1, 5).Select(i => new OrderItem
                {
                    Id = i,
                    OrderId = (i % 5) + 1,
                    MenuItemId = i,
                    Quantity = 1 + i % 3,
                    UnitPriceAtTimeOfOrder = 100 + i * 25
                }).ToArray()
            );
        }
    }
}
