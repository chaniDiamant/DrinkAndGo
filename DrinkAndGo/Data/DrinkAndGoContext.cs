using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DrinkAndGo.Models;

namespace DrinkAndGo.Models
{
    public class DrinkAndGoContext : DbContext
    {
        public DrinkAndGoContext(DbContextOptions<DrinkAndGoContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Category { get; set; }

        public DbSet<User> User { get; set; }
        public DbSet<Drink> Drink { get; set; }
        public DbSet<Cart_Drink> Cart_Drinks { get; set; }



        public DbSet<Store> Store { get; set; }

        internal static Task<string> ToListAsync()
        {
            throw new NotImplementedException();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cart_Drink>()
            .HasKey(pt => new { pt.UserId, pt.DrinkId });

            modelBuilder.Entity<Cart_Drink>()
                .HasOne(pt => pt.User)
                .WithMany(p => p.Cart)
                .HasForeignKey(pt => pt.UserId);

            modelBuilder.Entity<Cart_Drink>()
                .HasOne(pt => pt.Drink)
                .WithMany(t => t.Cart)
                .HasForeignKey(pt => pt.DrinkId);
        }

        public DbSet<DrinkAndGo.Models.Order> Order { get; set; }
    }
}
