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

        // 1. TRANG CHỦ
        public ActionResult Index(int? categoryId)
        {
            ViewBag.Categories = db.Categories.ToList();
            ViewBag.Brands = db.Brands.ToList();

            // Lấy danh sách mã giảm giá còn hạn để hiển thị Trạm Săn Voucher trên trang chủ
            var activeDiscounts = db.DiscountCodes
                .Where(d => d.IsActive == true && (!d.ExpiryDate.HasValue || d.ExpiryDate.Value >= DateTime.Now) && (!d.UsageLimit.HasValue || d.UsageLimit.Value > 0))
                .Take(4)
                .ToList();
            ViewBag.DiscountCodes = activeDiscounts;

          

            var query = db.Products
                .Include(p => p.Brands)
                .Include(p => p.Categories)
                .Where(p => p.IsActive == true);

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryID == categoryId.Value);
                ViewBag.SelectedCategoryId = categoryId.Value;
            }

            var dsSanPham = query.OrderByDescending(p => p.ProductID).ToList();
            return View(dsSanPham);
        }

        // 2. TRANG TÌM KIẾM & BỘ LỌC NGANG ĐA NĂNG
        public ActionResult Search(
            string keyword,
            int? categoryId,
            int? brandId,
            bool? isWireless,
            decimal? minPrice,
            decimal? maxPrice,
            string sortBy,
            string sortOrder)
        {
            ViewBag.Categories = db.Categories.ToList();
            ViewBag.Brands = db.Brands.ToList();

            var query = db.Products
                .Include(p => p.Brands)
                .Include(p => p.Categories)
                .Where(p => p.IsActive == true);

            // Lọc theo từ khóa
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string kw = keyword.Trim();
                query = query.Where(p =>
                    p.ProductName.Contains(kw) ||
                    (p.Description != null && p.Description.Contains(kw)) ||
                    (p.Brands != null && p.Brands.BrandName.Contains(kw)) ||
                    (p.Categories != null && (p.Categories.CategoryName.Contains(kw) || p.Categories.ParentType.Contains(kw)))
                );
            }

            // Lọc theo danh mục
            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryID == categoryId.Value);
            }

            // Lọc theo thương hiệu
            if (brandId.HasValue)
            {
                query = query.Where(p => p.BrandID == brandId.Value);
            }

            // Lọc theo chuẩn kết nối (Không dây / Có dây)
            if (isWireless.HasValue)
            {
                query = query.Where(p => p.IsWireless == isWireless.Value);
            }

            // Lọc theo khoảng giá
            if (minPrice.HasValue && minPrice.Value > 0)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue && maxPrice.Value > 0)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            // Đồng bộ tham số sắp xếp (hỗ trợ cả sortBy và sortOrder)
            string finalSort = !string.IsNullOrEmpty(sortBy) ? sortBy : sortOrder;
            switch (finalSort)
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
                case "name_desc":
                    query = query.OrderByDescending(p => p.ProductName);
                    break;
                default:
                    query = query.OrderByDescending(p => p.ProductID);
                    break;
            }

            // Lưu trạng thái bộ lọc vào ViewBag để hiển thị lại trên thanh lọc ngang
            ViewBag.Keyword = keyword;
            ViewBag.CategoryId = categoryId;
            ViewBag.BrandId = brandId;
            ViewBag.IsWireless = isWireless;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.SortBy = finalSort;
            ViewBag.SortOrder = finalSort;

            var result = query.ToList();
            return View(result);
        }

        // 3. TRANG CHI TIẾT SẢN PHẨM
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var product = db.Products
                .Include(p => p.Brands)
                .Include(p => p.Categories)
                .FirstOrDefault(p => p.ProductID == id.Value);

            if (product == null)
            {
                return HttpNotFound();
            }

            var relatedProducts = db.Products
                .Include(p => p.Brands)
                .Include(p => p.Categories)
                .Where(p => p.IsActive == true && p.ProductID != product.ProductID && (p.CategoryID == product.CategoryID || p.BrandID == product.BrandID))
                .Take(4)
                .ToList();

            ViewBag.RelatedProducts = relatedProducts;

            return View(product);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Hệ thống phân phối thiết bị âm thanh chính hãng AudioStore.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Liên hệ với AudioStore.";
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