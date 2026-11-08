using System;
using System.Linq;
using System.Web.Mvc;
using AudioStore.Models;

namespace AudioStore.Areas.Admin.Controllers
{
    public class DiscountCodesController : Controller
    {
        private DB_AudioStoreEntities db = new DB_AudioStoreEntities();

        private bool CheckAdmin()
        {
            if (Session["User"] is Users u && u.RoleID == 1) return true;
            return false;
        }

        public ActionResult Index()
        {
            if (!CheckAdmin()) return RedirectToAction("Login", "Account", new { area = "" });
            // Sắp xếp theo Code thay vì DiscountID
            var list = db.DiscountCodes.OrderByDescending(d => d.Code).ToList();
            return View(list);
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (!CheckAdmin()) return RedirectToAction("Login", "Account", new { area = "" });
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DiscountCodes model)
        {
            if (!CheckAdmin()) return RedirectToAction("Login", "Account", new { area = "" });

            if (ModelState.IsValid)
            {
                var existing = db.DiscountCodes.FirstOrDefault(d => d.Code == model.Code);
                if (existing != null)
                {
                    ModelState.AddModelError("Code", "Mã giảm giá này đã tồn tại trong hệ thống!");
                    return View(model);
                }

                model.IsActive = true;
                db.DiscountCodes.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // Đổi int id thành string id vì khóa chính Code là kiểu chuỗi
        public ActionResult Delete(string id)
        {
            if (!CheckAdmin()) return RedirectToAction("Login", "Account", new { area = "" });
            var item = db.DiscountCodes.Find(id);
            if (item != null)
            {
                db.DiscountCodes.Remove(item);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}