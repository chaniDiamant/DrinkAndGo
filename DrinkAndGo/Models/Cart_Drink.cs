using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrinkAndGo.Models
{
    public class Cart_Drink
    {   //many to many
        public int UserId { get; set; }
        public User User { get; set; }

        public int DrinkId { get; set; }
        public Drink Drink { get; set; }

        public int DrinkCount { get; set; } 
    }
}
