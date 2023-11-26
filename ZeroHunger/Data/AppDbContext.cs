using System;
using Microsoft.EntityFrameworkCore;
using ZeroHunger.Models;

namespace ZeroHunger.Data
{
	public class AppDbContext : DbContext
	{
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<FoodRequest> FoodRequests { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<FoodAssign> FoodAssigns { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Restaurant>()
                .HasOne(r => r.User)
                .WithOne(u => u.Restaurant)
                .HasForeignKey<Restaurant>(r => r.UserId);

            modelBuilder.Entity<FoodRequest>()
                .HasOne(fr => fr.Restaurant)
                .WithMany(r => r.FoodRequests)
                .HasForeignKey(fr => fr.RestaurantId);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.User)
                .WithOne(u => u.Employee)
                .HasForeignKey<Employee>(e => e.UserId);

            modelBuilder.Entity<FoodAssign>()
                .HasOne(e => e.User)
                .WithMany(r => r.FoodAssigns)
                .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<FoodAssign>()
                .HasOne(fa => fa.FoodRequest)
                .WithOne(u => u.FoodAssign)
                .HasForeignKey<FoodAssign>(fa => fa.FoodRequestId);
        }
    }
}

