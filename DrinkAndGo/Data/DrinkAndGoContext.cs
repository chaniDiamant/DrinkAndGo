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

        public DbSet<Drink> Drink { get; set; }

        public DbSet<User> User { get; set; }


        public DbSet<Store> Store { get; set; }

        public DbSet<Cart> Cart { get; set; }
        public DbSet<DrinkCountPair> DrinkCountPairs { get; set; }

        internal static Task<string> ToListAsync()
        {
            throw new NotImplementedException();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(p => p.Cart)
                .WithOne(i => i.User)
                .HasForeignKey<Cart>(b => b.UserForeignKey);
        }
    }
}
