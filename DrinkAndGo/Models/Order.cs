using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrinkAndGo.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string ZipCode { get; set; }
        public string PostalAddress { get; set; }
        public string PaymentCard { get; set; }
        public string Cvv { get; set; }

    }
}
