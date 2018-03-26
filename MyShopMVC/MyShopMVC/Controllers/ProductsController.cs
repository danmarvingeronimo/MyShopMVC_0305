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
    public class ProductsController : Controller
    {
        public List<CategoriesModel> GetCategories()
        {
            var list = new List<CategoriesModel>();
            using (SqlConnection con = new SqlConnection(Helper.GetConnection()))
            {
                con.Open();
                string query = @"SELECT CatID, Category FROM Categories
                    ORDER BY Category";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader data = cmd.ExecuteReader())
                    {
                        while (data.Read())
                        {
                            list.Add(new CategoriesModel
                            {
                                CatID = int.Parse(data["CatID"].ToString()),
                                Name = data["Category"].ToString()
                            });
                        }
                    }
                }
            }
            return list;
        }

        public ActionResult Add()
        {
            ProductsModel record = new ProductsModel();
            record.Categories = GetCategories();
            return View(record);
        }
        [HttpPost]
        public ActionResult Add(ProductsModel record, HttpPostedFileBase image)
        {
            // Insert product record to products table
            using (SqlConnection con = new SqlConnection(Helper.GetConnection()))
            {
                con.Open();
                string query = @"INSERT INTO products VALUES
                    (@Name, @CatID, @Code, @Description,
                    @Image, @Price, @IsFeatured, @Available,
                    @Critical, @Maximum, @Status, @DateAdded, @DateModified)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Name", record.Name);
                    cmd.Parameters.AddWithValue("@CatID", record.CatID);
                    cmd.Parameters.AddWithValue("@Code", record.Code);
                    cmd.Parameters.AddWithValue("@Description", record.Description);
                    string fileName = DateTime.Now.ToString("yyyyMMddHHmmss-") +
                        image.FileName;
                    cmd.Parameters.AddWithValue("@Image", fileName);

                    // Upload the chosen file to images > products
                    image.SaveAs(Server.MapPath("~/Images/Products/" + fileName));

                    cmd.Parameters.AddWithValue("@Price", record.Price);
                    cmd.Parameters.AddWithValue("@IsFeatured", 
                        record.IsFeatured ? "Yes" : "No");
                    cmd.Parameters.AddWithValue("@Available", 0);
                    cmd.Parameters.AddWithValue("@Critical", record.Critical);
                    cmd.Parameters.AddWithValue("@Maximum", record.Max);
                    cmd.Parameters.AddWithValue("@Status", "Out of Stock");
                    cmd.Parameters.AddWithValue("@DateAdded", DateTime.Now);
                    cmd.Parameters.AddWithValue("@DateModified", DBNull.Value);
                    cmd.ExecuteNonQuery();
                    // Redirect to index
                    return RedirectToAction("Index");
                }
            }

            
           
        }

        // GET: Products
        public ActionResult Index()
        {
            var list = new List<ProductsModel>();

            using (SqlConnection con = new SqlConnection(Helper.GetConnection()))
            {
                con.Open();
                string query = @"SELECT p.ProductID, p.Name, c.Category,
                    p.Code, p.Description, p.Image, p.Price, p.IsFeatured,
                    p.Status, p.DateAdded, p.DateModified
                    FROM products AS p
                    INNER JOIN categories c ON p.CatID = c.CatID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader data = cmd.ExecuteReader())
                    {
                        while (data.Read())
                        {
                            list.Add(new ProductsModel
                            {
                                ID = int.Parse(data["ProductID"].ToString()),
                                Name = data["Name"].ToString(),
                                Category = data["Category"].ToString(),
                                Code = data["Code"].ToString(),
                                Description = data["Description"].ToString(),
                                Image = data["Image"].ToString(),
                                Price = decimal.Parse(data["Price"].ToString()),
                                IsFeatured = data["IsFeatured"].ToString() == "Yes" ? true : false,
                                Status = data["Status"].ToString(),
                                DateAdded = DateTime.Parse(data["DateAdded"].ToString()),
                                DateModified = data["DateModified"].ToString() == "" ?
                                    (DateTime?)null : DateTime.Parse(data["DateModified"].ToString())
                            });
                        }
                    }
                }
            }
                return View(list);
        }
    }
}