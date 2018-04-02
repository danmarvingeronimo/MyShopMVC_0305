using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyShopMVC.Models
{
    public class CartModel
    {
        public int DetailID { get; set; }
        public int ProductID { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }

    }
}