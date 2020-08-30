using Quản_lý_điểm_thi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using Quản_lý_điểm_thi.Common;

namespace Quản_lý_điểm_thi.Controllers
{
    public class ExamController : Controller
    {
        QLDTEntities1 db = new QLDTEntities1();
        // GET: Exam
        public ActionResult Index()
        {
            List<Setting> lst_MT_1 = db.Settings.Where(d => d.Loai == "MT").ToList();
            ViewBag.lst_MT_1 = lst_MT_1;
            List<Setting> lst_PT_1 = db.Settings.Where(d => d.Loai == "PT").ToList();
            ViewBag.lst_PT_1 = lst_PT_1;
            return View();
        }

        public ActionResult LoadData()
        {
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
                    var listData = (from a in _context.Exams
                                    select a);

                    //Sorting    
                    if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                    {
                        listData = listData.OrderBy(sortColumn + " " + sortColumnDir);
                    }
                    //Search    
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        listData = listData.Where(m => m.khoa_thi.Contains(searchValue) || m.hoi_dong_thi.Contains(searchValue));
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
        public JsonResult saveJS(string id, string hoi_dong_thi, string khoa_thi, string nam_thi, string value_1, string value_2)
        {
            if (CheckRole(UserRole.Create))
            {
                if (id == null)
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    int Nid = Int32.Parse(id);
                    Exam record = db.Exams.Where(x => x.Id == Nid).FirstOrDefault();
                    record.hoi_dong_thi = hoi_dong_thi;
                    record.khoa_thi = khoa_thi;
                    record.nam_thi = nam_thi;
                    record.value_1 = value_1;
                    record.value_2 = value_2;
                    db.SaveChanges();
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(-2, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult CreateNewJS(string hoi_dong_thi, string khoa_thi, string nam_thi, string ID_PT, string ID_MT, string value_1, string value_2)
        {
            if (CheckRole(UserRole.Create))
            {
                //DanhMuc_TaiKhoan record = new DanhMuc_TaiKhoan();
                Exam record = new Exam();
                record.hoi_dong_thi = hoi_dong_thi;
                record.khoa_thi = khoa_thi;
                record.nam_thi = nam_thi;
                record.create_date = DateTime.Now.ToString();
                record.create_user = Session["curUser"].ToString();
                record.ID_MT_VALUE_NAME = Int32.Parse(ID_MT);
                record.ID_PT_VALUE_NAME = Int32.Parse(ID_PT);
                record.value_1 = value_1;
                record.value_2 = value_2;
                db.Exams.Add(record);
                db.SaveChanges();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(-2, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult deleteJS(string id)
        {
            if (CheckRole(UserRole.Delete))
            {
                if (id == null)
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Exam record = db.Exams.Find(Int32.Parse(id));
                    List<Hoi_dong_thi> lstHDT = db.Hoi_dong_thi.Where(x => x.value_11 == record.Id.ToString()).ToList();
                    foreach (Hoi_dong_thi recordHDT in lstHDT)
                    {
                        List<ExamRoom> lst = db.ExamRooms.Where(x => x.ID_Exam == recordHDT.Id).ToList();
                        foreach (ExamRoom x in lst)
                        {

                            List<Student> student = db.Students.Where(b => b.ID_Exam_Room == x.Id).ToList();
                            db.ExamRooms.Remove(x);
                            db.Students.RemoveRange(student);
                            db.SaveChanges();
                        }
                        db.Hoi_dong_thi.Remove(recordHDT);
                    }
                    db.Exams.Remove(record);
                    db.SaveChanges();
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(-2, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult getDataJS(string id)
        {
            if (id == null)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Exam record = db.Exams.Find(Int32.Parse(id));
                return Json(new
                {
                    id = record.Id,
                    hoi_dong_thi = record.hoi_dong_thi,
                    khoa_thi = record.khoa_thi,
                    nam_thi = record.nam_thi,
                    ID_PT = record.ID_PT_VALUE_NAME,
                    ID_MT = record.ID_MT_VALUE_NAME,
                    value_1 = record.value_1,
                    value_2 = record.value_2,
                }, JsonRequestBehavior.AllowGet);
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