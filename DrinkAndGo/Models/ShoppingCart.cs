using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
        public ICollection<ShoppingCartItem> ShoopingCartItems { get; set; }
      

        public static ShoppingCart GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext.Session;
            var context = services.GetService<DrinkAndGoContext>();
            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();
            session.SetString("CartId",cartId);
            return new ShoppingCart(context) {ShoppingCartId = cartId};
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

        //public int RemoveFromCart(Drink drink)
        //{
        //    var shoppingCartItem =
        //        _drinkAndGoContext.ShoppingCartItem.SingleOrDefault(
        //            s => s.Drink.DrinkId == drink.DrinkId && s.ShoppingCartId == ShoppingCartId);
        //    var local =
        //}

    }
}
