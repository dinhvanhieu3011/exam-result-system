using Newtonsoft.Json;
using Quản_lý_điểm_thi.Common;
using Quản_lý_điểm_thi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Quản_lý_điểm_thi.Controllers
{
    public class GenderController : Controller
    {
        private QLDTEntities1 _context;

        public GenderController()
        {
            _context = new QLDTEntities1();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetListGioiTinh(string draw, string start, string length, string searchValue)
        {

            int startPage = 0, drawPage = 0, totalRecord = 0;
            int lenghtPage = 0;

            Int32.TryParse(draw, out drawPage);
            Int32.TryParse(start, out startPage);
            Int32.TryParse(length, out lenghtPage);

            var listData = (from a in _context.GioiTinhs
                            where string.IsNullOrEmpty(searchValue)
                                || a.Ten.ToLower().Contains(searchValue.ToLower())
                            orderby a.Id
                            select a);
            totalRecord = listData.Count();

            var data = listData.Skip(startPage).Take(lenghtPage).ToList();
            return Json(new
            {
                draw = draw,
                recordsFiltered = totalRecord,
                recordsTotal = totalRecord,
                data = data
            });
        }

        [HttpGet]
        public ActionResult CreateGioiTinh()
        {
            return View("_CreateGioiTinh");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateGioiTinh(GenderModel model)
        {
            string message = "";
            bool isSuccess = false;
            User user = GetUserFromSession();
            if (CheckRole(UserRole.Create))
            {
                if (ModelState.IsValid)
                {
                    GioiTinh gioiTinh = new GioiTinh
                    {
                       Ten = model.Name,
                       Mo_Ta = model.Description,
                       Ghi_chu = model.Note,
                       create_date = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                       create_user = user.Id.ToString()
                    };
                    _context.GioiTinhs.Add(gioiTinh);
                    _context.SaveChanges();

                    message = "Thêm giới tính mới thành công";
                    isSuccess = true;

                }
                else
                {
                    message = "Bạn chưa nhập đầy đủ thông";
                    isSuccess = false;
                }
            }
            else
            {
                message = "Bạn chưa có quyền thêm, liên lạc admin để được giúp đỡ";
                isSuccess = false;
            }
            return Json(new
            {
                Message = message,
                IsSuccess = isSuccess
            });
        }

        [HttpGet]
        public ActionResult UpdateGioiTinh()
        {
            return View("_UpdateGioiTinh");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateGioiTinh(GenderModel model)
        {
            string message = "";
            bool isSuccess = false;
            if (CheckRole(UserRole.Edit))
            {
                if (ModelState.IsValid)
                {
                    GioiTinh currGT = _context.GioiTinhs.Where(u => u.Id == model.Id).FirstOrDefault();
                    if (currGT != null)
                    {
                        currGT.Ten = model.Name;
                        currGT.Mo_Ta = model.Description;
                        currGT.Ghi_chu = model.Note;
                    }
                    _context.SaveChanges();

                    message = "Cập nhật giới tính  thành công";
                    isSuccess = true;

                }
                else
                {
                    message = "Bạn chưa nhập đầy đủ thông";
                    isSuccess = false;
                }
            }
            else
            {
                message = "Bạn chưa có quyển sửa thông tin của giới tính";
                isSuccess = false;
            }
            return Json(new
            {
                Message = message,
                IsSuccess = isSuccess
            });
        }

        [HttpPost]
        public ActionResult Delete(int? id)
        {
            string message = "";
            bool isSuccess = false;
            if (id >= 0)
            {
                GioiTinh gioiTinh = _context.GioiTinhs.Where(p => p.Id == id).FirstOrDefault();
                if (gioiTinh != null )
                {
                    _context.GioiTinhs.Remove(gioiTinh);
                    _context.SaveChanges();
                    isSuccess = true;
                    message = "Đã xóa thành công";
                }
                else
                {
                    isSuccess = false;
                    message = "Khồng tìm thấy gioi tinh";
                }
            }
            else
            {
                isSuccess = false;
                message = "Hãy chọn 1 gioi tinh";
            }
            return Json(new
            {
                Message = message,
                IsSuccess = isSuccess
            });
        }

        [HttpGet]
        public ActionResult GetById(string Id)
        {
            int intId = 0;
            GioiTinh gioiTinh = new GioiTinh();
            if (Int32.TryParse(Id, out intId))
            {
                gioiTinh = _context.GioiTinhs.Where(a => a.Id == intId).FirstOrDefault();
            }
            return Json(new { GioiTinh = gioiTinh }, JsonRequestBehavior.AllowGet);
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
        
       private User GetUserFromSession()
        {
            User user = Session["curUser"] as User;
            if(user == null)
            {
                user = new User();
            }
            return user;
        }
    }
}