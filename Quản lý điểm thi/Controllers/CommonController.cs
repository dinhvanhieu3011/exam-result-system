using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Quản_lý_điểm_thi.Controllers
{
    public class CommonController : Controller
    {
        // GET: Common
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MessageBox()
        {
            return View("_MessageBox");
        }

        public ActionResult CheckRole(string role)
        {
            string message = "";
            bool isSuccess = false;
            int intRole = 0;
            if(Int32.TryParse(role, out intRole))
            {
                if (CheckRole(intRole))
                {
                    message = "Cập nhật người dùng  thành công";
                    isSuccess = true;
                }
                else
                {
                    message = "Bạn Không có quyển thực hiện hành động này, xin hãy thử lại với các hành động khác hoặc liên hệ admin để xin cấp thêm quyền.";
                    isSuccess = false;
                }
            }
            else
            {
                message = "không tìm thấy quyền của bạn, xin hãy thử lại với các hành động khác.";
                isSuccess = false;
            }
           
            return Json(new
            {
                Message = message,
                IsSuccess = isSuccess
            });
        }

        private bool CheckRole(int role)
        {
            List<int> listRole = Session["ListRole"] as List<int>;
            if (listRole != null && listRole.Any())
            {
                if (listRole.Contains(role))
                {
                    return true;
                }
            }
            return false;
        }
    }
}