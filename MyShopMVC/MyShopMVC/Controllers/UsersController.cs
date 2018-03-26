using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
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
    public class UsersController : Controller
    {
        public List<SelectListItem> GetUserTypes()
        {
            List<SelectListItem> list = new List<SelectListItem>();

            using (SqlConnection con = new SqlConnection(Helper.GetConnection()))
            {
                con.Open();
                string query = @"SELECT TypeID, UserType FROM Types ORDER BY UserType";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader data = cmd.ExecuteReader())
                    {
                        while (data.Read())
                        {
                            list.Add(new SelectListItem
                            {
                                Value = data["TypeID"].ToString(),
                                Text = data["UserType"].ToString()
                            });
                        }
                    }
                }
            }
            return list;
        }

        public ActionResult Add()
        {
            UsersModel record = new UsersModel();
            record.UserTypes = GetUserTypes();
            return View(record);
        }
        

        [HttpPost]
        public ActionResult Add(UsersModel record)
        {
            using (SqlConnection con = new SqlConnection(Helper.GetConnection()))
            {
                con.Open();
                string query = @"INSERT INTO users VALUES
                    (@TypeID, @Email, @Password, @LastName,
                    @FirstName, @Street, @Municipality,
                    @City, @Phone, @Mobile, @Status,
                    @DateAdded, @DateModified)"; // parameterized query
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@TypeID", record.TypeID);
                    cmd.Parameters.AddWithValue("@Email", record.Email);
                    cmd.Parameters.AddWithValue("@Password", record.Password);
                    cmd.Parameters.AddWithValue("@LastName", record.LN);
                    cmd.Parameters.AddWithValue("@FirstName", record.FN);
                    cmd.Parameters.AddWithValue("@Street", record.Street);
                    cmd.Parameters.AddWithValue("@Municipality", record.Municipality);
                    cmd.Parameters.AddWithValue("@City", record.City);
                    cmd.Parameters.AddWithValue("@Phone", record.Phone);
                    cmd.Parameters.AddWithValue("@Mobile", record.Mobile);
                    cmd.Parameters.AddWithValue("@Status", "Active");
                    cmd.Parameters.AddWithValue("@DateAdded", DateTime.Now);
                    cmd.Parameters.AddWithValue("@DateModified", DBNull.Value);
                    cmd.ExecuteNonQuery();
                    return RedirectToAction("Index");
                }
            }
        }

        // GET: Users
        public ActionResult Index()
        {
            var list = new List<UsersModel>();
            using (SqlConnection con = new SqlConnection(Helper.GetConnection()))
            {
                con.Open();
                string query = @"SELECT u.UserID, t.UserType, u.Email, u.LastName,
                    u.FirstName, u.Street, u.Municipality, u.City,
                    u.Phone, u.Mobile, u.Status
                    FROM Users u
                    INNER JOIN Types t ON u.TypeID = t.TypeID
                    WHERE u.Status!=@Status";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Status", "Archived");
                    using (SqlDataReader data = cmd.ExecuteReader())
                    {
                        while (data.Read())
                        {
                            list.Add(new UsersModel
                            {
                                ID = int.Parse(data["UserID"].ToString()),
                                UserType = data["UserType"].ToString(),
                                Email = data["Email"].ToString(),
                                LN = data["LastName"].ToString(),
                                FN = data["FirstName"].ToString(),
                                Street = data["Street"].ToString(),
                                Municipality = data["Municipality"].ToString(),
                                City = data["City"].ToString(),
                                Phone = data["Phone"].ToString(),
                                Mobile = data["Mobile"].ToString(),
                                Status = data["Status"].ToString()
                            });
                        }
                    }
                }
            }

            return View(list);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            UsersModel record = new UsersModel();
            record.UserTypes = GetUserTypes();

            using (SqlConnection con = new SqlConnection(Helper.GetConnection()))
            {
                con.Open();
                string query = @"SELECT TypeID, Email, LastName, FirstName,
                    Street, Municipality, City, Phone, Mobile
                    FROM Users
                    WHERE UserID=@UserID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserID", id);
                    using (SqlDataReader data = cmd.ExecuteReader())
                    {
                        if (data.HasRows) // record is existing
                        {
                            while (data.Read())
                            {
                                record.TypeID = int.Parse(data["TypeID"].ToString());
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
                        else
                        {
                            return RedirectToAction("Index");
                        }
                    }
                }
            }
        }

        [HttpPost]
        public ActionResult Edit(int? id, UsersModel record)
        {
            using (SqlConnection con = new SqlConnection(Helper.GetConnection()))
            {
                con.Open();
                string query = @"UPDATE Users SET TypeID=@TypeID, Email=@Email,
                    Password=@Password, LastName=@LastName, FirstName=@FirstName,
                    Street=@Street, Municipality=@Municipality, City=@City,
                    Phone=@Phone, Mobile=@Mobile, DateModified=@DateModified
                    WHERE UserID=@UserID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@TypeID", record.TypeID);
                    cmd.Parameters.AddWithValue("@Email", record.Email);
                    cmd.Parameters.AddWithValue("@Password", record.Password);
                    cmd.Parameters.AddWithValue("@LastName", record.LN);
                    cmd.Parameters.AddWithValue("@FirstName", record.FN);
                    cmd.Parameters.AddWithValue("@Street", record.Street);
                    cmd.Parameters.AddWithValue("@Municipality", record.Municipality);
                    cmd.Parameters.AddWithValue("@City", record.City);
                    cmd.Parameters.AddWithValue("@Phone", record.Phone);
                    cmd.Parameters.AddWithValue("@Mobile", record.Mobile);
                    cmd.Parameters.AddWithValue("@DateModified", DateTime.Now);
                    cmd.Parameters.AddWithValue("@UserID", id);
                    cmd.ExecuteNonQuery();
                    return RedirectToAction("Index");
                }
            }
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");

            using (SqlConnection con = new SqlConnection(Helper.GetConnection()))
            {
                con.Open();
                string query = @"UPDATE Users SET Status=@Status, 
                    DateModified=@DateModified
                    WHERE UserID=@UserID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Status", "Archived");
                    cmd.Parameters.AddWithValue("@DateModified", DateTime.Now);
                    cmd.Parameters.AddWithValue("@UserID", id);
                    cmd.ExecuteNonQuery();
                }

            }
            return RedirectToAction("Index");
        }

        public ActionResult PrintAll()
        {
            ReportDocument report = new ReportDocument();
            report.Load(Server.MapPath("~/Reports/rptUsers.rpt"));
            report.SetDatabaseLogon("limpinaj", "potato", "(local)", "myshopdb");
            // report.SetDatabaseLogon("", "", "(local)", "myshopdb");
            report.SetParameterValue("User", "John Doe");
            report.ExportToHttpResponse(ExportFormatType.PortableDocFormat, 
                System.Web.HttpContext.Current.Response,
                true, "Users Report");
            return RedirectToAction("Index");
        }
    }
}