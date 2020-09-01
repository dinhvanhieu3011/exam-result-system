using Newtonsoft.Json;
using Quản_lý_điểm_thi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;

namespace Quản_lý_điểm_thi.Controllers
{
    public class HomeController : Controller
    {
        QLDTEntities1 db = new QLDTEntities1();
        // GET: Home
        public ActionResult Index()
        {
            //comment de bo qua login
            if (Session["IsLogin"] != null)
            {
                int rID = Convert.ToInt32(Session["RoleIDofUser"]);
                ViewBag.rID = rID;

                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }


        }

        #region Xu ly login, logout ---------------------------------------
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string user, string pwd, string remeber)
        {
            var lstMenu = db.MenuItems.ToList();

            if (user != "")
            {
                //check user
                var lstUser = db.Users.Where(p => p.Username == user && p.Password == pwd).ToList();
                if (lstUser.Count <= 0)
                {
                    ViewBag.LoginResult = "Tên đăng nhập hoặc mật khẩu không đúng";
                    return RedirectToAction("Login", "Home");
                }
                User loginUser = lstUser.FirstOrDefault();
                if ((!string.IsNullOrEmpty(remeber) && remeber == "true") || (remeber == "on"))
                {
                    HttpCookie cookie = new HttpCookie("qldt_userlogin");
                    cookie.Values["qldt_username"] = loginUser.Username;
                    cookie.Expires = DateTime.Now.AddDays(10);
                    Response.Cookies.Add(cookie);

                    cookie.Values["qldt_password"] = loginUser.Password;
                    cookie.Expires = DateTime.Now.AddDays(10);

                    cookie.Values["qldt_remember"] = remeber;
                    cookie.Expires = DateTime.Now.AddDays(10);
                    Response.Cookies.Add(cookie);
                }
                else
                {
                    HttpCookie cookie = new HttpCookie("qldt_userlogin");
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(cookie);
                }

                List<int> listRole = new List<int>();
                Session["curUser"] = lstUser[0];
                Session["IdUnitOfUser"] = lstUser[0].UnitId;
                Session["isAdmin"] = lstUser[0].IsAdmin;
                Session["IsLogin"] = 1;

                if (!string.IsNullOrEmpty(loginUser.Image))
                {
                    listRole = JsonConvert.DeserializeObject<List<int>>(loginUser.Image);
                }
                Session["ListRole"] = listRole;
                //Loc quyen theo role id -----------------------------------------------------------
                int roleID = 1, unitID = 1;
                unitID = Convert.ToInt32(lstUser[0].UnitId);


                //lstMenu = db.MenuItems.Where
                //-----------------------------------------------------------------------------------


                List<MnuItem> menu = new List<MnuItem>();

                foreach (MenuItem item in lstMenu)
                {
                    MnuItem cur = new MnuItem();

                    cur.Id = item.Id.ToString();
                    cur.MenuName = item.Name;
                    cur.Url = item.Url;
                    cur.ParentId = item.ParentId.ToString();
                    cur.OrderIndex = item.OrderIndex.ToString();
                    cur.Icon = String.IsNullOrEmpty(item.Icon) == true ? "" : item.Icon.ToString();

                    //menu.Add(cur);

                    //------------------------
                    //add menu theo role
                    if (db.Role_MenuItem.Where(p => p.MenuItemId == item.Id && p.RoleId == roleID).ToList().Count > 0)
                    {
                        menu.Add(cur);
                    }
                }

                menu = menu.OrderBy(x => x.ParentId).OrderBy(x => x.OrderIndex).ToList();
                Session["lstMenu"] = menu;
                Session["Username"] = lstUser[0].Username;
                Session["Fullname"] = lstUser[0].FullName;

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        /// <summary>
        /// Logout
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {

            Session["curUser"] = null;

            Session["IsLogin"] = null;

            Session["lstMenu"] = null;
            Session["Username"] = null;
            return RedirectToAction("Login", "Home");
        }

        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordModal model)
        {
            string message = "";
            bool isSuccess = false;
            if (ModelState.IsValid)
            {
                User currUser = Session["curUser"] as User;
                QLDTEntities1 _context = new QLDTEntities1();

                if (currUser != null)
                {
                    var checkUser = _context.Users.Where(p => p.Username == currUser.Username && p.Password == model.OldPassword).FirstOrDefault();
                    if (checkUser != null)
                    {
                        checkUser.Password = model.Password;
                        _context.SaveChanges();
                        Session["curUser"] = checkUser;

                        HttpCookie cookie = new HttpCookie("qldt_userlogin");
                        if (cookie.Values["qldt_remember"] != "true")
                        {
                            cookie.Values["qldt_username"] = checkUser.Username;
                            cookie.Expires = DateTime.Now.AddDays(10);
                            Response.Cookies.Add(cookie);

                            cookie.Values["qldt_password"] = checkUser.Password;
                            cookie.Expires = DateTime.Now.AddDays(10);

                            cookie.Values["qldt_remember"] = "true";
                            cookie.Expires = DateTime.Now.AddDays(10);
                            Response.Cookies.Add(cookie);
                        }

                        message = "Thay đổi password thành công.";
                        isSuccess = true;
                    }
                    else
                    {
                        message = "Không tìm thấy tài khoản của bạn";
                        isSuccess = false;
                    }
                }
                else
                {
                    message = "Xin hãy đăng nhập để sử dụng chức năng này.";
                    isSuccess = false;
                }
            }
            else
            {
                message = "Mật khẩu xác thực và mật khẩu không được bỏ trống và phải trùng nhau.";
                isSuccess = false;
            }
            return Json(new
            {
                Message = message,
                IsSuccess = isSuccess
            });
        }

        public ActionResult UpdateProfile(UpdateProfileModel model)
        {
            string message = "";
            bool isSuccess = false;
            if (ModelState.IsValid)
            {
                User userLogin = Session["curUser"] as User;
                QLDTEntities1 _context = new QLDTEntities1();
                if (userLogin != null)
                {
                    User currUser = _context.Users.Where(p => p.Username == userLogin.Username && p.Password == userLogin.Password).FirstOrDefault();
                    currUser.FullName = model.FullName;
                    currUser.Phone = model.Phone;
                    currUser.Mail = model.Mail;
                    _context.SaveChanges();
                    Session["curUser"] = currUser;
                    message = "Cập nhật thông tin thành công.";
                    isSuccess = true;
                }
                else
                {
                    message = "Xin hãy đăng nhập để sử dụng chức năng này.";
                    isSuccess = false;
                }
            }
            else
            {
                message = "Xin hãy nhập đủ thông tin.";
                isSuccess = false;
            }
            return Json(new
            {
                Message = message,
                IsSuccess = isSuccess
            });
        }
        #endregion ------------------------------------------------

        #region Search student
        [HttpGet]
        public ActionResult SearchStudent(string searchKey)
        {
            if (!string.IsNullOrEmpty(searchKey))
            {
                ViewBag.SearchKey = searchKey;
            }
            else
            {
                ViewBag.SearchKey = "";
            }
            return View("SearchStudent");
        }

        [HttpPost]
        public ActionResult LoadStudentData()
        {
            //int id_Exam_Room = Int32.Parse(id);
            try
            {
                //Creating instance of DatabaseContext class  
                using (QLDTEntities1 _context = new QLDTEntities1())
                {
                    var draw = Request.Form.GetValues("draw").FirstOrDefault();
                    var start = Request.Form.GetValues("start").FirstOrDefault();
                    var length = Request.Form.GetValues("length").FirstOrDefault();
                    var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                    var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                    var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();


                    //Paging Size (10,20,50,100)    
                    int pageSize = length != null ? Convert.ToInt32(length) : 0;
                    int skip = start != null ? Convert.ToInt32(start) : 0;
                    int recordsTotal = 0;
                    // Test thay đoi
                    // Getting all  data    
                    var listData = (from a in _context.Students
                                    where string.IsNullOrEmpty(searchValue) || a.ho_ten.ToLower().Contains(searchValue.ToLower()) || a.sbd.ToLower().Contains(searchValue.ToLower())
                                    //orderby a.Id
                                    select a).ToList<Student>();

                    //Search    
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        listData = listData.Where(m => m.ho_ten.Contains(searchValue) || m.sbd.Contains(searchValue)).ToList<Student>();
                    }

                    //total number of rows count     
                    recordsTotal = listData.Count();
                    //Paging     
                    var data = listData.Skip(skip).Take(pageSize).ToList();
                    //Returning Json Data    
                    return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        public ActionResult StatisticalBox()
        {
            QLDTEntities1 _context = new QLDTEntities1();
            Int32 totalStudent = _context.Students.Count();
            Int32 totalUser = _context.Users.Count();
            Int32 totalExam = _context.Exams.Count();

            ViewBag.TotalStudent = totalStudent;
            ViewBag.TotalUser = totalUser;
            ViewBag.TotalExam = totalExam;

            return View("_StatisticalBox");
        }

        [ChildActionOnly]
        public ActionResult StudentList()
        {
            QLDTEntities1 _context = new QLDTEntities1();
            ViewBag.ListKhoaThi = _context.Exams.ToList<Exam>();
            ViewBag.ListExamRoom = _context.ExamRooms.ToList<ExamRoom>();
            ViewBag.ListHDThi = _context.Hoi_dong_thi.ToList<Hoi_dong_thi>();
            ViewBag.ListGioiTinh = _context.GioiTinhs.ToList<GioiTinh>();
            ViewBag.ListTruong = _context.Truongs.ToList<Truong>();
            ViewBag.ListHKiem = _context.XepLoaiHanhKiems.ToList<XepLoaiHanhKiem>();
            ViewBag.ListTNghiep = _context.XepLoaiTotNghieps.ToList<XepLoaiTotNghiep>();
            ViewBag.ListHLuc = _context.XepLoaiHocLucs.ToList<XepLoaiHocLuc>();
            ViewBag.ListKetQua = _context.KetQuas.ToList<KetQua>();
            ViewBag.ListDienUT = _context.DienUuTiens.ToList<DienUuTien>();
            return View("_StudentList");
        }


        public ActionResult SideBarBox()
        {
            QLDTEntities1 _context = new QLDTEntities1();
            Int32 totalStudent = _context.Students.Count();
            Int32 totalUser = _context.Users.Count();
            Int32 totalExam = _context.Exams.Count();
            Int32 totalExamRoom = _context.ExamRooms.Count();

            ViewBag.TotalStudent = totalStudent;
            ViewBag.TotalUser = totalUser;
            ViewBag.TotalExam = totalExam;
            ViewBag.TotalExamRoom = totalExamRoom;

            return View("_SideBarBox");
        }


        public ActionResult GetListStudent(string draw, string start, string length, string identifyNumber, string examRoom, string candidateNumber,
            string examCouncil, string fullName, string toTestDay, string fromTestDay, string toBirthday, string fromBirthday, string exam, string ketQua,
            string truong, string gioiTinh, string loaiTN, string hanhKiem, string hocLuc, string dienUT)
        {
            QLDTEntities1 _context = new QLDTEntities1();
            int startPage = 0, drawPage = 0, totalRecord = 0;
            int lenghtPage = 0;
            Int32.TryParse(draw, out drawPage);
            Int32.TryParse(start, out startPage);
            if (!Int32.TryParse(length, out lenghtPage))
            {
                lenghtPage = 10;
            }

            toTestDay = ConvertDate(toTestDay);
            fromBirthday = ConvertDate(fromBirthday);
            toBirthday = ConvertDate(toBirthday);
            fromTestDay = ConvertDate(fromTestDay);

            var listHDThi = new List<Hoi_dong_thi>();
            var listExamRoom = new List<ExamRoom>();
            var listStudents = new List<Student>();
            if (!string.IsNullOrEmpty(examCouncil) || !string.IsNullOrEmpty(exam))
            {
                listHDThi = GetListHDThi(examCouncil, exam, _context);
            }
            if (!string.IsNullOrEmpty(examRoom) || (listHDThi != null && listHDThi.Any()))
            {
                listExamRoom = GetListExamRoom(examRoom, listHDThi, _context);
            }
            if (!string.IsNullOrEmpty(fullName) || !string.IsNullOrEmpty(candidateNumber) || listExamRoom.Any() || !string.IsNullOrEmpty(fromBirthday) || !string.IsNullOrEmpty(toBirthday)
                || !string.IsNullOrEmpty(ketQua) || !string.IsNullOrEmpty(truong) || !string.IsNullOrEmpty(hanhKiem) || !string.IsNullOrEmpty(hocLuc) || !string.IsNullOrEmpty(gioiTinh)
                || !string.IsNullOrEmpty(loaiTN) || !string.IsNullOrEmpty(dienUT) || !string.IsNullOrEmpty(exam) || !string.IsNullOrEmpty(examRoom) || !string.IsNullOrEmpty(examCouncil))
            {
                listStudents = GetListSudent(exam, examCouncil, listExamRoom, fullName, candidateNumber, fromBirthday, toBirthday, ketQua, truong, gioiTinh, loaiTN, hanhKiem, hocLuc, dienUT, _context);
            }
            else
            {
                listStudents = new List<Student>();
            }

            totalRecord = listStudents.Count();
            var data = listStudents.Skip(startPage).Take(lenghtPage).ToList();
            return Json(new
            {
                draw = draw,
                recordsFiltered = totalRecord,
                recordsTotal = totalRecord,
                data = data
            });
        }
        #endregion

        #region Utilities method
        private string ConvertDate(string dateTime)
        {
            if (!string.IsNullOrEmpty(dateTime))
            {
                DateTime d = DateTime.ParseExact(dateTime, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                return d.ToString("dd/MM/yyyy");
            }
            return dateTime;
        }

        private List<ExamRoom> GetListExamRoom(string examRoom, List<Hoi_dong_thi> listHDThi, QLDTEntities1 context)
        {
            int idExamRoom = 0;
            Int32.TryParse(examRoom, out idExamRoom);
            context.ExamRooms.Load();
            return context.ExamRooms.Local.Where(r => (listHDThi == null || !listHDThi.Any() || (listHDThi.Any(e => e.Id == r.ID_Exam)))
                                                && (string.IsNullOrEmpty(examRoom) || r.Id == idExamRoom)
                                         ).ToList<ExamRoom>();
        }

        private List<Hoi_dong_thi> GetListHDThi(string hoiDongThi, string exam, QLDTEntities1 context)
        {
            string _hoiDThi = string.IsNullOrEmpty(hoiDongThi) ? "" : hoiDongThi.ToLower();
            string _exam = string.IsNullOrEmpty(exam) ? "" : exam.ToLower();
            int intHDT = 0;
            Int32.TryParse(hoiDongThi, out intHDT);
            return context.Hoi_dong_thi.Where(a => (string.IsNullOrEmpty(_exam)
                                                  || a.value_11.ToLower().Contains(_exam)
                                                  || a.Id == intHDT)).ToList<Hoi_dong_thi>();
        }

        private List<Student> GetListSudent(string exam, string hoiDongThi, List<ExamRoom> listExamRoom, string fullName, string candidateNumber, string toBirthday, string fromBirday,
            string ketQua, string truong, string gioiTinh, string loaiTN, string hanhKiem, string hocLuc, string dienUT, QLDTEntities1 context)
        {
            List<int> listExamRoomId = (listExamRoom != null && listExamRoom.Any()) ? listExamRoom.Select(a => a.Id).ToList() : null;

            context.Students.Load();

            return context.Students.Local.Where(a => (listExamRoomId == null && string.IsNullOrEmpty(exam) && string.IsNullOrEmpty(hoiDongThi) || (listExamRoom.Any() && listExamRoomId.Contains(a.ID_Exam_Room)))
                       && (string.IsNullOrEmpty(fullName) || a.ho_ten.ToLower().Contains(fullName.ToLower()))
                       && (string.IsNullOrEmpty(candidateNumber) || a.sbd.ToLower().Contains(candidateNumber.ToLower()))
                       && (string.IsNullOrEmpty(fromBirday) || (!string.IsNullOrEmpty(a.ngay_sinh) && DateTime.Parse(a.ngay_sinh) > DateTime.Parse(fromBirday)))
                       && (string.IsNullOrEmpty(toBirthday) || (!string.IsNullOrEmpty(a.ngay_sinh) && DateTime.Parse(a.ngay_sinh) < DateTime.Parse(toBirthday)))
                       && (string.IsNullOrEmpty(truong) || (!string.IsNullOrEmpty(a.truong_hoc) && a.truong_hoc.ToLower().Contains(truong.ToLower())))
                       && (string.IsNullOrEmpty(gioiTinh) || (!string.IsNullOrEmpty(a.gioi_tinh) && a.gioi_tinh.ToLower().Contains(gioiTinh.ToLower())))
                       && (string.IsNullOrEmpty(loaiTN) || (!string.IsNullOrEmpty(a.xeploai_totnghiep) && a.xeploai_totnghiep.ToLower().Contains(loaiTN.ToLower())))
                       && (string.IsNullOrEmpty(hanhKiem) || (!string.IsNullOrEmpty(a.xeploai_hanhkiem) && a.xeploai_hanhkiem.ToLower().Contains(hanhKiem.ToLower())))
                       && (string.IsNullOrEmpty(hocLuc) || (!string.IsNullOrEmpty(a.xeploai_hocluc) && a.xeploai_hocluc.ToLower().Contains(hocLuc.ToLower())))
                       && (string.IsNullOrEmpty(ketQua) || (!string.IsNullOrEmpty(a.ketqua_thi) && a.ketqua_thi.ToLower().Contains(ketQua.ToLower())))
                       && (string.IsNullOrEmpty(dienUT) || (!string.IsNullOrEmpty(a.dien_uudai) && a.dien_uudai.ToLower().Contains(dienUT.ToLower())))
             ).ToList<Student>();
        }

        [HttpPost]
        public ActionResult GetHDongThi(int? id)
        {
            QLDTEntities1 _context = new QLDTEntities1();
            List<Hoi_dong_thi> listHD = new List<Hoi_dong_thi>();
            _context.Hoi_dong_thi.Load();
            listHD = _context.Hoi_dong_thi.Local.Where(a => id == null || a.value_11 == id.Value.ToString()).ToList<Hoi_dong_thi>();

            return Json(new { arrs = listHD });
        }

        [HttpPost]
        public ActionResult GetExamRoom(int? id)
        {
            QLDTEntities1 _context = new QLDTEntities1();
            List<ExamRoom> listExamRoom = new List<ExamRoom>();

            listExamRoom = _context.ExamRooms.Where(a => id == null || a.ID_Exam == id.Value).ToList<ExamRoom>();

            return Json(new { arrs = listExamRoom });
        }
        #endregion
    }
}