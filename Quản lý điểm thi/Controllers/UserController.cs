using Quản_lý_điểm_thi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using System.IO;
using Newtonsoft.Json;
using Quản_lý_điểm_thi.Common;

namespace Quản_lý_điểm_thi.Controllers
{
    public class UserController : Controller
    {
        private QLDTEntities1 _context;

        public UserController()
        {
            _context = new QLDTEntities1();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetListUser(string draw, string start, string length, string searchValue)
        {

            int startPage = 0, drawPage = 0, totalRecord = 0;
            int lenghtPage = 0;

            Int32.TryParse(draw, out drawPage);
            Int32.TryParse(start, out startPage);
            Int32.TryParse(length, out lenghtPage);

            var listData = (from a in _context.Users
                            where string.IsNullOrEmpty(searchValue)
                                || a.FullName.ToLower().Contains(searchValue.ToLower())
                                || a.Username.ToLower().Contains(searchValue.ToLower())
                                || a.Mail.ToLower().Contains(searchValue.ToLower())
                                || a.Phone.ToLower().Contains(searchValue.ToLower())
                                || a.CMND.ToLower().Contains(searchValue.ToLower())
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
        public ActionResult CreateUser()
        {
            return View("_CreateUser");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateNewUser(UserModel userModel)
        {
            string message = "";
            bool isSuccess = false;
            if (CheckRole(UserRole.Edit))
            {
                if (ModelState.IsValid)
                {
                    User user = new User
                    {
                        Username = userModel.Username,
                        Birthday = userModel.Birthday,
                        Password = userModel.Password,
                        FullName = userModel.FullName,
                        Address = userModel.Address,
                        IsAdmin = userModel.IsAdmin,
                        UnitId = userModel.UnitId,
                        Phone = userModel.Phone,
                        CMND = userModel.CMND,
                        Mail = userModel.Mail,
                    };
                    if (userModel.Role != null && userModel.Role.Any())
                    {
                        string role = JsonConvert.SerializeObject(userModel.Role);
                        user.Image = role;
                    }

                    _context.Users.Add(user);
                    _context.SaveChanges();

                    message = "Thêm người dùng mới thành công";
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
        public ActionResult UpdateUser()
        {
            return View("_UpdateUser");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateUser(UserModel userModel)
        {
            string message = "";
            bool isSuccess = false;
            if (CheckRole(UserRole.Edit))
            {
                if (ModelState.IsValid)
                {
                    User currUser = _context.Users.Where(u => u.Id == userModel.Id).FirstOrDefault();
                    if (currUser != null)
                    {
                        currUser.Username = userModel.Username;
                        currUser.Birthday = userModel.Birthday;
                        currUser.Password = userModel.Password;
                        currUser.FullName = userModel.FullName;
                        currUser.Address = userModel.Address;
                        currUser.IsAdmin = userModel.IsAdmin;
                        currUser.UnitId = userModel.UnitId;
                        currUser.Phone = userModel.Phone;
                        currUser.CMND = userModel.CMND;
                        currUser.Mail = userModel.Mail;
                    }
                    if (userModel.Role != null && userModel.Role.Any())
                    {
                        string roles = JsonConvert.SerializeObject(userModel.Role);
                        currUser.Image = roles;
                    }
                    _context.SaveChanges();

                    message = "Cập nhật người dùng  thành công";
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
                message = "Bạn chưa có quyển sửa thông tin của người dùng";
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
                User user = _context.Users.Where(p => p.Id == id).FirstOrDefault();
                if (user != null && user.Username != "admin" && user.Username != "u1" && user.Username != "u2" && user.Username != "u3")
                {
                    _context.Users.Remove(user);
                    _context.SaveChanges();
                    isSuccess = true;
                    message = "Đã xóa thành công";
                }
                else
                {
                    isSuccess = false;
                    message = "Khồng tìm thấy user";
                }
            }
            else
            {
                isSuccess = false;
                message = "Hãy chọn 1 user";
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
            int userId = 0;
            User user = new User();
            if (Int32.TryParse(Id, out userId))
            {
                user = _context.Users.Where(a => a.Id == userId).FirstOrDefault();
            }
            return Json(new { User = user }, JsonRequestBehavior.AllowGet);
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