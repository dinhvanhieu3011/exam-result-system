using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Quản_lý_điểm_thi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;

namespace Quản_lý_điểm_thi.Controllers
{
    public class HomeController : Controller
    {
        QLDTEntities1 db = new QLDTEntities1();
        private static List<Student> Students;
        private static List<Hoi_dong_thi> Hoi_dong_this;
        private static List<ExamRoom> ExamRooms;
        private static List<StudentModel> StudentModels;
        private static Dictionary<int, List<StudentModel>> dictionaryExam = new Dictionary<int, List<StudentModel>>();

        public HomeController()
        {

        }

        // GET: Home
        public ActionResult Index()
        {
            //comment de bo qua login
            if (Session["IsLogin"] != null)
            {
                int rID = Convert.ToInt32(Session["RoleIDofUser"]);
                ViewBag.rID = rID;

                QLDTEntities1 _context = new QLDTEntities1();
                Students = _context.Students.ToList<Student>();
                Hoi_dong_this = _context.Hoi_dong_thi.ToList<Hoi_dong_thi>();
                ExamRooms = _context.ExamRooms.ToList<ExamRoom>();
                List<int> listExamId = Hoi_dong_this.Select(a => Int32.Parse(a.value_11)).Distinct().ToList<int>();
                StudentModels = (from std in Students
                                 join er in ExamRooms on std.ID_Exam_Room equals er.Id
                                 join hdt in Hoi_dong_this on er.ID_Exam equals hdt.Id
                                 select new StudentModel(std.Id, std.tt, std.sbd, std.ho_ten, std.ngay_sinh,
                                                 std.gioi_tinh, std.coquan_congtac, std.truong_hoc, std.xeploai_hanhkiem, std.xeploai_hocluc,
                                                 std.dien_uudai, std.tong_so, std.ketqua_thi, std.xeploai_totnghiep, std.ghichu, std.ID_Exam_Room,
                                                 er.value_1, Int32.Parse(hdt.value_11), hdt.Id, hdt.value_1, std.create_date, std.create_user, std.pdf, std.dantoc)
                                 ).ToList<StudentModel>();
                dictionaryExam.Clear();
                foreach (var id in listExamId)
                {
                    List<StudentModel> listStudentModel = StudentModels.Where(a => a.ID_Exam == id).ToList<StudentModel>();
                    dictionaryExam.Add(id, listStudentModel);
                }
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult ReloadEnityList()
        {
            QLDTEntities1 _context = new QLDTEntities1();
            Students = _context.Students.ToList<Student>();
            Hoi_dong_this = _context.Hoi_dong_thi.ToList<Hoi_dong_thi>();
            ExamRooms = _context.ExamRooms.ToList<ExamRoom>();
            return Json(new
            {
                Message = "update complete",
            });
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
                    return View();
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

            var listStudents = new List<StudentModel>();
            int intExamID = 0;
            Int32.TryParse(exam, out intExamID);
            if (!string.IsNullOrEmpty(exam))
            {
                listStudents = dictionaryExam[intExamID];
            }
            else if ( !string.IsNullOrEmpty(exam) || !string.IsNullOrEmpty(examRoom) || !string.IsNullOrEmpty(examCouncil) || !string.IsNullOrEmpty(fullName) || !string.IsNullOrEmpty(candidateNumber) || !string.IsNullOrEmpty(fromBirthday) || !string.IsNullOrEmpty(toBirthday)
                || !string.IsNullOrEmpty(ketQua) || !string.IsNullOrEmpty(truong) || !string.IsNullOrEmpty(hanhKiem) || !string.IsNullOrEmpty(hocLuc) || !string.IsNullOrEmpty(gioiTinh)
                || !string.IsNullOrEmpty(loaiTN) || !string.IsNullOrEmpty(dienUT) )
            {
                listStudents = GetListSudent(examCouncil, fullName, candidateNumber, fromBirthday, toBirthday, ketQua, truong, gioiTinh, loaiTN, hanhKiem, hocLuc, dienUT, exam, examCouncil, examRoom);
            }
            else
            {
                listStudents = new List<StudentModel>();
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
            return ExamRooms.Where(r => (listHDThi == null || !listHDThi.Any() || (listHDThi.Any(e => e.Id == r.ID_Exam)))
                                                && (string.IsNullOrEmpty(examRoom) || r.Id == idExamRoom)
                                         ).ToList<ExamRoom>();
        }

        private List<Hoi_dong_thi> GetListHDThi(string hoiDongThi, string exam)
        {
            string _hoiDThi = string.IsNullOrEmpty(hoiDongThi) ? "" : hoiDongThi.ToLower();
            string _exam = string.IsNullOrEmpty(exam) ? "" : exam.ToLower();
            int intHDT = 0;
            Int32.TryParse(hoiDongThi, out intHDT);
            return Hoi_dong_this.Where(a => (string.IsNullOrEmpty(_exam)
                                                  || a.value_11 == _exam
                                                  || a.Id == intHDT)).ToList<Hoi_dong_thi>();
        }

        private List<StudentModel> GetListSudent(string hoiDongThi, string fullName, string candidateNumber, string toBirthday, string fromBirday,
        string ketQua, string truong, string gioiTinh, string loaiTN, string hanhKiem, string hocLuc, string dienUT, string examId, string hoiDHId, string examRoomId, List<int> listHDId)
        {

            return StudentModels.Where(a => ((listHDId != null && listHDId.Any()) || listHDId.Contains(a.ID_HD_Thi))
                       && (string.IsNullOrEmpty(examRoomId) || a.ID_Exam_Room == Int32.Parse(examRoomId))
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
             ).ToList<StudentModel>();
        }


        private List<StudentModel> GetListSudent(string hoiDongThi, string fullName, string candidateNumber, string toBirthday, string fromBirday,
            string ketQua, string truong, string gioiTinh, string loaiTN, string hanhKiem, string hocLuc, string dienUT, string examId, string hoiDHId, string examRoomId)
        {
            var intExamID = 0;
       
            return StudentModels.Where(a => (string.IsNullOrEmpty(examId) || a.ID_Exam.ToString().Contains(examId))
                       && (string.IsNullOrEmpty(hoiDHId) || a.ID_HD_Thi == Int32.Parse(hoiDHId))
                       && (string.IsNullOrEmpty(examRoomId) || a.ID_Exam_Room == Int32.Parse(examRoomId))
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
             ).ToList<StudentModel>();
        }

        [HttpPost]
        public ActionResult GetHDongThi(int? id)
        {
            QLDTEntities1 _context = new QLDTEntities1();
            List<Hoi_dong_thi> listHD = new List<Hoi_dong_thi>();
            listHD = Hoi_dong_this.Where(a => id == null || a.value_11 == id.Value.ToString()).ToList<Hoi_dong_thi>();

            return Json(new { arrs = listHD });
        }

        [HttpPost]
        public ActionResult GetExamRoom(int? id)
        {
            List<ExamRoom> listExamRoom = new List<ExamRoom>();

            listExamRoom = ExamRooms.Where(a => id == null || a.ID_Exam == id.Value).ToList<ExamRoom>();

            return Json(new { arrs = listExamRoom });
        }
        #endregion

        public ActionResult ExportExcelSearchResult(string identifyNumber, string examRoom, string candidateNumber,
            string examCouncil, string fullName, string toTestDay, string fromTestDay, string toBirthday, string fromBirthday, string exam, string ketQua,
            string truong, string gioiTinh, string loaiTN, string hanhKiem, string hocLuc, string dienUT)
        {
            QLDTEntities1 _context = new QLDTEntities1();
            string message = "";
            bool isSuccess = false;
            int i = 0;
            try
            {

                toTestDay = ConvertDate(toTestDay);
                fromBirthday = ConvertDate(fromBirthday);
                toBirthday = ConvertDate(toBirthday);
                fromTestDay = ConvertDate(fromTestDay);

                var listHDThi = new List<Hoi_dong_thi>();
                var listExamRoom = new List<ExamRoom>();
                var listStudents = new List<StudentModel>();
                if (!string.IsNullOrEmpty(examCouncil) || !string.IsNullOrEmpty(exam))
                {
                    listHDThi = GetListHDThi(examCouncil, exam);
                }
                if (!string.IsNullOrEmpty(examRoom) || (listHDThi != null && listHDThi.Any()))
                {
                    listExamRoom = GetListExamRoom(examRoom, listHDThi, _context);
                }
                if (!string.IsNullOrEmpty(fullName) || !string.IsNullOrEmpty(candidateNumber) || listExamRoom.Any() || !string.IsNullOrEmpty(fromBirthday) || !string.IsNullOrEmpty(toBirthday)
                    || !string.IsNullOrEmpty(ketQua) || !string.IsNullOrEmpty(truong) || !string.IsNullOrEmpty(hanhKiem) || !string.IsNullOrEmpty(hocLuc) || !string.IsNullOrEmpty(gioiTinh)
                    || !string.IsNullOrEmpty(loaiTN) || !string.IsNullOrEmpty(dienUT) || !string.IsNullOrEmpty(exam) || !string.IsNullOrEmpty(examRoom) || !string.IsNullOrEmpty(examCouncil))
                {
                    listStudents = GetListSudent(examCouncil, fullName, candidateNumber, fromBirthday, toBirthday, ketQua, truong, gioiTinh, loaiTN, hanhKiem, hocLuc, dienUT, exam, examCouncil, examRoom);
                }
                else
                {
                    listStudents = new List<StudentModel>();
                }

                if (listStudents.Any())
                {
                    string fileName = "\\Excel\\Home\\mau_xua_excel.xlsx";
                    string outFolder = "\\Excel\\Home\\Search";
                    string fileClone = "ket_qua_tim_kiem", fileClonePath;
                    int startRowList = 10, stt = 1;
                    string excelsPath = Server.MapPath(fileName);
                    var folder = Server.MapPath(outFolder);
                    fileClonePath = folder + "\\" + fileClone + ".xlsx";

                    if (Directory.Exists(folder))
                    {
                        string[] files = Directory.GetFiles(folder);
                        foreach (string file in files)
                        {
                            System.IO.File.SetAttributes(file, FileAttributes.Normal);
                            System.IO.File.Delete(file);
                        }

                    }
                    else
                    {
                        Directory.CreateDirectory(folder);
                    }

                    System.IO.File.Copy(excelsPath, folder + "\\" + fileClone + ".xlsx");

                    FileInfo fileInfo = new FileInfo(fileClonePath);
                    using (ExcelPackage excelPack = new ExcelPackage(fileInfo))
                    {
                        ExcelWorksheet worksheet = excelPack.Workbook.Worksheets[1];
                        if (listExamRoom == null || !listExamRoom.Any())
                        {
                            List<int> listIDExamRoom = listStudents.Select(s => s.ID_Exam_Room).ToList<int>();
                            listExamRoom = _context.ExamRooms.Where(e => listIDExamRoom.Contains(e.Id)).ToList<ExamRoom>();
                        }
                        if (listHDThi == null || !listHDThi.Any())
                        {
                            List<int> listHDThiId = listExamRoom.Select(s => s.ID_Exam).ToList<int>();
                            listHDThi = _context.Hoi_dong_thi.Where(e => listHDThiId.Contains(e.Id)).ToList<Hoi_dong_thi>();
                        }

                        if (!string.IsNullOrEmpty(exam))
                        {
                            var _exam = _context.Exams.Where(e => e.Id.ToString() == exam).FirstOrDefault();
                            worksheet.Cells[4, 2].Value = _exam.value_1;
                        }
                        if (!string.IsNullOrEmpty(examCouncil))
                        {
                            var _examCon = _context.Hoi_dong_thi.Where(e => e.Id.ToString() == examCouncil).FirstOrDefault();
                            worksheet.Cells[5, 2].Value = _examCon.value_1;
                        }
                        if (!string.IsNullOrEmpty(examRoom))
                        {
                            var _examRoom = _context.ExamRooms.Where(e => e.Id.ToString() == examRoom).FirstOrDefault();
                            worksheet.Cells[6, 2].Value = _examRoom.value_1;
                        }
                        worksheet.Cells[startRowList - 1, 1].Value = "STT";
                        worksheet.Cells[startRowList - 1, 2].Value = "Họ và tên";
                        worksheet.Cells[startRowList - 1, 3].Value = "Số báo danh";
                        worksheet.Cells[startRowList - 1, 4].Value = "Ngày sinh";
                        worksheet.Cells[startRowList - 1, 5].Value = "Giới tính";
                        worksheet.Cells[startRowList - 1, 6].Value = "Dân tộc";
                        worksheet.Cells[startRowList - 1, 7].Value = "Trường học";
                        worksheet.Cells[startRowList - 1, 8].Value = "Hạnh kiểm";
                        worksheet.Cells[startRowList - 1, 9].Value = "Học lực";
                        worksheet.Cells[startRowList - 1, 10].Value = "Loại tốt nghiệp";
                        worksheet.Cells[startRowList - 1, 11].Value = "Phòng thi";
                        worksheet.Cells[startRowList - 1, 12].Value = "Hội đồng thi";
                        worksheet.Cells[startRowList - 1, 13].Value = "Niên khóa";
                        worksheet.Cells[startRowList - 1, 14].Value = "Quê quán";

                        #region THeem diem
                        if (!string.IsNullOrEmpty(exam))
                        {
                            i = 15;
                            var _exam = _context.Exams.Where(e => e.Id.ToString() == exam).FirstOrDefault();
                            List<MT_VALUE_NAME> lstmt = db.MT_VALUE_NAME.Where(x => x.ID_MT_VALUE_NAME == _exam.ID_MT_VALUE_NAME + "").ToList();

                            foreach (MT_VALUE_NAME mt in lstmt)
                            {
                                if (mt.status != "hidden")
                                {
                                    worksheet.Cells[startRowList - 1, i].Value = mt.mo_ta.ToString();
                                    //worksheet.Cells[startRowList, i + 2].Value = GetPropValue(g, mt.name.ToString()).ToString();
                                    i++;
                                }

                            }
                            for (int j = 0; j < 20; j++)
                            {
                                worksheet.Cells[startRowList - 1, i + j + 1].Value = "";

                            }
                        }

                        foreach (var student in listStudents)
                        {
                            if (startRowList > 200)
                            {
                                break;
                            }
                            ExamRoom exRoom = listExamRoom.Where(a => a.Id == student.ID_Exam_Room).FirstOrDefault();
                            Hoi_dong_thi hdThi = listHDThi.Where(a => a.Id == exRoom.ID_Exam).FirstOrDefault();
                            Exam ex = _context.Exams.Where(a => a.Id.ToString() == hdThi.value_11).FirstOrDefault();
                            string birthDay = "";
                            if (!string.IsNullOrEmpty(student.ngay_sinh))
                            {
                                string[] arr = student.ngay_sinh.Split('-');
                                birthDay = arr[2] + "-" + arr[1] + "-" + arr[0];
                            }
                            Grade g = db.Grades.Where(x => x.ID_Student == student.Id).FirstOrDefault();
                            worksheet.Cells[startRowList, 1].Value = stt++;
                            worksheet.Cells[startRowList, 2].Value = student.ho_ten;
                            worksheet.Cells[startRowList, 3].Value = student.sbd;
                            worksheet.Cells[startRowList, 4].Value = birthDay;
                            worksheet.Cells[startRowList, 5].Value = student.gioi_tinh;
                            worksheet.Cells[startRowList, 6].Value = student.dantoc;
                            worksheet.Cells[startRowList, 7].Value = student.truong_hoc;
                            worksheet.Cells[startRowList, 8].Value = student.xeploai_hanhkiem;
                            worksheet.Cells[startRowList, 9].Value = student.xeploai_hocluc;
                            worksheet.Cells[startRowList, 10].Value = student.xeploai_totnghiep;
                            worksheet.Cells[startRowList, 11].Value = exRoom.value_1;
                            worksheet.Cells[startRowList, 12].Value = hdThi.value_1;
                            worksheet.Cells[startRowList, 13].Value = ex.value_1;
                            worksheet.Cells[startRowList, 14].Value = student.coquan_congtac;
                            #region THeem diem
                            if (!string.IsNullOrEmpty(exam))
                            {
                                i = 15;
                                var _exam = _context.Exams.Where(e => e.Id.ToString() == exam).FirstOrDefault();
                                List<MT_VALUE_NAME> lstmt = db.MT_VALUE_NAME.Where(x => x.ID_MT_VALUE_NAME == _exam.ID_MT_VALUE_NAME + "").ToList();

                                foreach (MT_VALUE_NAME mt in lstmt)
                                {
                                    if (mt.status != "hidden")
                                    {
                                        // worksheet.Cells[startRowList - 1, i].Value = mt.mo_ta.ToString();
                                        worksheet.Cells[startRowList, i].Value = GetPropValue(g, mt.name.ToString()).ToString();

                                        i++;
                                    }

                                }
                                for (int j = 0; j < 20; j++)
                                {
                                    worksheet.Cells[startRowList, i + j + 1].Value = "";

                                }
                            }
                            #endregion
                            startRowList++;
                        }
                        using (var range = worksheet.Cells["A9:AB9"])
                        {
                            // Set PatternType
                            // range.Style.Fill.PatternType = ExcelFillStyle.DarkGray;
                            // Set Màu cho Background
                            // range.Style.Fill.BackgroundColor.SetColor(Color.Aqua);
                            // Canh giữa cho các text
                            range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            // Set Font cho text  trong Range hiện tại
                            range.Style.Font.SetFromFont(new Font("Times New Roman", 12));
                        }
                        #endregion
                        excelPack.Save();
                        byte[] fileBytes = System.IO.File.ReadAllBytes(fileClonePath);
                        isSuccess = true;
                        message = "Hệ thống sẽ trả về 1 file excel";
                        TempData["OutputExcels"] = fileBytes;
                    }

                }
                else
                {
                    message = "Không có bản ghi nào, không thể kết xuất file";
                }
            }
            catch (Exception ex)
            {
                message = ex.ToString();
            }

            return Json(new
            {
                Message = message,
                IsSuccess = isSuccess
            });
        }
        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
        public ActionResult DownloadExcel()
        {
            // retrieve byte array here
            var array = TempData["OutputExcels"] as byte[];
            if (array != null)
            {
                return File(array, System.Net.Mime.MediaTypeNames.Application.Octet, "Quan_ly_diem_thi.xlsx");
            }
            else
            {
                return new EmptyResult();
            }
        }
    }
}