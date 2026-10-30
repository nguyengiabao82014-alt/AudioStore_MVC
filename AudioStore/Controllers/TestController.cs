using System;
using System.Linq;
using System.Web.Mvc;
using AudioStore.Models; // Đảm bảo namespace này khớp với project của bạn

namespace AudioStore.Controllers
{
    public class TestController : Controller
    {
        private DB_AudioStoreEntities db = new DB_AudioStoreEntities();

        // GET: Test
        public ActionResult Index()
        {
            // Lấy toàn bộ sản phẩm, .Include("Categories") để lấy được thông tin Loại sản phẩm
            var products = db.Products.Include("Categories").ToList();
            return View(products);
        }
    }
}