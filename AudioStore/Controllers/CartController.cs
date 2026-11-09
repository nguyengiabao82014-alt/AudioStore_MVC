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
        // Khởi tạo kết nối Database 
        private DB_AudioStoreEntities db = new DB_AudioStoreEntities();

        // ====================================================================
        // HÀM KIỂM TRA ĐĂNG NHẬP (BẢO VỆ GIỎ HÀNG)
        // ====================================================================
        private bool CheckLogin()
        {
            if (Session["User"] == null)
            {
                return false;
            }
            return true;
        }

        // ====================================================================
        // PHẦN 1: QUẢN LÝ GIỎ HÀNG (DÙNG SESSION)
        // ====================================================================

        // Hàm hỗ trợ: Lấy giỏ hàng từ Session
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

        // Trang hiển thị Giỏ hàng
        public ActionResult Index()
        {
            if (!CheckLogin())
            {
                return View(new List<CartItem>());
            }

            var cart = GetCart();
            return View(cart);
        }

        // Thêm sản phẩm vào giỏ hàng nhưng KHÔNG chuyển trang, giữ nguyên ở trang hiện tại
        public ActionResult AddToCart(int id)
        {
            if (!CheckLogin()) return RedirectToAction("Login", "Account");

            var product = db.Products.FirstOrDefault(p => p.ProductID == id && p.IsActive == true);
            if (product != null)
            {
                if (product.StockQuantity.HasValue && product.StockQuantity.Value <= 0)
                {
                    return Redirect(Request.UrlReferrer?.ToString() ?? Url.Action("Index", "Home"));
                }

                var cart = GetCart();
                var existingItem = cart.FirstOrDefault(c => c.ProductID == id);

                if (existingItem != null)
                {
                    if (!product.StockQuantity.HasValue || existingItem.Quantity < product.StockQuantity.Value)
                    {
                        existingItem.Quantity++;
                    }
                }
                else
                {
                    string firstImage = "/Content/Images/no-image.png";
                    if (!string.IsNullOrEmpty(product.ImageUrl))
                    {
                        var images = product.ImageUrl.Split(new[] { ';', '|' }, StringSplitOptions.RemoveEmptyEntries);
                        if (images.Length > 0)
                        {
                            string rawImg = images[0].Trim();
                            if (!rawImg.Contains("."))
                            {
                                rawImg += ".webp";
                            }

                            if (rawImg.StartsWith("/") || rawImg.StartsWith("~") || rawImg.StartsWith("http"))
                            {
                                firstImage = rawImg;
                            }
                            else
                            {
                                firstImage = "/Content/Images/products/" + rawImg;
                            }
                        }
                    }

                    cart.Add(new CartItem
                    {
                        ProductID = product.ProductID,
                        ProductName = product.ProductName,
                        Price = product.Price,
                        Quantity = 1,
                        ImageUrl = firstImage
                    });
                }
            }

            // Quay lại đúng trang người dùng đang đứng
            if (Request.UrlReferrer != null)
            {
                return Redirect(Request.UrlReferrer.ToString());
            }
            return RedirectToAction("Index", "Home");
        }

        // Mua ngay (Thanh toán độc lập): Tạo đơn hàng riêng biệt, KHÔNG cộng dồn vào giỏ hàng chính
        public ActionResult BuyNow(int id)
        {
            if (!CheckLogin()) return RedirectToAction("Login", "Account");

            var product = db.Products.FirstOrDefault(p => p.ProductID == id && p.IsActive == true);
            if (product != null)
            {
                if (product.StockQuantity.HasValue && product.StockQuantity.Value <= 0)
                {
                    return RedirectToAction("Details", "Home", new { id = id });
                }

                string firstImage = "/Content/Images/no-image.png";
                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    var images = product.ImageUrl.Split(new[] { ';', '|' }, StringSplitOptions.RemoveEmptyEntries);
                    if (images.Length > 0)
                    {
                        string rawImg = images[0].Trim();
                        if (!rawImg.Contains(".")) rawImg += ".webp";
                        firstImage = (rawImg.StartsWith("/") || rawImg.StartsWith("~") || rawImg.StartsWith("http"))
                                     ? rawImg
                                     : "/Content/Images/products/" + rawImg;
                    }
                }

                // Tạo danh sách tạm thời CHỈ chứa đúng 1 sản phẩm mua ngay này
                var buyNowList = new List<CartItem>
                {
                    new CartItem
                    {
                        ProductID = product.ProductID,
                        ProductName = product.ProductName,
                        Price = product.Price,
                        Quantity = 1,
                        ImageUrl = firstImage
                    }
                };

                // Lưu vào session riêng biệt, tách rời hoàn toàn với Session["Cart"]
                Session["CheckoutNow"] = buyNowList;
            }

            // Chuyển hướng sang trang thanh toán độc lập
            return RedirectToAction("CheckoutNow");
        }

        // Cập nhật số lượng sản phẩm
        [HttpPost]
        public ActionResult UpdateQuantity(int id, int quantity)
        {
            if (!CheckLogin()) return RedirectToAction("Login", "Account");

            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.ProductID == id);
            if (item != null)
            {
                if (quantity > 0)
                {
                    var product = db.Products.Find(id);
                    if (product != null && product.StockQuantity.HasValue && quantity > product.StockQuantity.Value)
                    {
                        item.Quantity = product.StockQuantity.Value;
                    }
                    else
                    {
                        item.Quantity = quantity;
                    }
                }
                else
                {
                    cart.Remove(item);
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
        // PHẦN 2: LUỒNG THANH TOÁN & BẮT LỖI NGHIÊM NGẶT
        // ====================================================================

        // GET: Hiển thị form thanh toán (Dành cho giỏ hàng chính)
        [HttpGet]
        public ActionResult Checkout()
        {
            if (!CheckLogin()) return RedirectToAction("Login", "Account");

            // Nếu người dùng đang có session mua ngay, hãy xóa nó để tránh nhầm lẫn khi quay lại giỏ hàng
            Session["CheckoutNow"] = null;

            var cart = GetCart();
            if (cart.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new CheckoutViewModel());
        }

        // GET: Hiển thị form thanh toán độc lập (Dành cho Mua ngay)
        [HttpGet]
        public ActionResult CheckoutNow()
        {
            if (!CheckLogin()) return RedirectToAction("Login", "Account");

            var cart = Session["CheckoutNow"] as List<CartItem>;
            if (cart == null || cart.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            // Dùng chung view Checkout
            return View("Checkout", new CheckoutViewModel());
        }

        // POST: Xử lý khi nhấn nút Đặt hàng từ Giỏ hàng chính
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Checkout(CheckoutViewModel model)
        {
            if (!CheckLogin()) return RedirectToAction("Login", "Account");

            var cart = GetCart();
            if (cart.Count == 0) return RedirectToAction("Index", "Home");

            return ProcessOrder(model, cart, isBuyNow: false);
        }

        // POST: Xử lý khi nhấn nút Đặt hàng từ Mua ngay độc lập
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckoutNow(CheckoutViewModel model)
        {
            if (!CheckLogin()) return RedirectToAction("Login", "Account");

            var cart = Session["CheckoutNow"] as List<CartItem>;
            if (cart == null || cart.Count == 0) return RedirectToAction("Index", "Home");

            return ProcessOrder(model, cart, isBuyNow: true);
        }

        // Hàm chung xử lý lưu đơn hàng vào DB và trừ kho
        private ActionResult ProcessOrder(CheckoutViewModel model, List<CartItem> cart, bool isBuyNow)
        {
            if (!ModelState.IsValid)
            {
                return View("Checkout", model);
            }

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    int currentUserId = 0;
                    if (Session["User"] is AudioStore.Models.Users loggedInUser)
                    {
                        currentUserId = loggedInUser.UserID;
                    }
                    else if (Session["UserID"] != null)
                    {
                        currentUserId = Convert.ToInt32(Session["UserID"]);
                    }

                    decimal totalAmount = cart.Sum(item => item.TotalPrice);
                    decimal finalAmount = totalAmount;
                    string validDiscountCode = null;

                    if (!string.IsNullOrWhiteSpace(model.DiscountCode))
                    {
                        string cleanCode = model.DiscountCode.Trim().ToUpper();
                        var discount = db.DiscountCodes.FirstOrDefault(d => d.Code == cleanCode && d.IsActive == true);

                        if (discount != null &&
                            (!discount.ExpiryDate.HasValue || discount.ExpiryDate.Value >= DateTime.Now) &&
                            (!discount.UsageLimit.HasValue || discount.UsageLimit.Value > 0) &&
                            (!discount.MinOrderValue.HasValue || totalAmount >= discount.MinOrderValue.Value))
                        {
                            decimal discountAmount = (totalAmount * discount.DiscountPercent) / 100m;
                            if (discount.MaxDiscountAmount.HasValue && discount.MaxDiscountAmount.Value > 0 && discountAmount > discount.MaxDiscountAmount.Value)
                            {
                                discountAmount = discount.MaxDiscountAmount.Value;
                            }

                            finalAmount = totalAmount - discountAmount;
                            if (finalAmount < 0) finalAmount = 0;
                            validDiscountCode = discount.Code;

                            if (discount.UsageLimit.HasValue)
                            {
                                discount.UsageLimit -= 1;
                            }
                        }
                    }

                    Orders newOrder = new Orders
                    {
                        UserID = currentUserId,
                        OrderDate = DateTime.Now,
                        TotalAmount = totalAmount,
                        FinalAmount = finalAmount,
                        DiscountCode = validDiscountCode,
                        ShippingAddress = model.ShippingAddress,
                        PaymentMethod = model.PaymentMethod,
                        Status = "Pending"
                    };

                    db.Orders.Add(newOrder);
                    db.SaveChanges();

                    foreach (var item in cart)
                    {
                        var productInDb = db.Products.Find(item.ProductID);
                        if (productInDb != null)
                        {
                            if (productInDb.StockQuantity.HasValue && productInDb.StockQuantity.Value < item.Quantity)
                            {
                                transaction.Rollback();
                                ModelState.AddModelError("", $"Sản phẩm '{item.ProductName}' hiện chỉ còn {productInDb.StockQuantity.Value} chiếc trong kho!");
                                return View("Checkout", model);
                            }

                            if (productInDb.StockQuantity.HasValue)
                            {
                                productInDb.StockQuantity -= item.Quantity;
                            }
                        }

                        OrderDetails detail = new OrderDetails
                        {
                            OrderID = newOrder.OrderID,
                            ProductID = item.ProductID,
                            Quantity = item.Quantity,
                            UnitPrice = item.Price
                        };
                        db.OrderDetails.Add(detail);
                    }
                    db.SaveChanges();
                    transaction.Commit();

                    // Xóa giỏ hàng tương ứng sau khi thanh toán thành công
                    if (isBuyNow)
                    {
                        Session["CheckoutNow"] = null; // Chỉ xóa sản phẩm mua ngay độc lập
                    }
                    else
                    {
                        Session["Cart"] = null; // Xóa giỏ hàng chính
                    }

                    return RedirectToAction("Success");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ModelState.AddModelError("", "Đã xảy ra sự cố khi đặt hàng. Vui lòng thử lại. Lỗi: " + ex.Message);
                    return View("Checkout", model);
                }
            }
        }

        // ====================================================================
        // XỬ LÝ MÃ GIẢM GIÁ (AJAX)
        // ====================================================================
        [HttpPost]
        public JsonResult ApplyDiscount(string code)
        {
            // Kiểm tra xem đang ở giỏ hàng hay mua ngay để tính tổng tiền chính xác
            var cart = Session["CheckoutNow"] as List<CartItem> ?? GetCart();
            if (cart.Count == 0)
            {
                return Json(new { success = false, message = "Giỏ hàng trống!" });
            }

            if (string.IsNullOrWhiteSpace(code))
            {
                return Json(new { success = false, message = "Vui lòng nhập mã giảm giá!" });
            }

            string cleanCode = code.Trim().ToUpper();
            decimal totalAmount = cart.Sum(item => item.TotalPrice);

            var discount = db.DiscountCodes.FirstOrDefault(d => d.Code == cleanCode && d.IsActive == true);

            if (discount == null)
            {
                return Json(new { success = false, message = "Mã giảm giá không tồn tại hoặc đã bị khóa!" });
            }

            if (discount.ExpiryDate.HasValue && discount.ExpiryDate.Value < DateTime.Now)
            {
                return Json(new { success = false, message = "Mã giảm giá đã hết hạn!" });
            }

            if (discount.MinOrderValue.HasValue && totalAmount < discount.MinOrderValue.Value)
            {
                return Json(new { success = false, message = $"Đơn hàng chưa đạt mức tối thiểu {discount.MinOrderValue.Value:N0}đ để dùng mã này!" });
            }

            if (discount.UsageLimit.HasValue && discount.UsageLimit.Value <= 0)
            {
                return Json(new { success = false, message = "Mã giảm giá này đã hết lượt sử dụng!" });
            }

            decimal discountAmount = (totalAmount * discount.DiscountPercent) / 100m;

            if (discount.MaxDiscountAmount.HasValue && discount.MaxDiscountAmount.Value > 0 && discountAmount > discount.MaxDiscountAmount.Value)
            {
                discountAmount = discount.MaxDiscountAmount.Value;
            }

            decimal finalAmount = totalAmount - discountAmount;
            if (finalAmount < 0)
            {
                finalAmount = 0;
            }

            return Json(new
            {
                success = true,
                message = $"Áp dụng mã {cleanCode} thành công! Bạn được giảm {discountAmount:N0} đ.",
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