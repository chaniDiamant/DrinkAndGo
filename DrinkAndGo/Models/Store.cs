using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DrinkAndGo.Models
{
    public class Store
    {
        [Key]
        public string Name { get; set; }

        public string Adress { get; set; }
    }
}
