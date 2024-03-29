﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DrinkAndGo.Models;

namespace DrinkAndGo.Models
{
    public class DrinkAndGoContext : DbContext
    {
        public DrinkAndGoContext (DbContextOptions<DrinkAndGoContext> options)
            : base(options)
        {
        }

        public DbSet<DrinkAndGo.Models.Category> Category { get; set; }

        public DbSet<DrinkAndGo.Models.Drink> Drink { get; set; }

        public DbSet<DrinkAndGo.Models.ShoppingCart> ShoppingCart { get; set; }

        public DbSet<DrinkAndGo.Models.ShoppingCartItem> ShoppingCartItem { get; set; }

        public DbSet<DrinkAndGo.Models.User> User { get; set; }

        public DbSet<DrinkAndGo.Models.Order> Order { get; set; }

        public DbSet<DrinkAndGo.Models.OrderDetail> OrderDetail { get; set; }
    }
}
