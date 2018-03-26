using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShopMVC.Models
{
    public class UsersModel
    {
        [Key]
        [Display(Name = "User ID")]
        public int ID { get; set; }

        [Display(Name ="User Type")]
        [Required(ErrorMessage = "This is required.")]
        public int TypeID { get; set; }
        public List<SelectListItem> UserTypes { get; set; }
        public string UserType { get; set; }

        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "This is required.")]
        [MaxLength(100, ErrorMessage ="Incorrect input.")]
        [DataType(DataType.EmailAddress, ErrorMessage ="Incorrect format.")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "This is required.")]
        [MaxLength(20, ErrorMessage = "Incorrect input.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "This is required.")]
        [MaxLength(50, ErrorMessage = "Incorrect input.")]
        public string LN { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "This is required.")]
        [MaxLength(100, ErrorMessage = "Incorrect input.")]
        public string FN { get; set; }
        [Display(Name ="Street")]
        [MaxLength(50, ErrorMessage = "Incorrect input.")]
        public string Street { get; set; }
        [Display(Name ="Municipality")]
        [MaxLength(50, ErrorMessage = "Incorrect input.")]
        [DataType(DataType.MultilineText)]
        public string Municipality { get; set; }

        [Display(Name ="City")]
        [MaxLength(20, ErrorMessage = "Incorrect input.")]
        public string City { get; set; }
        [Display(Name = "Phone #")]
        [MaxLength(12, ErrorMessage = "Incorrect input.")]
        public string Phone { get; set; }
        [Display(Name = "Mobile #")]
        [MaxLength(11, ErrorMessage = "Incorrect input.")]
        [RegularExpression(".{11}", ErrorMessage = "Incorrect format.")]
        public string Mobile { get; set; }
        [Display(Name = "Status")]
        public string Status { get; set; }
        [Display(Name = "Date Added")]
        public DateTime DateAdded { get; set; }
        [Display(Name = "Date Modified")]
        public DateTime? DateModified { get; set; }
    }
}