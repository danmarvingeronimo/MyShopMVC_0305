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
    public class StoreController : Controller
    {
        public List<ProductsModel> GetProducts()
        {

            var list = new List<ProductsModel>();
            using (SqlConnection con = new SqlConnection(Helper.GetConnection()))
            {
                con.Open();
                string SQL = @"";
                if (Request.QueryString["c"] == null)
                {
                    SQL = "SELECT p.ProductID, p.Image, p.Name, p.Code, p.Price, c.Category FROM Products P INNER JOIN Categories C ON p.CatID = c.CatID ORDER BY p.Price";
                }

                else
                {
                    SQL = "SELECT p.ProductID, p.Image, p.Name, p.Code, p.Price, c.Category FROM Products P INNER JOIN Categories C ON p.CatID = c.CatID WHERE p.CatID=@CatID ORDER BY p.Price";

                }
                    
                using (SqlCommand cmd = new SqlCommand(SQL, con))
                {
                    cmd.Parameters.AddWithValue("@CatID", Request.QueryString["c"] == null ? "0" : Request.QueryString["c"].ToString());
                    using (SqlDataReader data = cmd.ExecuteReader())
                    {
                        while (data.Read())
                        {
                            list.Add(new ProductsModel
                            {
                                ID = int.Parse(data["ProductID"].ToString()),
                                Image = data["Image"].ToString(),
                                Name = data["Name"].ToString(),
                                Code = data["Code"].ToString(),
                                Category = data["Category"].ToString(),
                                Price = decimal.Parse(data["Price"].ToString())

                            });
                        }
                    }
                }


            }

            return list;
        }

        public List<CategoriesModel> GetCategories()
        {
            var list = new List<CategoriesModel>();
            using (SqlConnection con = new SqlConnection(Helper.GetConnection()))
            {
                con.Open();
                string SQL = @"SELECT c.CatID, c.Category,
                    (SELECT COUNT(p.ProductID) FROM Products p WHERE p.CatID = c.CatID) AS TotalCount
                    FROM Categories c ORDER BY c.Category";

                using (SqlCommand cmd = new SqlCommand(SQL, con))
                {
                    using (SqlDataReader data = cmd.ExecuteReader())
                    {
                        while (data.Read())
                        {
                            list.Add(new CategoriesModel
                            {
                                CatID = int.Parse(data["CatID"].ToString()),
                                Name = data["Category"].ToString(),
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
            var list = new StoreViewModel();
            list.Products = GetProducts();
            list.Categories = GetCategories();
            return View(list);

        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");

            }

            var record = new ProductsModel();
            using (SqlConnection con = new SqlConnection(Helper.GetConnection()))
            {
                con.Open();
                string SQL = @"SELECT p.Name, p.Code, p.Image, c.Category, p.Description, p.Price, p.CatID FROM Products p INNER JOIN Categories c ON p.CatID = c.CatID
                               WHERE p.ProductID = @ProductID";

                using (SqlCommand cmd = new SqlCommand(SQL, con))
                {
                    cmd.Parameters.AddWithValue("@ProductID", id);
                    using (SqlDataReader data = cmd.ExecuteReader())
                    {
                        if (data.HasRows)
                        {
                            while (data.Read())
                            {
                                record.Name = data["Name"].ToString();
                                record.Code = data["Code"].ToString();
                                record.Image = data["Image"].ToString();
                                record.Category = data["Category"].ToString();
                                record.Description = data["Description"].ToString();
                                record.Price = decimal.Parse(data["Price"].ToString());
                                record.CatID = int.Parse(data["CatID"].ToString());
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

        bool IsExisting(int productID)
        {
            using (SqlConnection con = new SqlConnection(Helper.GetConnection()))
            {
                con.Open();
                string SQL = @"SELECT ProductID FROM OrderDetails WHERE OrderNo=@OrderNO AND UserID=@UserID AND ProductID=@ProductID";
                using (SqlCommand cmd = new SqlCommand(SQL, con))
                {
                    cmd.Parameters.AddWithValue("@OrderNo", 0); //or DBNull Value
                    cmd.Parameters.AddWithValue("@UserID", 1); // or Session["userid"].ToString
                    cmd.Parameters.AddWithValue("@ProductID", productID);
                    return cmd.ExecuteScalar() == null ? false : true;
                }
            }
        }

        void AddToCart(int productID, int quantity)
        {
            double price = GetPrice(productID);
            bool productExisting = IsExisting(productID);

            using (SqlConnection con = new SqlConnection(Helper.GetConnection()))
            {
                con.Open();
                string SQL = "";
                if(productExisting)
                {
                    SQL = @"UPDATE OrderDetails SET Quantity = Quantity +@Quantity, Amount = Amount + @Amount
                                WHERE OrderNo=@OrderNo AND UserID=@UserID AND ProductID=@ProductID";
                }
                else
                {
                    SQL = @"INSERT INTO OrderDetails VALUES (@OrderNo, @UserID, @ProductId, @Quantity, @Price, @Amount, @Status)";
                }
                    
                    
                   
                using (SqlCommand cmd = new SqlCommand(SQL, con))
                {
                    cmd.Parameters.AddWithValue("@OrderNo", 0); //or DbNull.Value
                    cmd.Parameters.AddWithValue("@UserID", 1); //or Session["userid"].ToString()
                    cmd.Parameters.AddWithValue("@ProductID", productID);
                    cmd.Parameters.AddWithValue("@Quantity", quantity);
                    cmd.Parameters.AddWithValue("@Price", price);
                    cmd.Parameters.AddWithValue("@Amount", quantity * price);
                    cmd.Parameters.AddWithValue("@Status", "Pending");
                    cmd.ExecuteNonQuery();

                }
            }
        }

        public ActionResult AddToCart(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");

            AddToCart((int)id, 1);
            return RedirectToAction("Index");
        }
    }
}