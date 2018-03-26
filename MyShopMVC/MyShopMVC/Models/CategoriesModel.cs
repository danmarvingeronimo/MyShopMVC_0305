using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MyShopMVC.Models
{
    public class CategoriesModel
    {
        [Key]
        public int CatID { get; set; }

        [Display(Name = "Category")]
        public string Name { get; set; }

        public int TotalCount { get; set; }
    }

}