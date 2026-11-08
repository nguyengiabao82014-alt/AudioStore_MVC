using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AudioStore.Models;

namespace AudioStore.Controllers
{
    public class HomeController : Controller
    {
        private DB_AudioStoreEntities db = new DB_AudioStoreEntities();

        // 1. TRANG CHỦ: Đổ danh sách sản phẩm, danh mục, thương hiệu
        public ActionResult Index()
        {
            var products = db.Products
                .Include(p => p.Brands)
                .Include(p => p.Categories)
                .Where(p => p.IsActive != false)
                .OrderByDescending(p => p.ProductID)
                .ToList();

            ViewBag.Categories = db.Categories.ToList();
            ViewBag.Brands = db.Brands.ToList();

            return View(products);
        }

        // 2. CHI TIẾT SẢN PHẨM: Hình ảnh, Thông số kỹ thuật, Video YouTube, Sản phẩm cùng loại
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var product = db.Products
                .Include(p => p.Brands)
                .Include(p => p.Categories)
                .FirstOrDefault(p => p.ProductID == id);

            if (product == null)
            {
                return HttpNotFound();
            }

            // Lấy tối đa 4 sản phẩm liên quan cùng danh mục
            ViewBag.RelatedProducts = db.Products
                .Include(p => p.Brands)
                .Include(p => p.Categories)
                .Where(p => p.CategoryID == product.CategoryID && p.ProductID != product.ProductID && p.IsActive != false)
                .Take(4)
                .ToList();

            return View(product);
        }

        // 3. TÌM KIẾM & BỘ LỌC ĐA NĂNG (Từ khóa, Danh mục, Hãng, Khoảng giá, Có dây/Không dây, Sắp xếp)
        public ActionResult Search(string keyword, int? categoryId, int? brandId, string parentType, decimal? minPrice, decimal? maxPrice, bool? isWireless, string sortOrder)
        {
            var query = db.Products
                .Include(p => p.Brands)
                .Include(p => p.Categories)
                .Where(p => p.IsActive != false);

            // Lọc theo từ khóa (tên sản phẩm, mô tả hoặc tên hãng)
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string kw = keyword.Trim();
                query = query.Where(p => p.ProductName.Contains(kw) ||
                                         p.Description.Contains(kw) ||
                                         p.Brands.BrandName.Contains(kw));
            }

            // Lọc theo Danh mục cụ thể
            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryID == categoryId.Value);
            }

            // Lọc theo Nhóm thiết bị (ParentType: Loa, Tai nghe, Micro...)
            if (!string.IsNullOrWhiteSpace(parentType))
            {
                query = query.Where(p => p.Categories.ParentType.Contains(parentType));
            }

            // Lọc theo Hãng (JBL, Marshall, Sony...)
            if (brandId.HasValue)
            {
                query = query.Where(p => p.BrandID == brandId.Value);
            }

            // Lọc theo Khoảng giá tiền
            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            // Lọc theo Có dây / Không dây
            if (isWireless.HasValue)
            {
                query = query.Where(p => p.IsWireless == isWireless.Value);
            }

            // Sắp xếp kết quả
            switch (sortOrder)
            {
                case "price_asc":
                    query = query.OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    query = query.OrderByDescending(p => p.Price);
                    break;
                case "name_asc":
                    query = query.OrderBy(p => p.ProductName);
                    break;
                default:
                    query = query.OrderByDescending(p => p.ProductID);
                    break;
            }

            ViewBag.Categories = db.Categories.ToList();
            ViewBag.Brands = db.Brands.ToList();
            ViewBag.Keyword = keyword;
            ViewBag.SelectedCategory = categoryId;
            ViewBag.SelectedBrand = brandId;
            ViewBag.SelectedParentType = parentType;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.IsWireless = isWireless;
            ViewBag.SortOrder = sortOrder;

            return View(query.ToList());
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
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