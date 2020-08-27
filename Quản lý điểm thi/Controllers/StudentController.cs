using Quản_lý_điểm_thi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using System.IO;
using ExcelDataReader;
using System.Data;

namespace Quản_lý_điểm_thi.Controllers
{
    public class StudentController : Controller
    {
        QLDTEntities1 db = new QLDTEntities1();
        int id_Room;
        String folder;
        // GET: Student
        //id = id phòng thi
        public ActionResult Index(int id)
        {
            List<GioiTinh> Gt = db.GioiTinhs.ToList();
            ViewBag.lstGioiTinh = Gt;
            List<Truong> Truong = db.Truongs.ToList();
            ViewBag.lstTruong = Truong;
            List<XepLoaiHanhKiem> XepLoaiHanhKiem = db.XepLoaiHanhKiems.ToList();
            ViewBag.lstXepLoaiHanhKiem = XepLoaiHanhKiem;
            List<XepLoaiHocLuc> XepLoaiHocLuc = db.XepLoaiHocLucs.ToList();
            ViewBag.lstXepLoaiHocLuc = XepLoaiHocLuc;
            List<XepLoaiTotNghiep> XepLoaiTotNghiep = db.XepLoaiTotNghieps.ToList();
            ViewBag.lstXepLoaiTotNghiep = XepLoaiTotNghiep;
            List<DienUuTien> DienUuTien = db.DienUuTiens.ToList();
            ViewBag.DienUuTien = DienUuTien;




            ExamRoom room = db.ExamRooms.Where(x => x.Id == id).First();
            ViewBag.TenPhong = room.value_1;
            ViewBag.IdPhong = room.Id;

            Hoi_dong_thi Hdt = db.Hoi_dong_thi.Where(x => x.Id == room.ID_Exam).First();
            ViewBag.TenHoiDongThi = Hdt.value_1;
            ViewBag.IDHoiDongThi = Hdt.Id;

            string ID_Exam = Hdt.value_11.ToString();
            int id_Exam_int = Int32.Parse(ID_Exam);
            Exam Exam = db.Exams.Where(x => x.Id == id_Exam_int).First();
            ViewBag.ID_Exam = ID_Exam;
            ViewBag.TenExam = Exam.khoa_thi;
            this.folder = Server.MapPath("~/PDF/" + id);
            string[] filePaths = Directory.GetFiles(folder, "*.pdf").Select(Path.GetFileName).ToArray(); ;

            ViewBag.filePaths = filePaths;
            List<string> header = new List<string>();
            List<string> example = new List<string>();
            //new string[] { "CMND", "STT", "SBD", "HoTen", "NgaySinh", "GioiTinh", "QueQuan", "TruongHoc", "KetQua", "GhiChu" };
            #region
            header.Add("CMND");
            header.Add("STT");
            header.Add("SBD");
            header.Add("HoTen");
            header.Add("NgaySinh");
            header.Add("GioiTinh");
            header.Add("QueQuan");
            //
            header.Add("DanToc");
            //
            header.Add("TruongHoc");
            header.Add("KetQua");
            header.Add("GhiChu");

            header.Add("XepLoaiHocLuc");
            header.Add("XepLoaiHanhKiem");
            header.Add("XepLoaiTotNghiep");
            header.Add("DienUuDai");

            example.Add("So CMND");
            example.Add("So thu tu");
            example.Add("SBD");
            example.Add("Ho ten");
            example.Add("Ngay sinh(yyyy-MM-dd)");
            example.Add("Gioi tinh");
            example.Add("Que quan");
            //
            example.Add("Dan Toc");
            //
            example.Add("Truong hoc");
            example.Add("Ket qua");
            example.Add("Ghi chu");
            example.Add("Xep Loai Hoc Luc");
            example.Add("Xep Loai Hanh Kiem");
            example.Add("Xep Loai Tot Nghiep");
            example.Add("Dien Uu Tien");
            #endregion

            //new string[] { "CMND", "STT", "SBD", "HoTen", "yyyy-MM-dd", "GioiTinh", "QueQuan", "TruongHoc", "KetQua", "GhiChu" };
            string ID_MT = db.Exams.Where(x => x.Id == id_Exam_int).First().ID_MT_VALUE_NAME.ToString();
            //List<MT_VALUE_NAME> lst_MT = db.MT_VALUE_NAME.Where(x => x.ID_MT_VALUE_NAME == ID_MT.ToString()).Where(x=>x.status!="hidden").OrderBy(x=>x.status).ToList();
            List<MT_VALUE_NAME> lst_MT = db.MT_VALUE_NAME.Where(x => x.ID_MT_VALUE_NAME == ID_MT.ToString()).ToList();

            int i = 10;
            foreach (MT_VALUE_NAME a in lst_MT)
            {
                header.Add(a.name);
                if (a.type == "date")
                {
                    example.Add(RemoveUnicode(a.mo_ta) + "(yyyy-MM-dd)");
                }
                else
                {
                    example.Add(RemoveUnicode(a.mo_ta));
                }
            }

            ViewBag.header = header;
            ViewBag.example = example;

            ViewBag.id = id;
            return View();
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
        public ActionResult test()
        {
            return View();
        }
        public string FirstLetterToUpper(string str)
        {
            if (str == null)
                return null;

            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);

            return str.ToUpper();
        }
        [HttpPost]
        public ActionResult UploadFiles(string id)
        {
            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  

                        HttpPostedFileBase file = files[i];
                        string fname;

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }

