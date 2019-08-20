using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DrinkAndGo.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }
        public ICollection<Drink> Drinks { get; set; }

        public Cart()
        {
            Drinks = new LinkedList<Drink>();
        }
    }
}
