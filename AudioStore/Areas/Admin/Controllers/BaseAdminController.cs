using System.Web.Mvc;
using System.Web.Routing;

namespace AudioStore.Areas.Admin.Controllers
{
    // Kế thừa từ Controller mặc định
    public class BaseAdminController : Controller
    {
        // Hàm này sẽ tự động kích hoạt TRƯỚC KHI bất kỳ trang Admin nào được tải
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Kiểm tra: Nếu chưa đăng nhập HOẶC RoleID không phải là 1 (không phải Admin)
            if (Session["User"] == null || Session["RoleID"] == null || (int)Session["RoleID"] != 1)
            {
                // Đá văng người dùng về trang Đăng nhập ở khu vực người dùng (area = "")
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(
                        new { controller = "Account", action = "Login", area = "" }
                    )
                );
            }

            // Nếu đúng là Admin, cho phép đi tiếp
            base.OnActionExecuting(filterContext);
        }
    }
}