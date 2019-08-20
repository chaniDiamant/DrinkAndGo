using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DrinkAndGo.Models
{
    public class User
    {
        [Key]
        public string UserName { get; set; }

        public int Age { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Role { get; set; }

        public Cart Cart { get; set; }

        public User()
        {
            this.Cart = new Cart();
            this.Cart.User = this;
        }

 
    }
}
