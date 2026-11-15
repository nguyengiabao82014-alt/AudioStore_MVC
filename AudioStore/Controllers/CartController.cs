using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AudioStore.Models;

namespace AudioStore.Controllers
{
    public class CartController : Controller
    {
        private DB_AudioStoreEntities db = new DB_AudioStoreEntities();

        // ====================================================================
        // HÀM KIỂM TRA ĐĂNG NHẬP & KIỂM TRA QUYỀN ADMIN
        // ====================================================================
        private bool CheckLogin()
        {
            return Session["User"] != null;
        }

        private bool IsAdmin()
        {
            if (Session["RoleID"] != null && Convert.ToInt32(Session["RoleID"]) == 1)
                return true;
            if (Session["User"] is Users u && u.RoleID == 1)
                return true;
            return false;
        }

        private ActionResult AlertAndRedirectBack(string message, string fallbackUrl = null)
        {
            string targetUrl = fallbackUrl;
            if (string.IsNullOrEmpty(targetUrl))
            {
                targetUrl = Request.UrlReferrer != null
                    ? Request.UrlReferrer.ToString()
                    : Url.Action("Index", "Home");
            }

            string safeMsg = HttpUtility.JavaScriptStringEncode(message);
            string safeUrl = HttpUtility.JavaScriptStringEncode(targetUrl);

            return Content(
                $"<!DOCTYPE html><html><head><meta charset='utf-8'/></head><body><script>alert('{safeMsg}'); window.location.href='{safeUrl}';</script></body></html>",
                "text/html"
            );
        }

        private string GetFirstImageUrl(string rawImageUrl)
        {
            if (string.IsNullOrWhiteSpace(rawImageUrl))
                return "/Content/Images/no-image.png";

            var parts = rawImageUrl.Split(new[] { ';', '|' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0)
                return "/Content/Images/no-image.png";

            string firstImg = parts[0].Trim();
            if (firstImg.StartsWith("http://") || firstImg.StartsWith("https://"))
                return firstImg;

            if (!firstImg.Contains("."))
                firstImg += ".webp";

            if (firstImg.StartsWith("~/"))
                return firstImg.Substring(1);
            if (!firstImg.StartsWith("/"))
                firstImg = "/Content/Images/products/" + firstImg;

            return firstImg;
        }

        // ====================================================================
        // PHẦN 1: QUẢN LÝ GIỎ HÀNG (SESSION["Cart"] & SESSION["CheckoutNow"])
        // ====================================================================
        private List<CartItem> GetCart()
        {
            var cart = Session["Cart"] as List<CartItem>;
            if (cart == null)
            {
                cart = new List<CartItem>();
                Session["Cart"] = cart;
            }
            return cart;
        }

        // Trang hiển thị Giỏ hàng chung
        public ActionResult Index()
        {
            if (!CheckLogin())
            {
                return View(new List<CartItem>());
            }

            // Khi khách vào xem Giỏ hàng chung, xóa phiên Mua ngay tạm thời (nếu có)
            // để khi bấm "Tiến hành thanh toán" sẽ thanh toán đúng Giỏ hàng chung
            Session["CheckoutNow"] = null;

            var cart = GetCart();

            // Đồng bộ lại tồn kho thực tế cho các sản phẩm trong giỏ
            var stockDict = new Dictionary<int, int>();
            foreach (var item in cart.ToList())
            {
                var prod = db.Products.Find(item.ProductID);
                int availableStock = (prod != null && prod.IsActive == true) ? (prod.StockQuantity ?? 0) : 0;
                stockDict[item.ProductID] = availableStock;

                if (availableStock <= 0)
                {
                    cart.Remove(item);
                    TempData["CartError"] = $"Sản phẩm '{item.ProductName}' đã hết hàng nên được gỡ khỏi giỏ.";
                }
                else if (item.Quantity > availableStock)
                {
                    item.Quantity = availableStock;
                    TempData["CartError"] = $"Sản phẩm '{item.ProductName}' chỉ còn {availableStock} chiếc trong kho. Đã tự động điều chỉnh số lượng.";
                }
            }
            ViewBag.StockDict = stockDict;

            return View(cart);
        }

        // Thêm sản phẩm vào giỏ hàng (Giữ khách ở lại trang hiện tại bằng UrlReferrer)
        public ActionResult AddToCart(int id, int quantity = 1)
        {
            if (!CheckLogin()) return RedirectToAction("Login", "Account");

            // CHẶN ADMIN ĐẶT HÀNG
            if (IsAdmin())
            {
                return AlertAndRedirectBack("Tài khoản Quản trị viên (Admin) không được phép mua hoặc đặt hàng!");
            }

            if (quantity < 1) quantity = 1;

            var product = db.Products.FirstOrDefault(p => p.ProductID == id && p.IsActive == true);
            if (product == null)
            {
                return AlertAndRedirectBack("Sản phẩm không tồn tại hoặc đã ngừng kinh doanh!");
            }

            int stock = product.StockQuantity ?? 0;
            if (stock <= 0)
            {
                return AlertAndRedirectBack($"Sản phẩm '{product.ProductName}' hiện đã hết hàng!");
            }

            var cart = GetCart();
            var existingItem = cart.FirstOrDefault(c => c.ProductID == id);

            int currentInCart = existingItem != null ? existingItem.Quantity : 0;
            if (currentInCart + quantity > stock)
            {
                if (existingItem != null)
                {
                    existingItem.Quantity = stock;
                }
                else
                {
                    cart.Add(new CartItem
                    {
                        ProductID = product.ProductID,
                        ProductName = product.ProductName,
                        Price = product.Price,
                        Quantity = stock,
                        ImageUrl = GetFirstImageUrl(product.ImageUrl)
                    });
                }

                TempData["CartError"] = $"Kho chỉ còn tối đa {stock} sản phẩm '{product.ProductName}'. Không thể thêm vượt quá số lượng trong kho!";
                return AlertAndRedirectBack($"Không thể thêm quá số lượng tồn kho! Sản phẩm '{product.ProductName}' hiện chỉ còn tối đa {stock} chiếc trong kho (trong giỏ của bạn đã đạt mức tối đa {stock} chiếc).");
            }

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductID = product.ProductID,
                    ProductName = product.ProductName,
                    Price = product.Price,
                    Quantity = quantity,
                    ImageUrl = GetFirstImageUrl(product.ImageUrl)
                });
            }

