using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrinkAndGo.Models
{
    public class ShoppingCart
    {
        public string ShoppingCartId { get; set; }
        public ICollection<ShoppingCartItem> ShoopingCartItems { get; set; }
    }
}
