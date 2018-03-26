using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyShopMVC.Models
{
    public class StoreViewModel
    {
        public List<ProductsModel> Products { get; set; }

        public List<CategoriesModel> Categories { get; set; }
    }

}