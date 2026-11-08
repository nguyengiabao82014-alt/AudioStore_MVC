using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AudioStore.Models
{
    // =========================================================================================
    // 1. CHECKOUT VIEW MODEL - BỨC TƯỜNG LỬA CHO TRANG THANH TOÁN
    // =========================================================================================
    public class CheckoutViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập họ và tên người nhận.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Tên phải từ 5 đến 100 ký tự.")]
        [RegularExpression(@"^[\p{L}]+\s+[\p{L}\s]+$", ErrorMessage = "Vui lòng nhập đầy đủ Họ và Tên (phải có khoảng trắng, không chứa số hay ký tự đặc biệt).")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Số điện thoại là bắt buộc.")]
        [RegularExpression(@"^(0[3|5|7|8|9])+([0-9]{8})$", ErrorMessage = "Số điện thoại không hợp lệ (VD: 0912345678).")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ Email.")]
        [EmailAddress(ErrorMessage = "Định dạng email không hợp lệ (VD: ten@gmail.com).")]
        [StringLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ giao hàng chi tiết.")]
        [StringLength(500, MinimumLength = 15, ErrorMessage = "Địa chỉ quá ngắn, vui lòng ghi rõ số nhà, tên đường, phường/xã.")]
        [RegularExpression(@"^[\p{L}0-9,.-/]+(?:\s+[\p{L}0-9,.-/]+){2,}$", ErrorMessage = "Địa chỉ phải bao gồm ít nhất 3 từ (VD: 123 Lê Lợi, Quận 1).")]
        public string ShippingAddress { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn phương thức thanh toán.")]
        public string PaymentMethod { get; set; }

        [StringLength(50, ErrorMessage = "Mã giảm giá không hợp lệ.")]
        public string DiscountCode { get; set; }
    }

    // =========================================================================================
    // 2. RÀNG BUỘC CHO CLASS "Orders" GỐC (Sinh ra từ bảng DB)
    // =========================================================================================
    [MetadataType(typeof(OrderMetadata))]
    public partial class Orders
    {
        // Để trống, chỉ dùng để gắn cầu nối Metadata
    }

    public class OrderMetadata
    {
        [Required(ErrorMessage = "Hệ thống thiếu địa chỉ giao hàng.")]
        public string ShippingAddress { get; set; }

        [Required(ErrorMessage = "Hệ thống thiếu phương thức thanh toán.")]
        [StringLength(50, ErrorMessage = "Phương thức thanh toán quá dài.")]
        public string PaymentMethod { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(50)]
        public string DiscountCode { get; set; }
    }

    // =========================================================================================
    // 3. RÀNG BUỘC CHO CLASS "DiscountCodes" GỐC (Quản lý mã giảm giá của Admin)
    // =========================================================================================
    [MetadataType(typeof(DiscountCodeMetadata))]
    public partial class DiscountCodes
    {
        // Để trống, gắn cầu nối Metadata cho bảng DiscountCodes
    }

    public class DiscountCodeMetadata
    {
        [Required(ErrorMessage = "Vui lòng nhập tên mã giảm giá.")]
        [StringLength(50, ErrorMessage = "Mã giảm giá không được vượt quá 50 ký tự.")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Phần trăm giảm là bắt buộc.")]
        [Range(1, 100, ErrorMessage = "Phần trăm giảm giá phải nằm trong khoảng từ 1% đến 100%.")]
        public decimal DiscountPercent { get; set; }
    }
}