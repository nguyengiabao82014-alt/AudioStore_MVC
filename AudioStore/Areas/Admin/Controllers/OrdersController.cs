using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AudioStore.Models;

namespace AudioStore.Areas.Admin.Controllers
{
    public class OrdersController : BaseAdminController
    {
        private DB_AudioStoreEntities db = new DB_AudioStoreEntities();

        // GET: Admin/Orders (Hiển thị tất cả đơn hàng đang có)
        public ActionResult Index(string status = "")
        {
            var query = db.Orders
                .Include(o => o.Users)
                .Include(o => o.OrderDetails);

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(o => o.Status == status);
            }

            var orders = query.OrderByDescending(o => o.OrderDate).ToList();
            ViewBag.CurrentStatus = status;
            return View(orders);
        }

        // GET: Admin/Orders/Details/5 (Xem chi tiết đơn hàng của người dùng)
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var order = db.Orders
                .Include(o => o.Users)
                .Include(o => o.OrderDetails.Select(od => od.Products))
                .FirstOrDefault(o => o.OrderID == id.Value);

            if (order == null)
            {
                return HttpNotFound();
            }

            return View(order);
        }

        // GET: Admin/Orders/Edit/5 (Chỉnh sửa đơn hàng của mỗi người dùng)
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var order = db.Orders
                .Include(o => o.Users)
                .Include(o => o.OrderDetails.Select(od => od.Products))
                .FirstOrDefault(o => o.OrderID == id.Value);

            if (order == null)
            {
                return HttpNotFound();
            }

            return View(order);
        }

        // POST: Admin/Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int OrderID, string Status, string ShippingAddress, string PaymentMethod)
        {
            var orderInDb = db.Orders
                .Include(o => o.Users)
                .Include(o => o.OrderDetails.Select(od => od.Products))
                .FirstOrDefault(o => o.OrderID == OrderID);

            if (orderInDb == null)
            {
                return HttpNotFound();
            }

            if (string.IsNullOrWhiteSpace(ShippingAddress) || ShippingAddress.Trim().Length < 5)
            {
                ModelState.AddModelError("ShippingAddress", "Địa chỉ nhận hàng phải có ít nhất 5 ký tự.");
                return View(orderInDb);
            }

            string oldStatus = orderInDb.Status ?? "Pending";
            string newStatus = string.IsNullOrWhiteSpace(Status) ? "Pending" : Status.Trim();

            bool wasCancelled = (oldStatus == "Cancelled" || oldStatus == "Đã hủy");
            bool isNowCancelled = (newStatus == "Cancelled" || newStatus == "Đã hủy");

            // Tự động hoàn lại tồn kho nếu Admin chuyển đơn sang "Cancelled" (Đã hủy)
            if (!wasCancelled && isNowCancelled)
            {
                foreach (var detail in orderInDb.OrderDetails)
                {
                    var product = db.Products.Find(detail.ProductID);
                    if (product != null)
                    {
                        product.StockQuantity = (product.StockQuantity ?? 0) + detail.Quantity;
                    }
                }
            }
            // Nếu đơn đang "Cancelled" mà Admin khôi phục lại trạng thái hoạt động thì trừ lại kho
            else if (wasCancelled && !isNowCancelled)
            {
                foreach (var detail in orderInDb.OrderDetails)
                {
                    var product = db.Products.Find(detail.ProductID);
                    int stock = product != null ? (product.StockQuantity ?? 0) : 0;
                    if (stock < detail.Quantity)
                    {
                        ModelState.AddModelError("", $"Không thể khôi phục đơn hàng vì sản phẩm '{product?.ProductName}' chỉ còn {stock} chiếc trong kho.");
                        return View(orderInDb);
                    }
                    if (product != null)
                    {
                        product.StockQuantity = stock - detail.Quantity;
                    }
                }
            }

            orderInDb.Status = newStatus;
            orderInDb.ShippingAddress = ShippingAddress.Trim();
            if (!string.IsNullOrWhiteSpace(PaymentMethod))
            {
                orderInDb.PaymentMethod = PaymentMethod.Trim();
            }

            db.SaveChanges();
            TempData["SuccessMessage"] = $"Đã cập nhật đơn hàng #{OrderID} thành công!";
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