using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyShopMVC.Models
{
    public class AuthorsModel
    {
        [Key]
        [Display(Name = "Author ID")]
        public int AuthID { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "This is required.")]
        [MaxLength(50, ErrorMessage = "Incorrect input.")]
        public string LN { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "This is required.")]
        [MaxLength(100, ErrorMessage = "Incorrect input.")]
        public string FN { get; set; }
        [Display(Name = "Address")]
        [MaxLength(40, ErrorMessage = "Incorrect input.")]
        public string Address { get; set; }
        [Display(Name = "State")]
        [MaxLength(2, ErrorMessage = "Incorrect input.")]

        public string State { get; set; }

        [Display(Name = "City")]
        [MaxLength(20, ErrorMessage = "Incorrect input.")]
        public string City { get; set; }


        [Display(Name = "Zip")]
        [MaxLength(50, ErrorMessage = "Incorrect input.")]
        public string Zip { get; set; }
        
    }
}