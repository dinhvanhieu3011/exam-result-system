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
using System.Text.RegularExpressions;

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

                    user = _context.Users.Add(user);
                    _context.SaveChanges();
                    if (userModel.Avatar != null)
                    {
                        user = SaveImage(userModel.Avatar, user);
                    }
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
                    if (userModel.Avatar != null)
                    {
                        currUser = SaveImage(userModel.Avatar, currUser);
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
                    var folder = Server.MapPath(user.AvatarFolderPath);
                    if (Directory.Exists(folder))
                    {
                        var parrentFolder = Directory.GetParent(folder);
                        string[] files = Directory.GetFiles(folder);
                        foreach (string file in files)
                        {
                            System.IO.File.SetAttributes(file, FileAttributes.Normal);
                            System.IO.File.Delete(file);
                        }
                        Directory.Delete(folder);
                        parrentFolder.Refresh();
                        parrentFolder.Delete();
                    }

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

        private User SaveImage(HttpPostedFileBase file, User user)
        {
            string fileName = GetFileName(user);
            string fileExtention = Path.GetExtension(file.FileName);
            string fullFileName = fileName + fileExtention;

            string folferPath = "~/Image/User/"+ user.Id +"/"+ fileName;
            string url = "/Image/User/" + user.Id + "/" + fileName+"/"+fullFileName;
            string path = folferPath + "\\" + fullFileName;
            var folder = Server.MapPath(folferPath);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            else
            {
                string[] files = Directory.GetFiles(folder);
                foreach (string fileImg in files)
                {
                    System.IO.File.SetAttributes(fileImg, FileAttributes.Normal);
                    System.IO.File.Delete(fileImg);
                }
            }
            string fullPath = Path.Combine(Server.MapPath(folferPath), fullFileName);

            file.SaveAs(fullPath);
            user.AvatarFolderPath = folferPath;
            user.AvatarUrl = url;
            return user;
        }

        public static string RemoveUnicode(string text)
        {
            string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
                                    "đ",
                                    "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ",
                                    "í","ì","ỉ","ĩ","ị",
                                    "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",
                                    "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",
                                    "ý","ỳ","ỷ","ỹ","ỵ",};
            string[] arr2 = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
                                    "d",
                                    "e","e","e","e","e","e","e","e","e","e","e",
                                    "i","i","i","i","i",
                                    "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",
                                    "u","u","u","u","u","u","u","u","u","u","u",
                                    "y","y","y","y","y",};
            for (int i = 0; i < arr1.Length; i++)
            {
                text = text.Replace(arr1[i], arr2[i]);
                text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
            }
            return text;
        }

        private string GetFileName(User user)
        {
            if (!string.IsNullOrEmpty(user.Username))
            {
                string fileName = RemoveUnicode(user.Username);
                fileName = Regex.Replace(fileName, @"[^0-9a-zA-Z]+", "_") + "_id_" + user.Id;
                return fileName;
            }
            else
            {
                return "id_" + user.Id;
            }
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