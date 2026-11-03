using System.Linq;
using System.Web.Mvc;
using AudioStore.Models;

namespace AudioStore.Controllers
{
    public class HomeController : Controller
    {
        DB_AudioStoreEntities db = new DB_AudioStoreEntities();

        public ActionResult Index()
        {
            var dsSanPham = db.Products.ToList();
            return View(dsSanPham);
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
    }
}