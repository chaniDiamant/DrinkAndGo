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

        public virtual ICollection<DrinkCountPair> DrinksWithCount { get; set; }

        public string UserForeignKey { get; set; }
        public User User { get; set; }

        public Cart()
        {
            DrinksWithCount = new LinkedList<DrinkCountPair>();
        }
    }
}
