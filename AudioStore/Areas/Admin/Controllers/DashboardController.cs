using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using AudioStore.Models;

namespace AudioStore.Areas.Admin.Controllers
{
    public class DashboardController : BaseAdminController
    {
        private DB_AudioStoreEntities db = new DB_AudioStoreEntities();

        // GET: Admin/Dashboard/Index
        public ActionResult Index()
        {
            var allOrders = db.Orders
                .Include(o => o.Users)
                .Include(o => o.OrderDetails)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            ViewBag.TotalOrders = allOrders.Count;
            ViewBag.PendingOrders = allOrders.Count(o => o.Status == "Pending" || o.Status == "Chờ xử lý");
            ViewBag.TotalRevenue = allOrders
                .Where(o => o.Status != "Cancelled" && o.Status != "Đã hủy")
                .Sum(o => (decimal?)o.FinalAmount) ?? 0m;
            ViewBag.TotalProducts = db.Products.Count();

            return View(allOrders);
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