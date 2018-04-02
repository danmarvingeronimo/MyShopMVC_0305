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
    public class AccountController : Controller
    {
        /// <summary>
        /// Allows to check if email is existing from users table
        /// </summary>
        /// <param name="email">User input</param>
        /// <returns>Existing record</returns>
        bool IsExisting(string email)
        {
            using (SqlConnection con = new SqlConnection(Helper.GetConnection()))
            {
                con.Open();
                string query = @"SELECT Email FROM Users
                    WHERE Email=@Email";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    return cmd.ExecuteScalar() == null ? false : true;
                }
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(UsersModel record)
        {
            if (IsExisting(record.Email))
            {
                ViewBag.Error = "<div class='alert alert-danger'>Email address already existing!</div>";
                return View();
            }
            else
            {
                using (SqlConnection con = new SqlConnection(Helper.GetConnection()))
                {
                    con.Open();
                    string query = @"INSERT INTO Users VALUES (@TypeID, @Email,
                        @Password, @LastName, @FirstName, @Street,
                        @Municipality, @City, @Phone, @Mobile, @Status,
                        @DateAdded, @DateModified);";
                    using (SqlCommand cmd = new SqlCommand(query, con)) 
                    {
                        cmd.Parameters.AddWithValue("@TypeID", 5);
                        cmd.Parameters.AddWithValue("@Email", record.Email);
                        cmd.Parameters.AddWithValue("@Password", Helper.Hash(record.Password));
                        cmd.Parameters.AddWithValue("@LastName", record.LN);
                        cmd.Parameters.AddWithValue("@FirstName", record.FN);
                        cmd.Parameters.AddWithValue("@Street", "");
                        cmd.Parameters.AddWithValue("@Municipality", "");
                        cmd.Parameters.AddWithValue("@City", "");
                        cmd.Parameters.AddWithValue("@Phone", "");
                        cmd.Parameters.AddWithValue("@Mobile", "");
                        cmd.Parameters.AddWithValue("@Status", "Pending");
                        cmd.Parameters.AddWithValue("@DateAdded", DateTime.Now);
                        cmd.Parameters.AddWithValue("@DateModified", DBNull.Value);
                        cmd.ExecuteNonQuery();

                        string message = "Hello, " + record.FN + " " + record.LN + "!<br/>" +
                            "You have created a new account.<br/>" +
                            "Here are your credentials: <br/>" +
                            "Email: " + record.Email + "<br/>" +
                            "Password: " + record.Password + "<br/><br/>" +
                            "Thank you!<br/><br/>" +
                            "<h3>The Administrator</h3>";
                        Helper.SendEmail(record.Email, "Account Created", message);
                        return RedirectToAction("Login");
                    }
                }
            }
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(UsersModel record)
        {
            /* Step 1. Check if email and password match
             * Step 2. If email and password don't match, display error.
             * Step 3. If email and password match, redirect to Profile page
             */
            using (SqlConnection con = new SqlConnection(Helper.GetConnection()))
            {
                con.Open();
                string query = @"SELECT UserID, TypeID FROM Users
                    WHERE Email=@Email AND Password=@Password
                    AND Status!=@Status";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Email", record.Email);
                    cmd.Parameters.AddWithValue("@Password", Helper.Hash(record.Password));
                    cmd.Parameters.AddWithValue("@Status", "Archived");
                    using (SqlDataReader data = cmd.ExecuteReader())
                    {
                        if (data.HasRows) // email and password match <3
                        {
                            while (data.Read())
                            {
                                Session["userid"] = data["UserID"].ToString();
                                Session["typeid"] = data["TypeID"].ToString();
                            }
                            return RedirectToAction("Profile");
                        }
                        else
                        {
                            ViewBag.Error =
                                "<div class='alert alert-danger col-lg-6'>Invalid credentials.</div>";
                            return View();
                        }
                    }
                }

            }
        }

        public ActionResult Profile()
        {
            if (Session["userid"] == null) // user has not logged in
                return RedirectToAction("Login");

            var record = new UsersModel();
            using (SqlConnection con = new SqlConnection(Helper.GetConnection()))
            {
                con.Open();
                string query = @"SELECT Email, LastName, FirstName,
                    Street, Municipality, City, Phone, Mobile,
                    DateModified FROM Users
                    WHERE UserID=@UserID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserID", Session["userid"].ToString());
                    using (SqlDataReader data = cmd.ExecuteReader())
                    {
                        while (data.Read())
                        {
                            record.Email = data["Email"].ToString();
                            record.LN = data["LastName"].ToString();
                            record.FN = data["FirstName"].ToString();
                            record.Street = data["Street"].ToString();
                            record.Municipality = data["Municipality"].ToString();
                            record.City = data["City"].ToString();
                            record.Phone = data["Phone"].ToString();
                            record.Mobile = data["Mobile"].ToString();
                        }
                        return View(record);
                    }
                }
            }
        }

        [HttpPost]
        public ActionResult Profile(UsersModel record)
        {
            using (SqlConnection con = new SqlConnection(Helper.GetConnection()))
            {
                con.Open();
                string query = @"UPDATE Users SET Email=@Email,
                    Password=@Password, LastName=@LastName,
                    FirstName=@FirstName, Street=@Street,
                    Municipality=@Municipality, City=@City,
                    Phone=@Phone, Mobile=@Mobile, DateModified=@DateModified
                    WHERE UserID=@UserID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Email", record.Email);
                    cmd.Parameters.AddWithValue("@Password", Helper.Hash(record.Password));
                    cmd.Parameters.AddWithValue("@LastName", record.LN);
                    cmd.Parameters.AddWithValue("@FirstName", record.FN);
                    cmd.Parameters.AddWithValue("@Street", record.Street);
                    cmd.Parameters.AddWithValue("@Municipality", record.Municipality);
                    cmd.Parameters.AddWithValue("@City", record.City);
                    cmd.Parameters.AddWithValue("@Phone", record.Phone);
                    cmd.Parameters.AddWithValue("@Mobile", record.Mobile);
                    cmd.Parameters.AddWithValue("@DateModified", DateTime.Now);
                    cmd.Parameters.AddWithValue("@UserID", Session["userid"].ToString());
                    cmd.ExecuteNonQuery();
                    ViewBag.Success =
                        "<div class='alert alert-success col-lg-6'>Profile updated.</div>";
                    return View(record);
                }
            }
        }

        double GetTotalAmount()
        {
            using (SqlConnection con = new SqlConnection(Helper.GetConnection()))
            {
                con.Open();
                string SQL = @"SELECT SUM(Amount) FROM OrderDetails WHERE OrderNo=@OrderNo AND UserID=@UserID HAVING COUNT(DetailID) > 0";
                
                using (SqlCommand cmd = new SqlCommand (SQL, con))
                {
                    cmd.Parameters.AddWithValue("@OrderNo", 0);
                    cmd.Parameters.AddWithValue("@UserID", 1);
                    return cmd.ExecuteScalar() == null ? 0 : Convert.ToDouble((decimal)cmd.ExecuteScalar());
                }
            }
        }


        public ActionResult Cart()
        {
            var list = new List<CartModel>();
            using (SqlConnection con = new SqlConnection(Helper.GetConnection()))
            {
                con.Open();
                string SQL = @"SELECT od.DetailID, p.ProductID, p.Image, p.Name, p.Code, c.Category, od.Price, od.Quantity, od.Amount 
                              FROM OrderDetails od INNER JOIN Products p ON od.ProductID = p.ProductID 
                                                   INNER JOIN Categories c ON p.CatID = c.CatID
                                                   WHERE od.OrderNo=@OrderNo AND od.UserId=@UserID";

                using (SqlCommand cmd = new SqlCommand(SQL, con))
                {
                    cmd.Parameters.AddWithValue("@OrderNo", 0);
                    cmd.Parameters.AddWithValue("@UserID", 1);

                    using (SqlDataReader data = cmd.ExecuteReader())
                    {
                        while(data.Read())
                        {
                            list.Add(new CartModel
                            {
                                DetailID = int.Parse(data["DetailID"].ToString()),
                                ProductID = int.Parse(data["ProductID"].ToString()),
                                Image = data["Image"].ToString(),
                                Name = data["Name"].ToString(),
                                Code = data["Code"].ToString(),
                                Category = data["Category"].ToString(),
                                Price = decimal.Parse(data["Price"].ToString()),
                                Quantity = int.Parse(data["Quantity"].ToString()),
                                Amount = decimal.Parse(data["Amount"].ToString())

                            });
                        }

                        ViewBag.Gross = (GetTotalAmount() * .88).ToString("#,##0.00");
                        ViewBag.VAT = (GetTotalAmount() * .12).ToString("#,##0.00");
                        ViewBag.Total = GetTotalAmount().ToString("#,##0.00");

                        return View(list);
                    }
                }
            }
  
        }
    }
}