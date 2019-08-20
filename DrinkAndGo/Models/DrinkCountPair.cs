using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DrinkAndGo.Models
{
    public class DrinkCountPair
    {

        [Key]
        public int Id { get; set; }

        public Drink Drink { get; set; }

        public int Count { get; set; }
    }
}
