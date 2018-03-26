using MyShopMVC.App_Code;
using MyShopMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShopMVC.Controllers
{
    public class BookController : Controller
    {
        // GET: Book
        public List<TitlesModel> GetBooks()
        {

            var list = new List<TitlesModel>();
            using (SqlConnection con = new SqlConnection(Helper.GetConnection()))
            {
                con.Open();
                string SQL = @"SELECT t.titleID, p.pubID, a.authorID, t.titleName, t.titlePrice, t.titlePubDate, t.titleNotes FROM titles t INNER JOIN publishers p ON t.pubID = p.pubID 
                               INNER JOIN authors a ON t.authorID = a.authorID";

                using (SqlCommand cmd = new SqlCommand(SQL, con))
                {

                    using (SqlDataReader data = cmd.ExecuteReader())
                    {
                        while (data.Read())
                        {
                            list.Add(new TitlesModel
                            {
                                ID = int.Parse(data["titleID"].ToString()),
                                TitleName = data["titleName"].ToString(),
                                Notes = data["titleNotes"].ToString(),
                                PublicationDate = DateTime.Parse(data["titlePubDate"].ToString()),
                                TitlePrice = decimal.Parse(data["titlePrice"].ToString())

                            });
                        }
                    }
                }


            }

            return list;
        }

        public List<PublishersModel> GetPublishers()
        {
            var list = new List<PublishersModel>();
            using (SqlConnection con = new SqlConnection(Helper.GetConnection()))
            {
                con.Open();
                string SQL = @"SELECT p.pubID, p.pubName,
                    (SELECT COUNT(t.titleID) FROM titles t WHERE t.pubID = p.pubID) AS TotalCount
                    FROM publishers p ORDER BY p.pubName";

                using (SqlCommand cmd = new SqlCommand(SQL, con))
                {
                    using (SqlDataReader data = cmd.ExecuteReader())
                    {
                        while (data.Read())
                        {
                            list.Add(new PublishersModel
                            {
                                PubID = int.Parse(data["pubID"].ToString()),
                                PublisherName = data["pubName"].ToString(),
                                TotalCount = int.Parse(data["TotalCount"].ToString())

                            });
                        }
                    }
                    return list;
                }
            }
        }
        public ActionResult Index()
        {
            var list = new BookViewModel();
            list.BookTitle = GetBooks();
            list.Publishers = GetPublishers();
            return View(list);

        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");

            }

            var record = new TitlesModel();
            using (SqlConnection con = new SqlConnection(Helper.GetConnection()))
            {
                con.Open();
                string SQL = @"SELECT t.titleName, t.titlePrice, t.titlePubDate, t.titleNotes, p.pubID, a.authorID ROM titles t INNER JOIN publishers p ON t.pubID = p.pubID 
                               INNER JOIN authors a ON t.authorID = a.authorID
                               WHERE t.titleID = @TitleID";

                using (SqlCommand cmd = new SqlCommand(SQL, con))
                {
                    cmd.Parameters.AddWithValue("@Title", id);
                    using (SqlDataReader data = cmd.ExecuteReader())
                    {
                        if (data.HasRows)
                        {
                            while (data.Read())
                            {
                                record.TitleName = data["titleName"].ToString();
                                record.Author = data["authorID"].ToString();
                                record.Publisher = data["Image"].ToString();
                                //record.Category = data["Category"].ToString();
                                //record.Description = data["Description"].ToString();
                                //record.TitlePrice = decimal.Parse(data["titlePrice"].ToString());
                                //record.CatID = int.Parse(data["CatID"].ToString());
                            }

                            return View(record);
                        }
                        else
                        {
                            return RedirectToAction("Index");
                        }
                    }
                }
            }
        }

        double GetPrice(int productID)
        {
            using (SqlConnection con = new SqlConnection(Helper.GetConnection()))
            {
                con.Open();
                string SQL = @"SELECT Price FROM Products WHERE ProductID=@ProductID";
                using (SqlCommand cmd = new SqlCommand(SQL, con))
                {
                    cmd.Parameters.AddWithValue("@ProductID", productID);
                    return Convert.ToDouble((decimal)cmd.ExecuteScalar());
                }
            }
        }
    }
}