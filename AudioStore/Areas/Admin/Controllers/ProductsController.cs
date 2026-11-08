using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO; // Thư viện xử lý File & Directory
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AudioStore.Models;

namespace AudioStore.Areas.Admin.Controllers
{
    public class ProductsController : BaseAdminController
    {
        private DB_AudioStoreEntities db = new DB_AudioStoreEntities();

        // GET: Admin/Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Brands).Include(p => p.Categories);
            return View(products.ToList());
        }

        // GET: Admin/Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // GET: Admin/Products/Create
        public ActionResult Create()
        {
            ViewBag.BrandID = new SelectList(db.Brands, "BrandID", "BrandName");
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View();
        }

        // POST: Admin/Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,ProductName,Price,StockQuantity,CategoryID,BrandID,IsWireless,ImageUrl,VideoUrl,Specifications,Description,IsActive")] Products products, IEnumerable<HttpPostedFileBase> imageFiles)
        {
            if (ModelState.IsValid)
            {
                // --- XỬ LÝ UPLOAD NHIỀU ẢNH AN TOÀN VỚI SaveAs ---
                if (imageFiles != null && imageFiles.Any(f => f != null && f.ContentLength > 0))
                {
                    List<string> imagePaths = new List<string>();

                    string folderPath = Server.MapPath("~/Content/Images/products/");
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    foreach (var file in imageFiles)
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            // Lấy phần mở rộng thực tế của file (.jpg, .png, .webp...)
                            string extension = Path.GetExtension(file.FileName);
                            if (string.IsNullOrEmpty(extension)) extension = ".jpg";

                            // Tạo tên file duy nhất tránh trùng đè
                            string fileName = "product-" + Guid.NewGuid().ToString().Substring(0, 8) + extension;
                            string path = Path.Combine(folderPath, fileName);

                            // Ghi file trực tiếp xuống ổ cứng (không thông qua GDI+ gây lỗi Parameter is not valid)
                            file.SaveAs(path);

                            // Thêm đường dẫn tương đối vào danh sách
                            imagePaths.Add("/Content/Images/products/" + fileName);
                        }
                    }

                    // Nối danh sách các đường dẫn lại với nhau bằng dấu chấm phẩy (;)
                    products.ImageUrl = string.Join(";", imagePaths);
                }

                db.Products.Add(products);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BrandID = new SelectList(db.Brands, "BrandID", "BrandName", products.BrandID);
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", products.CategoryID);
            return View(products);
        }

        // GET: Admin/Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            ViewBag.BrandID = new SelectList(db.Brands, "BrandID", "BrandName", products.BrandID);
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", products.CategoryID);
            return View(products);
        }

        // POST: Admin/Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,ProductName,Price,StockQuantity,CategoryID,BrandID,IsWireless,ImageUrl,VideoUrl,Specifications,Description,IsActive")] Products products, IEnumerable<HttpPostedFileBase> imageFiles)
        {
            if (ModelState.IsValid)
            {
                // --- XỬ LÝ CẬP NHẬT NHIỀU ẢNH KHI EDIT ---
                if (imageFiles != null && imageFiles.Any(f => f != null && f.ContentLength > 0))
                {
                    List<string> imagePaths = new List<string>();

                    string folderPath = Server.MapPath("~/Content/Images/products/");
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    foreach (var file in imageFiles)
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            string extension = Path.GetExtension(file.FileName);
                            if (string.IsNullOrEmpty(extension)) extension = ".jpg";

                            string fileName = "product-" + Guid.NewGuid().ToString().Substring(0, 8) + extension;
                            string path = Path.Combine(folderPath, fileName);

                            file.SaveAs(path);

                            imagePaths.Add("/Content/Images/products/" + fileName);
                        }
                    }

                    products.ImageUrl = string.Join(";", imagePaths);
                }

                db.Entry(products).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BrandID = new SelectList(db.Brands, "BrandID", "BrandName", products.BrandID);
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", products.CategoryID);
            return View(products);
        }

        // GET: Admin/Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Products products = db.Products.Find(id);
            db.Products.Remove(products);
            db.SaveChanges();
            return RedirectToAction("Index");
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