                        // Get the complete folder path and store the file inside it.  
                        fname = Path.Combine(Server.MapPath("~/PDF/" + id), fname);
                        file.SaveAs(fname);
                    }
                    // Returns message that successfully uploaded  
                    return Json("File Uploaded Successfully!");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }
        public ActionResult UploadExcel(string id)
        {
            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  

                        HttpPostedFileBase file = files[i];
                        string fname;

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = DateTime.Now.ToString("dddd dd MMMM yyyy mm ss") + ".xls";
                        }
                        else
                        {
                            fname = DateTime.Now.ToString("dddd dd MMMM yyyy mm ss") + ".xls";
                        }

                        // Get the complete folder path and store the file inside it.  
                        string Fullfname = Path.Combine(Server.MapPath("~/Excel/" + id), fname);
                        file.SaveAs(Fullfname);
                        string path = HttpContext.Server.MapPath("~/Excel/" + id + "/" + fname);
                        int a = Int32.Parse(id);
                        ImportDataFromExcel(path, a);
                    }
                    // Returns message that successfully uploaded  
                    return Json("Thêm dữ liệu thành công");
                }
                catch (Exception ex)
                {
                    return Json("Thêm dữ liệu thất bại. Chi tiết: " + ex.Message);
                }
            }
            else
            {
                return Json("Không có file nào được chọn.");
            }

        }

        public int ImportDataFromExcel(string path, int id)
        {
            using (var stream = System.IO.File.Open(path, FileMode.Open, FileAccess.Read))
            {

                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    do
                    {
                        while (reader.Read())
                        {
                            // reader.GetDouble(0);
                        }
                    } while (reader.NextResult());

                    // 2. Use the AsDataSet extension method
                    DataSet result = reader.AsDataSet();
                    DataTable dt = result.Tables[0];
                    //List<string> lstTinh = db.AsEnumerable().Select(x => x["Column0"].ToString()).Distinct().ToList();
                    if (dt.Rows.Count > 2)
                    {

                        foreach (DataRow row in dt.Rows)
                        {
                            if (dt.Rows.IndexOf(row) > 1)
                            {
                                Student record = new Student();
                                //record.Id = Nid;
                                record.tong_so = row[0].ToString();
                                record.tt = row[1].ToString();
                                record.sbd = row[2].ToString();
                                record.ho_ten = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(row[3].ToString().ToLower());
                                record.ngay_sinh = row[4].ToString();
                                record.gioi_tinh = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(row[5].ToString().ToLower());
                                record.coquan_congtac =  System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(row[6].ToString().ToLower());
                                record.dantoc = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(row[7].ToString());
                                record.truong_hoc = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(row[8].ToString());
                                record.ketqua_thi = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(row[9].ToString());
                                record.ghichu = row[9 + 1].ToString();

                                record.xeploai_hocluc = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(row[11].ToString());
                                record.xeploai_hanhkiem = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(row[12 ].ToString());
                                record.xeploai_totnghiep =  System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(row[12 + 1].ToString());
                                record.dien_uudai = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(row[13 + + 1].ToString().ToLower());

                                record.create_date = DateTime.Now.ToShortDateString();
                                record.create_user = Session["Username"].ToString();
                                record.ID_Exam_Room = id;
                                record.pdf = RemoveUnicode(row[3].ToString().ToLower().Replace(" ","")) + "_" + row[2].ToString().ToLower();
                                db.Students.Add(record);
                                db.SaveChanges();



                                Grade record_ = new Grade();
                                record_.value_1 = row[10 + 4 + 1].ToString();
                                record_.value_2 = row[11 + 4 + 1].ToString();
                                record_.value_3 = row[12 + 4 + 1].ToString();
                                record_.value_4 = row[13 + 4 + 1].ToString();
                                record_.value_5 = row[14 + 4 + 1].ToString();
                                record_.value_6 = row[15 + 4 + 1].ToString();
                                record_.value_7 = row[16 + 4 + 1].ToString();
                                record_.value_8 = row[17 + 4 + 1].ToString();
                                record_.value_9 = row[18 + 4 + 1].ToString();
                                record_.value_10 = row[19 + 4 + 1].ToString();

                                record_.value_11 = row[20 + 4 + 1].ToString();
                                record_.value_12 = row[21 + 4 + 1].ToString();
                                record_.value_13 = row[22 + 4 + 1].ToString();
                                record_.value_14 = row[23 + 4 + 1].ToString();
                                record_.value_15 = row[24 + 4 + 1].ToString();
                                record_.value_16 = row[25 + 4 + 1].ToString();
                                record_.value_17 = row[26 + 4 + 1].ToString();
                                record_.value_18 = row[27 + 4 + 1].ToString();
                                record_.value_19 = row[28 + 4 + 1].ToString();
                                record_.value_20 = row[29 + 4 + 1].ToString();

                                record_.value_21 = row[30 + 4 + 1].ToString();
                                record_.value_22 = row[31 + 4 + 1].ToString();
                                record_.value_23 = row[32 + 4 + 1].ToString();
                                record_.value_24 = row[33 + 4 + 1].ToString();
                                record_.value_25 = row[34 + 4 + 1].ToString();
                                record_.value_26 = row[35 + 4 + 1].ToString();
                                record_.value_27 = row[36 + 4 + 1].ToString();
                                record_.value_28 = row[37 + 4 + 1].ToString();
                                record_.value_29 = row[38 + 4 + 1].ToString();
                                record_.value_30 = row[39 + 4 + 1].ToString();

                                record_.value_31 = row[40 + 4 + 1].ToString();
                                record_.value_32 = row[41 + 4 + 1].ToString();
                                record_.value_33 = row[42 + 4 + 1].ToString();
                                record_.value_34 = row[43 + 4 + 1].ToString();
                                record_.value_35 = row[44 + 4 + 1].ToString();
                                record_.value_36 = row[45 + 4 + 1].ToString();
                                record_.value_37 = row[46 + 4 + 1].ToString();
                                record_.value_38 = row[47 + 4 + 1].ToString();
                                record_.value_39 = row[48 + 4 + 1].ToString();
                                record_.value_40 = row[49 + 4 + 1].ToString();

                                record_.value_41 = row[50 + 4 + 1].ToString();
                                record_.value_42 = row[51 + 4 + 1].ToString();
                                record_.value_43 = row[52 + 4 + 1].ToString();
                                record_.value_44 = row[53 + 4 + 1].ToString();
                                record_.value_45 = row[54 + 4 + 1].ToString();
                                record_.value_46 = row[55 + 4 + 1].ToString();
                                record_.value_47 = row[56 + 4 + 1].ToString();
                                record_.value_48 = row[57 + 4 + 1].ToString();
                                record_.value_49 = row[58 + 4 + 1].ToString();
                                record_.value_50 = row[59 + 4 + 1].ToString();
                                record_.ID_Student = record.Id;
                                record_.create_date = DateTime.Now.ToShortDateString();
                                record_.create_user = Session["Username"].ToString();
                                db.Grades.Add(record_);
                                db.SaveChanges();

                            }
                        }
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }

                }
            }


        }

        /// <summary>
        /// De hihen thi anh len view
        /// </summary>
        /// <returns></returns>
        public FileStreamResult ByteArrayToImage(string fileName, string type, string id_Room)
        {

            string path = "";

            if (type == null)
            {
                path = HttpContext.Server.MapPath("~/PDF/" + id_Room + "/" + fileName);
            }
            else
            {
                path = HttpContext.Server.MapPath("~/PDF/" + id_Room + "/" + fileName);
            }
            if (fileName == "blank.pdf")
            {
                path = HttpContext.Server.MapPath("~/PDF/" + fileName);
            }
            else if (string.IsNullOrEmpty(fileName)|| fileName == "PDF.pdf")
            {
                    path = HttpContext.Server.MapPath("~/PDF/error.pdf");
            }
            //18_165_20191005_01_06
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            return File(fs, "application/pdf");
        }


        //id = id thí sinh
        public ActionResult Detail(int id)
        {
            
            Student a = db.Students.Where(x => x.Id == id).FirstOrDefault();
            List<GioiTinh> Gt = db.GioiTinhs.ToList();
            ViewBag.lstGioiTinh = Gt;
            List<Truong> Truong = db.Truongs.ToList();
            ViewBag.lstTruong = Truong;
            List<XepLoaiHanhKiem> XepLoaiHanhKiem = db.XepLoaiHanhKiems.ToList();
            ViewBag.lstXepLoaiHanhKiem = XepLoaiHanhKiem;
            List<XepLoaiHocLuc> XepLoaiHocLuc = db.XepLoaiHocLucs.ToList();
            ViewBag.lstXepLoaiHocLuc = XepLoaiHocLuc;
            List<XepLoaiTotNghiep> XepLoaiTotNghiep = db.XepLoaiTotNghieps.ToList();
            ViewBag.lstXepLoaiTotNghiep = XepLoaiTotNghiep;
            List<DienUuTien> DienUuTien = db.DienUuTiens.ToList();
            ViewBag.DienUuTien = DienUuTien;
            List<KetQua> KetQua = db.KetQuas.ToList();
            ViewBag.KetQua = KetQua;
            List<Student> lstStudent = db.Students.Where(x => x.ID_Exam_Room == a.ID_Exam_Room).ToList();
            ViewBag.lstStudent = lstStudent;
            ExamRoom room = db.ExamRooms.Where(x => x.Id == a.ID_Exam_Room).FirstOrDefault();
            Hoi_dong_thi hdt = db.Hoi_dong_thi.Where(x => x.Id == room.ID_Exam).FirstOrDefault();

            int ID_Exam = Int32.Parse(hdt.value_11);

            string ID_MT = db.Exams.Where(x => x.Id == ID_Exam).First().ID_MT_VALUE_NAME.ToString();
            List<MT_VALUE_NAME> lst_MT = db.MT_VALUE_NAME.Where(x => x.ID_MT_VALUE_NAME == ID_MT.ToString()).ToList();
            ViewBag.lst_MT = lst_MT;



            ViewBag.TenPhong = room.value_1;
            ViewBag.IdPhong = room.Id;

            ViewBag.TenHoiDongThi = hdt.value_1;
            ViewBag.IDHoiDongThi = hdt.Id;

            Exam Exam = db.Exams.Where(x => x.Id == ID_Exam).First();
            ViewBag.ID_Exam = ID_Exam;
            ViewBag.TenExam = Exam.khoa_thi;
            ViewBag.TenTT = a.ho_ten;
            //this.id_Room = a.ID_Exam_Room;
            //this.folder = Server.MapPath("~/PDF/" + a.ID_Exam_Room);
            //string[] filePaths = Directory.GetFiles(folder, "*.pdf").Select(Path.GetFileName).ToArray(); 

            //ViewBag.filePaths = filePaths;
            if (a.pdf == null)
            {
                ViewBag.pdf = "PDF.pdf";
            }
            else
            {
                ViewBag.pdf = a.pdf;
            }
            this.folder = Server.MapPath("~/PDF/" + room.Id);
            string[] filePaths = Directory.GetFiles(folder, "*.pdf").Select(Path.GetFileName).ToArray(); ;

            ViewBag.filePaths = filePaths;

            ViewBag.id_student = id;
            ViewBag.ID_Exam_Room = a.ID_Exam_Room;
            return View();
        }
        public ActionResult Createnew(string id)
        {
            List<GioiTinh> Gt = db.GioiTinhs.ToList();
            ViewBag.lstGioiTinh = Gt;
            List<Truong> Truong = db.Truongs.ToList();
            ViewBag.lstTruong = Truong;
            List<XepLoaiHanhKiem> XepLoaiHanhKiem = db.XepLoaiHanhKiems.ToList();
            ViewBag.lstXepLoaiHanhKiem = XepLoaiHanhKiem;
            List<XepLoaiHocLuc> XepLoaiHocLuc = db.XepLoaiHocLucs.ToList();
            ViewBag.lstXepLoaiHocLuc = XepLoaiHocLuc;
            List<XepLoaiTotNghiep> XepLoaiTotNghiep = db.XepLoaiTotNghieps.ToList();
            ViewBag.lstXepLoaiTotNghiep = XepLoaiTotNghiep;
            List<DienUuTien> DienUuTien = db.DienUuTiens.ToList();
            ViewBag.DienUuTien = DienUuTien;
            List<KetQua> KetQua = db.KetQuas.ToList();
            ViewBag.KetQua = KetQua;
            int i = Int32.Parse(id);
            //Student a = db.Students.Where(x => x.Id == i).FirstOrDefault();
            ExamRoom room = db.ExamRooms.Where(x => x.Id == i).FirstOrDefault();
            Hoi_dong_thi hdt = db.Hoi_dong_thi.Where(x => x.Id == room.ID_Exam).FirstOrDefault();

            int ID_Exam = Int32.Parse(hdt.value_11);

            string ID_MT = db.Exams.Where(x => x.Id == ID_Exam).First().ID_MT_VALUE_NAME.ToString();
            List<MT_VALUE_NAME> lst_MT = db.MT_VALUE_NAME.Where(x => x.ID_MT_VALUE_NAME == ID_MT.ToString()).ToList();
            ViewBag.lst_MT = lst_MT;



            ViewBag.TenPhong = room.value_1;
            ViewBag.IdPhong = room.Id;

            ViewBag.TenHoiDongThi = hdt.value_1;
            ViewBag.IDHoiDongThi = hdt.Id;

            Exam Exam = db.Exams.Where(x => x.Id == ID_Exam).First();
            ViewBag.ID_Exam = ID_Exam;
            ViewBag.TenExam = Exam.khoa_thi;

            this.id_Room = i;
            this.folder = Server.MapPath("~/PDF/" + i);
            string[] filePaths = Directory.GetFiles(folder, "*.pdf").Select(Path.GetFileName).ToArray(); ;

            ViewBag.filePaths = filePaths;

            ViewBag.pdf = "blank.pdf";

            ViewBag.ID_Exam_Room = i;
            return View();
        }
        public ActionResult LoadData(string id)
        {
            int id_Exam_Room = Int32.Parse(id);
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

                    // Getting all  data    
                    var listData = (from a in _context.Students
                                    where a.ID_Exam_Room == id_Exam_Room
                                    select a);

                    //Sorting    
                    if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                    {
                        listData = listData.OrderBy(sortColumn + " " + sortColumnDir);
                    }
                    //Search    
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        listData = listData.Where(m => m.ho_ten.Contains(searchValue) || m.sbd.Contains(searchValue));
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

        public JsonResult deleteJS(string id)
        {

            if (id == null)
            {
                return Json(0);
            }
            else
            {
                Student record = db.Students.Find(Int32.Parse(id));
                db.Students.Remove(record);
                db.SaveChanges();
                return Json(1);
            }
        }

        #region Lấy dữ liệu điểm thi
        public JsonResult getDiem(string id)
        {

            if (id == null)
            {
                return Json(0);
            }
            else
            {
                int Nid = Int32.Parse(id);
                Grade record = db.Grades.Where(x => x.ID_Student == Nid).FirstOrDefault();
                if (record == null)
                {
                    return Json(new
                    {
                        value_1 = "0",
                        value_2 = "0",
                        value_3 = "0",
                        value_4 = "0",
                        value_5 = "0",
                        value_6 = "0",
                        value_7 = "0",
                        value_8 = "0",
                        value_9 = "0",
                        value_10 = "0",

                        value_11 = "0",
                        value_12 = "0",
                        value_13 = "0",
                        value_14 = "0",
                        value_15 = "0",
                        value_16 = "0",
                        value_17 = "0",
                        value_18 = "0",
                        value_19 = "0",
                        value_20 = "0",

                        value_21 = "0",
                        value_22 = "0",
                        value_23 = "0",
                        value_24 = "0",
                        value_25 = "0",
                        value_26 = "0",
                        value_27 = "0",
                        value_28 = "0",
                        value_29 = "0",
                        value_30 = "0",

                        value_31 = "0",
                        value_32 = "0",
                        value_33 = "0",
                        value_34 = "0",
                        value_35 = "0",
                        value_36 = "0",
                        value_37 = "0",
                        value_38 = "0",
                        value_39 = "0",
                        value_40 = "0",

                        value_41 = "0",
                        value_42 = "0",
                        value_43 = "0",
                        value_44 = "0",
                        value_45 = "0",
                        value_46 = "0",
                        value_47 = "0",
                        value_48 = "0",
                        value_49 = "0",
                        value_50 = "0",

                    });
                }
                else
                    return Json(new
                    {
                        id = record.Id,
                        value_1 = record.value_1,
                        value_2 = record.value_2,
                        value_3 = record.value_3,
                        value_4 = record.value_4,
                        value_5 = record.value_5,
                        value_6 = record.value_6,
                        value_7 = record.value_7,
                        value_8 = record.value_8,
                        value_9 = record.value_9,
                        value_10 = record.value_10,

                        value_11 = record.value_11,
                        value_12 = record.value_12,
                        value_13 = record.value_13,
                        value_14 = record.value_14,
                        value_15 = record.value_15,
                        value_16 = record.value_16,
                        value_17 = record.value_17,
                        value_18 = record.value_18,
                        value_19 = record.value_19,
                        value_20 = record.value_20,

                        value_21 = record.value_21,
                        value_22 = record.value_22,
                        value_23 = record.value_23,
                        value_24 = record.value_24,
                        value_25 = record.value_25,
                        value_26 = record.value_26,
                        value_27 = record.value_27,
                        value_28 = record.value_28,
                        value_29 = record.value_29,
                        value_30 = record.value_30,

                        value_31 = record.value_31,
                        value_32 = record.value_32,
                        value_33 = record.value_33,
                        value_34 = record.value_34,
                        value_35 = record.value_35,
                        value_36 = record.value_36,
                        value_37 = record.value_37,
                        value_38 = record.value_38,
                        value_39 = record.value_39,
                        value_40 = record.value_40,

                        value_41 = record.value_41,
                        value_42 = record.value_42,
                        value_43 = record.value_43,
                        value_44 = record.value_44,
                        value_45 = record.value_45,
                        value_46 = record.value_46,
                        value_47 = record.value_47,
                        value_48 = record.value_48,
                        value_49 = record.value_49,
                        value_50 = record.value_50,

                    });
            }
        }
        #endregion

        #region Lấy dữ liệu điểm thi
        public JsonResult getThongTin(string id)
        {

            if (id == null)
            {
                return Json(0);
            }
            else
            {
                int Nid = Int32.Parse(id);
                Student record = db.Students.Where(x => x.Id == Nid).FirstOrDefault();
                return Json(new
                {
                    id = record.Id,
                    tt = record.tt,
                    sbd = record.sbd,
                    hoten = record.ho_ten,
                    ngaysinh = record.ngay_sinh,
                    gioitinh = record.gioi_tinh,
                    coquancongtac = record.coquan_congtac,
                    truonghoc = record.truong_hoc,
                    xeploaihanhkiem = record.xeploai_hanhkiem,
                    xeploaihocluc = record.xeploai_hocluc,
                    dienuudai = record.dien_uudai,

                    tongso = record.tong_so,
                    ketquathi = record.ketqua_thi,
                    xeploaitotnghiep = record.xeploai_totnghiep,
                    ghichu = record.ghichu,
                    dantoc = record.dantoc,
                });
            }
        }
        #endregion

        #region Lưu điểm thi
        public JsonResult saveDiem(string id, string value_1, string value_2, string value_3, string value_4, string value_5, string value_6,
    string value_7, string value_8, string value_9, string value_10, string value_11,
    string value_12, string value_13, string value_14, string value_15, string value_16, string value_17,
    string value_18, string value_19, string value_20, string value_21, string value_22,
    string value_23, string value_24, string value_25, string value_26, string value_27,
    string value_28, string value_29, string value_30, string value_31, string value_32,
    string value_33, string value_34, string value_35, string value_36, string value_37,
    string value_38, string value_39, string value_40, string value_41, string value_42,
    string value_43, string value_44, string value_45, string value_46, string value_47, string value_48, string value_49, string value_50)
        {

            if (id == null)
            {
                return Json(0);
            }
            else
            {
                int Nid = Int32.Parse(id);
                Grade record = db.Grades.Where(x => x.ID_Student == Nid).FirstOrDefault();
                //record.Id = Nid;
                if (record == null)
                {
                    Grade record_ = new Grade();
                    record_.value_1 = value_1;
                    record_.value_2 = value_2;
                    record_.value_3 = value_3;
                    record_.value_4 = value_4;
                    record_.value_5 = value_5;
                    record_.value_6 = value_6;
                    record_.value_7 = value_7;
                    record_.value_8 = value_8;
                    record_.value_9 = value_9;
                    record_.value_10 = value_10;

                    record_.value_11 = value_11;
                    record_.value_12 = value_12;
                    record_.value_13 = value_13;
                    record_.value_14 = value_14;
                    record_.value_15 = value_15;
                    record_.value_16 = value_16;
                    record_.value_17 = value_17;
                    record_.value_18 = value_18;
                    record_.value_19 = value_19;
                    record_.value_20 = value_20;

                    record_.value_21 = value_21;
                    record_.value_22 = value_22;
                    record_.value_23 = value_23;
                    record_.value_24 = value_24;
                    record_.value_25 = value_25;
                    record_.value_26 = value_26;
                    record_.value_27 = value_27;
                    record_.value_28 = value_28;
                    record_.value_29 = value_29;
                    record_.value_30 = value_30;

                    record_.value_31 = value_31;
                    record_.value_32 = value_32;
                    record_.value_33 = value_33;
                    record_.value_34 = value_34;
                    record_.value_35 = value_35;
                    record_.value_36 = value_36;
                    record_.value_37 = value_37;
                    record_.value_38 = value_38;
                    record_.value_39 = value_39;
                    record_.value_40 = value_40;

                    record_.value_41 = value_41;
                    record_.value_42 = value_42;
                    record_.value_43 = value_43;
                    record_.value_44 = value_44;
                    record_.value_45 = value_45;
                    record_.value_46 = value_46;
                    record_.value_47 = value_47;
                    record_.value_48 = value_48;
                    record_.value_49 = value_49;
                    record_.value_50 = value_50;
                    record_.ID_Student = Nid;
                    record_.create_date = DateTime.Now.ToShortDateString();
                    record_.create_user = Session["Username"].ToString();
                    db.Grades.Add(record_);
                    db.SaveChanges();
                }
                else
                {
                    record.value_1 = value_1;
                    record.value_2 = value_2;
                    record.value_3 = value_3;
                    record.value_4 = value_4;
                    record.value_5 = value_5;
                    record.value_6 = value_6;
                    record.value_7 = value_7;
                    record.value_8 = value_8;
                    record.value_9 = value_9;
                    record.value_10 = value_10;

                    record.value_11 = value_11;
                    record.value_12 = value_12;
                    record.value_13 = value_13;
                    record.value_14 = value_14;
                    record.value_15 = value_15;
                    record.value_16 = value_16;
                    record.value_17 = value_17;
                    record.value_18 = value_18;
                    record.value_19 = value_19;
                    record.value_20 = value_20;

                    record.value_21 = value_21;
                    record.value_22 = value_22;
                    record.value_23 = value_23;
                    record.value_24 = value_24;
                    record.value_25 = value_25;
                    record.value_26 = value_26;
                    record.value_27 = value_27;
                    record.value_28 = value_28;
                    record.value_29 = value_29;
                    record.value_30 = value_30;

                    record.value_31 = value_31;
                    record.value_32 = value_32;
                    record.value_33 = value_33;
                    record.value_34 = value_34;
                    record.value_35 = value_35;
                    record.value_36 = value_36;
                    record.value_37 = value_37;
                    record.value_38 = value_38;
                    record.value_39 = value_39;
                    record.value_40 = value_40;

                    record.value_41 = value_41;
                    record.value_42 = value_42;
                    record.value_43 = value_43;
                    record.value_44 = value_44;
                    record.value_45 = value_45;
                    record.value_46 = value_46;
                    record.value_47 = value_47;
                    record.value_48 = value_48;
                    record.value_49 = value_49;
                    record.value_50 = value_50;

                    db.SaveChanges();
                }

                return Json(1);
            }
        }
        #endregion

        #region Lưu thông tin
        public JsonResult saveThongTin(string id, string tt, string sbd, string hoten, string ngaysinh, string gioitinh, string coquancongtac,
    string truonghoc, string xeploaihanhkiem, string xeploaihocluc, string dienuudai, string tongso,
    string ketquathi, string xeploaitotnghiep, string ghichu, string dantoc, string pdf)
        {

            if (id == null)
            {
                return Json(0);
            }
            else
            {
                int Nid = Int32.Parse(id);
                Student record = db.Students.Where(x => x.Id == Nid).FirstOrDefault();
                //record.Id = Nid;
                record.tt = tt;
                record.sbd = sbd;
                record.ho_ten = hoten;
                record.ngay_sinh = ngaysinh;
                record.gioi_tinh = gioitinh;
                record.coquan_congtac = coquancongtac;
                record.truong_hoc = truonghoc;
                record.xeploai_hanhkiem = xeploaihanhkiem;
                record.xeploai_hocluc = xeploaihocluc;
                record.dien_uudai = dienuudai;
                record.dantoc = dantoc;
                record.tong_so = tongso;
                record.ketqua_thi = ketquathi;
                record.xeploai_totnghiep = xeploaitotnghiep;
                record.ghichu = ghichu;
                record.pdf = pdf;
                //record.pdf = pdf;


                db.SaveChanges();
                return Json(1);
            }
        }
        #endregion

        #region Lưu thông tin mới
        public JsonResult saveNewThongTin(string tt, string sbd, string hoten, string ngaysinh, string gioitinh, string coquancongtac,
    string truonghoc, string xeploaihanhkiem, string xeploaihocluc, string dienuudai, string tongso,
    string ketquathi, string xeploaitotnghiep, string ghichu, string pdf, string id_room, string dantoc)
        {
            Student record = new Student();
            //record.Id = Nid;
            record.tt = tt;
            record.sbd = sbd;
            record.ho_ten = hoten;
            record.ngay_sinh = ngaysinh;
            record.gioi_tinh = gioitinh;
            record.coquan_congtac = coquancongtac;
            record.truong_hoc = truonghoc;
            record.xeploai_hanhkiem = xeploaihanhkiem;
            record.xeploai_hocluc = xeploaihocluc;
            record.dien_uudai = dienuudai;
            record.dantoc = dantoc;
            record.tong_so = tongso;
            record.ketqua_thi = ketquathi;
            record.xeploai_totnghiep = xeploaitotnghiep;
            record.ghichu = ghichu;
            record.pdf = pdf;
            record.create_date = DateTime.Now.ToShortDateString();
            record.create_user = Session["Username"].ToString();
            record.ID_Exam_Room = Int32.Parse(id_room);
            db.Students.Add(record);
            db.SaveChanges();
            return Json(record.Id);

        }
        #endregion

        #region Lưu điểm thi
        public JsonResult saveNewDiem(string id, string value_1, string value_2, string value_3, string value_4, string value_5, string value_6,
    string value_7, string value_8, string value_9, string value_10, string value_11,
    string value_12, string value_13, string value_14, string value_15, string value_16, string value_17,
    string value_18, string value_19, string value_20, string value_21, string value_22,
    string value_23, string value_24, string value_25, string value_26, string value_27,
    string value_28, string value_29, string value_30, string value_31, string value_32,
    string value_33, string value_34, string value_35, string value_36, string value_37,
    string value_38, string value_39, string value_40, string value_41, string value_42,
    string value_43, string value_44, string value_45, string value_46, string value_47, string value_48, string value_49, string value_50)
        {

            if (id == null)
            {
                return Json(0);
            }
            else
            {
                int Nid = Int32.Parse(id);
                Grade record = db.Grades.Where(x => x.ID_Student == Nid).FirstOrDefault();
                //record.Id = Nid;
                if (record == null)
                {
                    Grade record_ = new Grade();
                    record_.value_1 = value_1;
                    record_.value_2 = value_2;
                    record_.value_3 = value_3;
                    record_.value_4 = value_4;
                    record_.value_5 = value_5;
                    record_.value_6 = value_6;
                    record_.value_7 = value_7;
                    record_.value_8 = value_8;
                    record_.value_9 = value_9;
                    record_.value_10 = value_10;

                    record_.value_11 = value_11;
                    record_.value_12 = value_12;
                    record_.value_13 = value_13;
                    record_.value_14 = value_14;
                    record_.value_15 = value_15;
                    record_.value_16 = value_16;
                    record_.value_17 = value_17;
                    record_.value_18 = value_18;
                    record_.value_19 = value_19;
                    record_.value_20 = value_20;

                    record_.value_21 = value_21;
                    record_.value_22 = value_22;
                    record_.value_23 = value_23;
                    record_.value_24 = value_24;
                    record_.value_25 = value_25;
                    record_.value_26 = value_26;
                    record_.value_27 = value_27;
                    record_.value_28 = value_28;
                    record_.value_29 = value_29;
                    record_.value_30 = value_30;

                    record_.value_31 = value_31;
                    record_.value_32 = value_32;
                    record_.value_33 = value_33;
                    record_.value_34 = value_34;
                    record_.value_35 = value_35;
                    record_.value_36 = value_36;
                    record_.value_37 = value_37;
                    record_.value_38 = value_38;
                    record_.value_39 = value_39;
                    record_.value_40 = value_40;

                    record_.value_41 = value_41;
                    record_.value_42 = value_42;
                    record_.value_43 = value_43;
                    record_.value_44 = value_44;
                    record_.value_45 = value_45;
                    record_.value_46 = value_46;
                    record_.value_47 = value_47;
                    record_.value_48 = value_48;
                    record_.value_49 = value_49;
                    record_.value_50 = value_50;
                    record_.ID_Student = Nid;
                    record_.create_date = DateTime.Now.ToShortDateString();
                    record_.create_user = Session["Username"].ToString();
                    db.Grades.Add(record_);
                    db.SaveChanges();
                }
                else
                {
                    record.value_1 = value_1;
                    record.value_2 = value_2;
                    record.value_3 = value_3;
                    record.value_4 = value_4;
                    record.value_5 = value_5;
                    record.value_6 = value_6;
                    record.value_7 = value_7;
                    record.value_8 = value_8;
                    record.value_9 = value_9;
                    record.value_10 = value_10;

                    record.value_11 = value_11;
                    record.value_12 = value_12;
                    record.value_13 = value_13;
                    record.value_14 = value_14;
                    record.value_15 = value_15;
                    record.value_16 = value_16;
                    record.value_17 = value_17;
                    record.value_18 = value_18;
                    record.value_19 = value_19;
                    record.value_20 = value_20;

                    record.value_21 = value_21;
                    record.value_22 = value_22;
                    record.value_23 = value_23;
                    record.value_24 = value_24;
                    record.value_25 = value_25;
                    record.value_26 = value_26;
                    record.value_27 = value_27;
                    record.value_28 = value_28;
                    record.value_29 = value_29;
                    record.value_30 = value_30;

                    record.value_31 = value_31;
                    record.value_32 = value_32;
                    record.value_33 = value_33;
                    record.value_34 = value_34;
                    record.value_35 = value_35;
                    record.value_36 = value_36;
                    record.value_37 = value_37;
                    record.value_38 = value_38;
                    record.value_39 = value_39;
                    record.value_40 = value_40;

                    record.value_41 = value_41;
                    record.value_42 = value_42;
                    record.value_43 = value_43;
                    record.value_44 = value_44;
                    record.value_45 = value_45;
                    record.value_46 = value_46;
                    record.value_47 = value_47;
                    record.value_48 = value_48;
                    record.value_49 = value_49;
                    record.value_50 = value_50;

                    db.SaveChanges();
                }

                return Json(1);
            }
        }
        #endregion
    }
}