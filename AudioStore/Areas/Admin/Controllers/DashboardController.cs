using System.Web.Mvc;
using AudioStore.Models;

namespace AudioStore.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        private DB_AudioStoreEntities db = new DB_AudioStoreEntities();

        private bool CheckAdmin()
        {
            if (Session["User"] is Users u && u.RoleID == 1) return true;
            return false;
        }

        // GET: Admin/Dashboard/Index
        public ActionResult Index()
        {
            if (!CheckAdmin()) return RedirectToAction("Login", "Account", new { area = "" });
            return View();
        }
    }
}