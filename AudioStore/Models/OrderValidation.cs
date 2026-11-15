using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AudioStore.Models
{
    // =========================================================================================
    // 1. CHECKOUT VIEW MODEL - KIỂM TRA HỢP LỆ THÔNG TIN NHẬN HÀNG (TƯƠNG THÍCH CẢ JS & C#)
    // =========================================================================================
    public class CheckoutViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập họ và tên người nhận.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Họ và tên phải từ 2 đến 100 ký tự.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Số điện thoại là bắt buộc.")]
        [RegularExpression(@"^0[0-9]{9,10}$", ErrorMessage = "Số điện thoại không hợp lệ (gồm 10-11 chữ số, bắt đầu bằng số 0).")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ Email.")]
        [EmailAddress(ErrorMessage = "Định dạng email không hợp lệ (VD: ten@gmail.com).")]
        [StringLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ giao hàng chi tiết.")]
        [StringLength(500, MinimumLength = 5, ErrorMessage = "Vui lòng nhập địa chỉ cụ thể hơn (tối thiểu 5 ký tự).")]
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
    }

    public class OrderMetadata
    {
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ giao hàng.")]
        [StringLength(500, MinimumLength = 5, ErrorMessage = "Địa chỉ giao hàng phải từ 5 đến 500 ký tự.")]
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
    }

    public class DiscountCodeMetadata
    {
        [Required(ErrorMessage = "Vui lòng nhập tên mã giảm giá.")]
        [StringLength(50, ErrorMessage = "Mã giảm giá không được vượt quá 50 ký tự.")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Phần trăm giảm là bắt buộc.")]
        [Range(1, 100, ErrorMessage = "Phần trăm giảm giá phải nằm trong khoảng từ 1% đến 100%.")]
        public int DiscountPercent { get; set; }
    }
}