            if (Request.UrlReferrer != null)
            {
                return Redirect(Request.UrlReferrer.ToString());
            }
            return RedirectToAction("Index", "Home");
        }

        // Luồng Mua ngay độc lập (Không cộng dồn vào Session["Cart"])
        public ActionResult BuyNow(int id, int quantity = 1)
        {
            if (!CheckLogin()) return RedirectToAction("Login", "Account");

            // CHẶN ADMIN ĐẶT HÀNG
            if (IsAdmin())
            {
                return AlertAndRedirectBack("Tài khoản Quản trị viên (Admin) không được phép mua hoặc đặt hàng!");
            }

            if (quantity < 1) quantity = 1;

            var product = db.Products.FirstOrDefault(p => p.ProductID == id && p.IsActive == true);
            if (product == null)
            {
                return AlertAndRedirectBack("Sản phẩm không tồn tại hoặc đã ngừng kinh doanh!");
            }

            int stock = product.StockQuantity ?? 0;
            if (stock <= 0)
            {
                return AlertAndRedirectBack($"Sản phẩm '{product.ProductName}' hiện đã hết hàng!");
            }

            if (quantity > stock)
            {
                return AlertAndRedirectBack($"Số lượng mua ({quantity}) vượt quá số lượng hiện có trong kho ({stock} chiếc)!");
            }

            var checkoutNowList = new List<CartItem>
            {
                new CartItem
                {
                    ProductID = product.ProductID,
                    ProductName = product.ProductName,
                    Price = product.Price,
                    Quantity = quantity,
                    ImageUrl = GetFirstImageUrl(product.ImageUrl)
                }
            };

            Session["CheckoutNow"] = checkoutNowList;
            return RedirectToAction("Checkout");
        }

        // Cập nhật số lượng sản phẩm trong giỏ
        [HttpPost]
        public ActionResult UpdateQuantity(int id, int quantity)
        {
            if (!CheckLogin()) return RedirectToAction("Login", "Account");
            if (IsAdmin()) return AlertAndRedirectBack("Tài khoản Admin không được phép đặt hàng!", Url.Action("Index", "Home"));

            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.ProductID == id);
            if (item != null)
            {
                if (quantity <= 0)
                {
                    cart.Remove(item);
                }
                else
                {
                    var product = db.Products.Find(id);
                    int stock = (product != null && product.IsActive == true) ? (product.StockQuantity ?? 0) : 0;

                    if (stock <= 0)
                    {
                        cart.Remove(item);
                        TempData["CartError"] = $"Sản phẩm '{item.ProductName}' vừa hết hàng và đã được xóa khỏi giỏ.";
                    }
                    else if (quantity > stock)
                    {
                        item.Quantity = stock;
                        TempData["CartError"] = $"Không thể vượt quá số lượng trong kho! Sản phẩm '{item.ProductName}' chỉ còn tối đa {stock} chiếc.";
                    }
                    else
                    {
                        item.Quantity = quantity;
                    }
                }
            }
            return RedirectToAction("Index");
        }

        // Xóa sản phẩm khỏi giỏ
        public ActionResult RemoveFromCart(int id)
        {
            if (!CheckLogin()) return RedirectToAction("Login", "Account");

            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.ProductID == id);
            if (item != null)
            {
                cart.Remove(item);
            }
            return RedirectToAction("Index");
        }

        // ====================================================================
        // PHẦN 2: LUỒNG THANH TOÁN (HỖ TRỢ CẢ GIỎ HÀNG CHUNG & MUA NGAY)
        // ====================================================================
        private List<CartItem> GetCurrentCheckoutItems()
        {
            var checkoutNow = Session["CheckoutNow"] as List<CartItem>;
            if (checkoutNow != null && checkoutNow.Count > 0)
            {
                return checkoutNow;
            }
            return GetCart();
        }

        // GET: Hiển thị form thanh toán
        [HttpGet]
        public ActionResult Checkout()
        {
            if (!CheckLogin()) return RedirectToAction("Login", "Account");

            // CHẶN ADMIN TRUY CẬP TRANG THANH TOÁN
            if (IsAdmin())
            {
                return AlertAndRedirectBack("Tài khoản Quản trị viên (Admin) không được phép đặt hàng!", Url.Action("Index", "Home"));
            }

            var itemsToCheckout = GetCurrentCheckoutItems();
            if (itemsToCheckout == null || itemsToCheckout.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            // Tự động điền sẵn Họ tên, Email, SĐT của người dùng đang đăng nhập để thuận tiện
            var model = new CheckoutViewModel
            {
                PaymentMethod = "COD"
            };

            if (Session["User"] is Users u)
            {
                model.FullName = u.FullName;
                model.Email = u.Email;
                model.Phone = u.Phone;
            }

            return View(model);
        }

        // POST: Xử lý khi nhấn nút Đặt hàng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Checkout(CheckoutViewModel model)
        {
            if (!CheckLogin()) return RedirectToAction("Login", "Account");

            // CHẶN ADMIN ĐẶT HÀNG
            if (IsAdmin())
            {
                return AlertAndRedirectBack("Tài khoản Quản trị viên (Admin) không được phép đặt hàng!", Url.Action("Index", "Home"));
            }

            bool isBuyNowFlow = (Session["CheckoutNow"] as List<CartItem>) != null && ((List<CartItem>)Session["CheckoutNow"]).Count > 0;
            var itemsToCheckout = GetCurrentCheckoutItems();

            if (itemsToCheckout == null || itemsToCheckout.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            // Chuẩn hóa khoảng trắng đầu/cuối trước khi kiểm tra
            if (model != null)
            {
                model.FullName = model.FullName?.Trim();
                model.Phone = model.Phone?.Trim();
                model.Email = model.Email?.Trim();
                model.ShippingAddress = model.ShippingAddress?.Trim();
                model.DiscountCode = model.DiscountCode?.Trim();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    int currentUserId = 0;
                    if (Session["User"] is Users loggedInUser)
                    {
                        currentUserId = loggedInUser.UserID;
                    }
                    else if (Session["UserID"] != null)
                    {
                        currentUserId = Convert.ToInt32(Session["UserID"]);
                    }

                    // Kiểm tra tồn kho thực tế trước khi tạo đơn
                    foreach (var item in itemsToCheckout)
                    {
                        var productInDb = db.Products.FirstOrDefault(p => p.ProductID == item.ProductID && p.IsActive == true);
                        int availableStock = productInDb != null ? (productInDb.StockQuantity ?? 0) : 0;

                        if (productInDb == null || availableStock < item.Quantity)
                        {
                            transaction.Rollback();
                            ModelState.AddModelError("", $"Sản phẩm '{item.ProductName}' hiện chỉ còn {availableStock} chiếc trong kho, không đủ số lượng ({item.Quantity}) bạn đặt.");
                            return View(model);
                        }
                    }

                    decimal totalAmount = itemsToCheckout.Sum(item => item.TotalPrice);
                    decimal finalAmount = totalAmount;
                    string appliedCode = null;

                    // Xử lý mã giảm giá (nếu có)
                    if (!string.IsNullOrEmpty(model.DiscountCode))
                    {
                        var discount = db.DiscountCodes.FirstOrDefault(d => d.Code == model.DiscountCode && d.IsActive == true);
                        if (discount != null &&
                            (!discount.ExpiryDate.HasValue || discount.ExpiryDate.Value >= DateTime.Now) &&
                            (!discount.MinOrderValue.HasValue || totalAmount >= discount.MinOrderValue.Value) &&
                            (!discount.UsageLimit.HasValue || discount.UsageLimit.Value > 0))
                        {
                            decimal discountAmount = (totalAmount * discount.DiscountPercent) / 100m;
                            if (discount.MaxDiscountAmount.HasValue && discountAmount > discount.MaxDiscountAmount.Value)
                            {
                                discountAmount = discount.MaxDiscountAmount.Value;
                            }

                            finalAmount = totalAmount - discountAmount;
                            if (finalAmount < 0) finalAmount = 0;

                            if (discount.UsageLimit.HasValue)
                            {
                                discount.UsageLimit -= 1;
                            }
                            appliedCode = discount.Code;
                        }
                    }

                    // A. Lưu đơn hàng vào bảng Orders
                    Orders newOrder = new Orders
                    {
                        UserID = currentUserId,
                        OrderDate = DateTime.Now,
                        TotalAmount = totalAmount,
                        FinalAmount = finalAmount,
                        DiscountCode = appliedCode,
                        ShippingAddress = model.ShippingAddress,
                        PaymentMethod = model.PaymentMethod,
                        Status = "Pending"
                    };

                    db.Orders.Add(newOrder);
                    db.SaveChanges();

                    // B. Lưu chi tiết đơn hàng (OrderDetails) & Trừ số lượng tồn kho (Products)
                    foreach (var item in itemsToCheckout)
                    {
                        OrderDetails detail = new OrderDetails
                        {
                            OrderID = newOrder.OrderID,
                            ProductID = item.ProductID,
                            Quantity = item.Quantity,
                            UnitPrice = item.Price
                        };
                        db.OrderDetails.Add(detail);

                        var productInDb = db.Products.Find(item.ProductID);
                        if (productInDb != null)
                        {
                            productInDb.StockQuantity = (productInDb.StockQuantity ?? 0) - item.Quantity;
                        }
                    }

                    db.SaveChanges();
                    transaction.Commit();

                    // Dọn sạch Session thanh toán tương ứng
                    if (isBuyNowFlow)
                    {
                        Session["CheckoutNow"] = null;
                    }
                    else
                    {
                        Session["Cart"] = null;
                    }

                    return RedirectToAction("Success");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ModelState.AddModelError("", "Đã xảy ra sự cố khi đặt hàng. Vui lòng thử lại. Chi tiết: " + ex.Message);
                    return View(model);
                }
            }
        }

        // ====================================================================
        // XỬ LÝ MÃ GIẢM GIÁ (AJAX)
        // ====================================================================
        [HttpPost]
        public JsonResult ApplyDiscount(string code)
        {
            var itemsToCheckout = GetCurrentCheckoutItems();
            if (itemsToCheckout == null || itemsToCheckout.Count == 0)
                return Json(new { success = false, message = "Giỏ hàng trống!" });

            decimal totalAmount = itemsToCheckout.Sum(item => item.TotalPrice);

            var discount = db.DiscountCodes.FirstOrDefault(d => d.Code == code && d.IsActive == true);

            if (discount == null)
                return Json(new { success = false, message = "Mã giảm giá không tồn tại hoặc đã bị khóa!" });

            if (discount.ExpiryDate.HasValue && discount.ExpiryDate.Value < DateTime.Now)
                return Json(new { success = false, message = "Mã giảm giá đã hết hạn!" });

            if (discount.MinOrderValue.HasValue && totalAmount < discount.MinOrderValue.Value)
                return Json(new { success = false, message = $"Đơn hàng chưa đạt mức tối thiểu {discount.MinOrderValue.Value:N0}đ để dùng mã này!" });

            if (discount.UsageLimit.HasValue && discount.UsageLimit.Value <= 0)
                return Json(new { success = false, message = "Mã giảm giá này đã hết lượt sử dụng!" });

            decimal discountAmount = (totalAmount * discount.DiscountPercent) / 100m;

            if (discount.MaxDiscountAmount.HasValue && discountAmount > discount.MaxDiscountAmount.Value)
            {
                discountAmount = discount.MaxDiscountAmount.Value;
            }

            decimal finalAmount = totalAmount - discountAmount;
            if (finalAmount < 0) finalAmount = 0;

            return Json(new
            {
                success = true,
                message = "Áp dụng mã giảm giá thành công!",
                discountAmount = discountAmount,
                finalAmount = finalAmount
            });
        }

        // Trang báo đặt hàng thành công
        public ActionResult Success()
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