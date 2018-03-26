using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyShopMVC.Models
{
    public class ProductsModel
    {
        [Key]
        [Display(Name="Product ID")]
        public int ID { get; set; }

        [Display(Name="Name")]
        [Required(ErrorMessage ="Required")]
        [MaxLength(100, ErrorMessage ="Invalid input length.")]
        public string Name { get; set; }

        [Display(Name="Category")]
        [Required(ErrorMessage ="Select from list.")]
        public int CatID { get; set; }

        public List<CategoriesModel> Categories { get; set; }

        [Display(Name="Category")]
        public string Category { get; set; }

        [Display(Name="Code")]
        [MaxLength(20, ErrorMessage ="Invalid input length.")]
        public string Code { get; set; }

        [Display(Name="Description")]
        [Required(ErrorMessage ="Required")]
        [MaxLength(300, ErrorMessage ="Invalud input length")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name="Image")]
        [DataType(DataType.Upload)]
        public string Image { get; set; }

        [Display(Name="Price")]
        [DisplayFormat(DataFormatString = "{0: #,##0.00}")]
        [Range(0.01, 10000.00, ErrorMessage ="Input not in range.")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Display(Name="Is Featured?")]
        public bool IsFeatured { get; set; }
        [Display(Name="Available")]
        public int Available { get; set; }

        [Display(Name="Critical Level")]
        [Required(ErrorMessage ="Required")]
        [Range(0, 1000, ErrorMessage ="Input not in range")]
        public int Critical { get; set; }

        [Display(Name = "Max Level")]
        [Required(ErrorMessage = "Required")]
        [Range(0, 1000, ErrorMessage = "Input not in range")]
        public int Max { get; set; }

        [Display(Name="Status")]
        [Required(ErrorMessage ="Select from list.")]
        public string Status { get; set; }
        public string AllStatus { get; set; }
        
        [Display(Name="Date Added")]
        public DateTime DateAdded { get; set; }

        [Display(Name="Date Modified")]
        [DisplayFormat(NullDisplayText = "")]
        public DateTime? DateModified { get; set; }
    }
}