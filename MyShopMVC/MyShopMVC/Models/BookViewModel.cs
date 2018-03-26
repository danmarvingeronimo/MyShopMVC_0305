using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyShopMVC.Models
{
    public class BookViewModel
    {
        public List<TitlesModel> BookTitle { get; set; }
        public List<AuthorsModel> Authors { get; set; }
        public List<PublishersModel> Publishers { get; set; }

    }
}