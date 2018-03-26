using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyShopMVC.Models
{
    public class TitlesModel
    {
        [Key]
        [Display(Name = "Book Title ID")]
        public int ID { get; set; }

        [Display(Name = "Book Name")]
        [Required(ErrorMessage = "This is required.")]
        [MaxLength(50, ErrorMessage = "Incorrect input.")]
        public string TitleName { get; set; }

        [Display(Name = "Price")]
        [DisplayFormat(DataFormatString = "{0: #,##0.00}")]
        [Range(0.01, 10000.00, ErrorMessage = "Input not in range.")]
        [DataType(DataType.Currency)]
        public decimal TitlePrice { get; set; }

        [Display(Name = "Publication Date")]
        [MaxLength(40, ErrorMessage = "Incorrect input.")]
        public DateTime PublicationDate { get; set; }

        [Display(Name = "Notes")]
        [MaxLength(200, ErrorMessage = "Incorrect input.")]
        public string Notes { get; set; }

        [Display(Name = "Publisher ID")]
        public int pubID { get; set; }

        public List<PublishersModel> Publishers { get; set; }

        [Display(Name = "Publisher")]
        public string Publisher { get; set; }

        [Display(Name = "Author ID")]

        public int authorID { get; set; }

        public List<AuthorsModel> Authors { get; set; }

        [Display(Name = "Author")]
        public string Author { get; set; }

    }
}