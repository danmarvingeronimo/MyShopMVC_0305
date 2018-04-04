using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyShopMVC.Models
{
    public class CheckoutViewModel
    {
        public List<UsersModel> User { get; set; }

        public List<CartModel> Cart { get; set; }

        public string PaymentMethod { get; set; }


         
    }
}