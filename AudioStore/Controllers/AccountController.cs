using System;
using System.Linq;
using System.Web.Mvc;
using AudioStore.Models; // Thay đổi theo namespace Models của bạn

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
                // Kiểm tra Mật khẩu nhập lại
                if (Password != ConfirmPassword)
                {
                    ViewBag.Error = "Mật khẩu xác nhận không khớp!";
                    return View();
                }

                // Kiểm tra trùng lặp Email (hoặc Username)
                var checkEmail = db.Users.FirstOrDefault(u => u.Email == Email || u.Username == Email);
                if (checkEmail != null)
                {
                    ViewBag.Error = "Email này đã được sử dụng!";
                    return View();
                }

                // Tạo User mới
                Users newUser = new Users
                {
                    FullName = FullName,
                    Email = Email,
                    Username = Email, // Lấy luôn Email làm Username
                    Password = Password,
                    RoleID = 2, // 2 là Customer dựa theo DB của bạn
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
                    // Lưu thông tin vào Session
                    Session["User"] = user;
                    Session["FullName"] = user.FullName;
                    Session["RoleID"] = user.RoleID;

                    // Phân quyền chuyển hướng
                    if (user.RoleID == 1) // 1 là Admin
                    {
                        return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                    }
                    else // 2 là Customer
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
            Session["Cart"] = null; // Xóa giỏ hàng khi đăng xuất
            Session.Clear(); // Xóa toàn bộ Session
            return RedirectToAction("Index", "Home");
        }

        // 4. XEM LỊCH SỬ ĐƠN HÀNG (BỔ SUNG CHO NÚT "XEM LỊCH SỬ ĐƠN HÀNG")
        public ActionResult OrderHistory()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int currentUserId = 0;
            if (Session["User"] is Users loggedInUser)
            {
                currentUserId = loggedInUser.UserID;
            }
            else if (Session["UserID"] != null)
            {
                currentUserId = Convert.ToInt32(Session["UserID"]);
            }

            // Lấy danh sách đơn hàng của đúng user đang đăng nhập, sắp xếp mới nhất lên đầu
            var orders = db.Orders
                           .Where(o => o.UserID == currentUserId)
                           .OrderByDescending(o => o.OrderDate)
                           .ToList();

            return View(orders);
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