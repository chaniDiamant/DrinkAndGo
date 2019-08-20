using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DrinkAndGo.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string UserName { get; set; }

        public int Age { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Role { get; set; }


        public List<Cart_Drink> Cart { get; set; } // will hold the connections between a user and a drink

        //public Dictionary<int, int> DrinkToCount { get; set; } // dict[drinkID] = drinkCount


        //public Cart Cart { get; set; }

        public User()
        {
            Cart = new List<Cart_Drink>();
            //DrinkToCount = new Dictionary<int, int>();
        }

    }
}
