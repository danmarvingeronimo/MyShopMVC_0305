using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyShopMVC.Models
{
    public class PublishersModel
    {
        [Key]
        [Display(Name = "Publisher ID")]
        public int PubID { get; set; }

        [Display(Name = "Publisher Name")]
        [Required(ErrorMessage = "This is required.")]
        [MaxLength(50, ErrorMessage = "Incorrect input.")]
        public string PublisherName { get; set; }
        public int TotalCount { get; set; }



    }
}