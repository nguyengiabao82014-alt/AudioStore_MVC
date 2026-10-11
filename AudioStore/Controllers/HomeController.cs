using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AudioStore.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        // Trang Đăng nhập
        public ActionResult Login()
        {
            return View();
        }

        // Trang Đăng ký
        public ActionResult Register()
        {
            return View();
        }

        // Trang Giỏ hàng
        public ActionResult Cart()
        {
            return View();
        }
    }
}