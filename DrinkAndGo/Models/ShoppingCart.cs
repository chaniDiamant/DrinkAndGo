using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace DrinkAndGo.Models
{
    public class ShoppingCart
    {
        private readonly DrinkAndGoContext _drinkAndGoContext;


        private ShoppingCart(DrinkAndGoContext drinkAndGoContext)
        {
            _drinkAndGoContext = drinkAndGoContext;
        }

        public string ShoppingCartId { get; set; }
        public ICollection<ShoppingCartItem> ShoppingCartItems { get; set; }


        public static ShoppingCart GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext.Session;
            var context = services.GetService<DrinkAndGoContext>();
            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();
            session.SetString("CartId", cartId);
            return new ShoppingCart(context) { ShoppingCartId = cartId };
        }

        public void AddToCart(Drink drink, int amount)
        {
            var shoppingCartItem = _drinkAndGoContext.ShoppingCartItem.SingleOrDefault(
                s => s.Drink.DrinkId == drink.DrinkId && s.ShoppingCartId == ShoppingCartId);
            if (shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem
                {
                    ShoppingCartId = ShoppingCartId,
                    Drink = drink,
                    Amount = 1
                };
                _drinkAndGoContext.ShoppingCartItem.Add(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.Amount++;
            }

            _drinkAndGoContext.SaveChanges();
        }

        public int RemoveFromCart(Drink drink)
        {
            var shoppingCartItem =
                _drinkAndGoContext.ShoppingCartItem.SingleOrDefault(
                    s => s.Drink.DrinkId == drink.DrinkId && s.ShoppingCartId == ShoppingCartId);
            var localAmount = 0;
            if (shoppingCartItem != null)
            {
                if (shoppingCartItem.Amount > 1)
                {
                    shoppingCartItem.Amount--;
                    localAmount = shoppingCartItem.Amount;
                }
                else
                {
                    _drinkAndGoContext.ShoppingCartItem.Remove(shoppingCartItem);
                }
            }
            _drinkAndGoContext.SaveChanges();
            return localAmount;
        }
        public ICollection<ShoppingCartItem> GetShoppingCartItems()
        {
            return ShoppingCartItems ??
                (ShoppingCartItems = _drinkAndGoContext.ShoppingCartItem.Where(c => c.ShoppingCartId == ShoppingCartId)
                  .Include(s => s.Drink).ToList());
        }
        public void ClearCart()
        {
            var CartItems = _drinkAndGoContext.ShoppingCartItem.Where(cart => cart.ShoppingCartId == ShoppingCartId);
            _drinkAndGoContext.ShoppingCartItem.RemoveRange(CartItems);
            _drinkAndGoContext.SaveChanges();

        }
        public decimal GetShoppingCartTotal()
        {
            var total = _drinkAndGoContext.ShoppingCartItem.Where(c => c.ShoppingCartId == ShoppingCartId)
                .Select(c => c.Drink.Price * c.Amount).Sum();
            return total;
        }
    }
}

