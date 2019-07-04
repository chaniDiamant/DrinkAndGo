using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrinkAndGo.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public decimal OrderTotal { get; set; }

    }
}
