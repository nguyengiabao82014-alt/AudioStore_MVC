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
                    // Vì form Đăng ký của bạn không có ô Username, ta lấy luôn Email làm Username
                    Username = Email,
                    Password = Password, // Lưu ý: Thực tế nên băm (Hash) mật khẩu trước khi lưu
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
                // Tra cứu bằng Username hoặc Email vì giao diện ghi "Email hoặc Tên đăng nhập"
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
                        return RedirectToAction("Index", "Products", new { area = "Admin" });
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
            Session.Clear(); // Xóa toàn bộ Session
            return RedirectToAction("Index", "Home");
        }
    }
}