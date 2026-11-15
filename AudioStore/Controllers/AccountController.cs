using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AudioStore.Models;

namespace AudioStore.Controllers
{
    public class AccountController : Controller
    {
        private DB_AudioStoreEntities db = new DB_AudioStoreEntities();

        // 1. ĐĂNG KÝ (GET)
        public ActionResult Register()
        {
            return View();
        }

        // 1. ĐĂNG KÝ (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(string FullName, string Email, string Password, string ConfirmPassword)
        {
            if (ModelState.IsValid)
            {
                if (Password != ConfirmPassword)
                {
                    ViewBag.Error = "Mật khẩu xác nhận không khớp!";
                    return View();
                }

                var checkEmail = db.Users.FirstOrDefault(u => u.Email == Email || u.Username == Email);
                if (checkEmail != null)
                {
                    ViewBag.Error = "Email này đã được sử dụng!";
                    return View();
                }

                Users newUser = new Users
                {
                    FullName = FullName,
                    Email = Email,
                    Username = Email,
                    Password = Password,
                    RoleID = 2, // 2 = Customer
                    CreatedAt = DateTime.Now
                };

                db.Users.Add(newUser);
                db.SaveChanges();

                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        // 2. ĐĂNG NHẬP (GET)
        public ActionResult Login()
        {
            return View();
        }

        // 2. ĐĂNG NHẬP (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string UsernameOrEmail, string Password)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.FirstOrDefault(u =>
                    (u.Username == UsernameOrEmail || u.Email == UsernameOrEmail) &&
                    u.Password == Password);

                if (user != null)
                {
                    Session["User"] = user;
                    Session["UserID"] = user.UserID;
                    Session["FullName"] = user.FullName;
                    Session["RoleID"] = user.RoleID;

                    if (user.RoleID == 1) // 1 = Admin
                    {
                        // Xóa giỏ hàng nếu có vì Admin không mua hàng
                        Session["Cart"] = null;
                        Session["CheckoutNow"] = null;
                        return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                    }
                    else // 2 = Customer
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không chính xác!";
            }
            return View();
        }

        // 3. ĐĂNG XUẤT
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // 4. LỊCH SỬ ĐƠN HÀNG CỦA NGƯỜI MUA
        public ActionResult OrderHistory()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var currentUser = Session["User"] as Users;
            int userId = currentUser != null ? currentUser.UserID : Convert.ToInt32(Session["UserID"]);

            var orders = db.Orders
                .Include(o => o.OrderDetails)
                .Where(o => o.UserID == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            return View(orders);
        }

        // 5. XEM CHI TIẾT ĐƠN HÀNG ĐÃ ĐẶT (PHÍA NGƯỜI MUA)
        public ActionResult OrderDetails(int? id)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var currentUser = Session["User"] as Users;
            int userId = currentUser != null ? currentUser.UserID : Convert.ToInt32(Session["UserID"]);

            // Chỉ cho phép người mua xem đúng đơn hàng của chính mình
            var order = db.Orders
                .Include(o => o.OrderDetails.Select(od => od.Products))
                .Include(o => o.Users)
                .FirstOrDefault(o => o.OrderID == id.Value && o.UserID == userId);

            if (order == null)
            {
                return HttpNotFound("Không tìm thấy đơn hàng hoặc bạn không có quyền xem đơn hàng này.");
            }

            return View(order);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}