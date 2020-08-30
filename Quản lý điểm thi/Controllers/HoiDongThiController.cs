using Quản_lý_điểm_thi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using System.IO;
using Quản_lý_điểm_thi.Common;

namespace Quản_lý_điểm_thi.Controllers
{
    public class HoiDongThiController : Controller
    {
        QLDTEntities1 db = new QLDTEntities1();
        // GET: HoiDongThi
        public ActionResult Index(int id)
        {

            ViewBag.id = id;
            Exam exam = db.Exams.Where(x => x.Id == id).FirstOrDefault();
            ViewBag.TenKhoaThi = exam.khoa_thi;
            return View();
        }

        public ActionResult LoadData(string id)
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
                    var listData = (from a in _context.Hoi_dong_thi
                                    where a.value_11 == id
                                    select a);

                    //Sorting    
                    if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                    {
                        listData = listData.OrderBy(sortColumn + " " + sortColumnDir);
                    }
                    //Search    
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        listData = listData.Where(m => m.value_1.Contains(searchValue)
                        || m.value_2.Contains(searchValue)
                        || m.value_3.Contains(searchValue)
                        || m.value_4.Contains(searchValue)
                        || m.value_5.Contains(searchValue)
                        || m.value_5.Contains(searchValue)
                        || m.value_6.Contains(searchValue)
                        || m.value_7.Contains(searchValue)
                        || m.value_8.Contains(searchValue)
                        || m.value_9.Contains(searchValue)
                        || m.value_10.Contains(searchValue)
                        || m.value_11.Contains(searchValue)

                        );
                    }

                    //total number of rows count     
                    recordsTotal = listData.Count();
                    //Paging     
                    var data = listData.Skip(skip).Take(pageSize).ToList();
                    //Returning Json Data    
                    return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        public JsonResult saveJS(string id, string value_1, string value_2, string value_3, string value_4,
                string value_5, string value_6, string value_7, string value_8)
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
                    Hoi_dong_thi record = db.Hoi_dong_thi.Where(x => x.Id == Nid).FirstOrDefault();
                    record.value_1 = value_1;
                    record.value_2 = value_2;
                    record.value_3 = value_3;
                    record.value_4 = value_4;
                    record.value_5 = value_5;
                    record.value_6 = value_6;
                    record.value_7 = value_7;
                    record.value_8 = value_8;
                    db.SaveChanges();
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(-2, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CreateNewJS(string id_exam, string value_1, string value_2, string value_3, string value_4,
            string value_5, string value_6, string value_7, string value_8)
        {
            if (CheckRole(UserRole.Create))
            {
                //DanhMuc_TaiKhoan record = new DanhMuc_TaiKhoan();
                Hoi_dong_thi record = new Hoi_dong_thi();
                record.value_1 = value_1;
                record.value_2 = value_2;
                record.value_3 = value_3;
                record.value_4 = value_4;
                record.value_5 = value_5;
                record.value_6 = value_6;
                record.value_7 = value_7;
                record.value_8 = value_8;
                record.value_10 = DateTime.Now.ToString();
                record.value_9 = Session["curUser"].ToString();
                record.value_11 = id_exam;
                db.Hoi_dong_thi.Add(record);
                db.SaveChanges();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(-2, JsonRequestBehavior.AllowGet);
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
                    Hoi_dong_thi record = db.Hoi_dong_thi.Find(Int32.Parse(id));
                    List<ExamRoom> lst = db.ExamRooms.Where(x => x.ID_Exam == record.Id).ToList();
                    foreach (ExamRoom x in lst)
                    {

                        List<Student> student = db.Students.Where(b => b.ID_Exam_Room == x.Id).ToList();
                        db.ExamRooms.Remove(x);
                        db.Students.RemoveRange(student);
                        db.SaveChanges();
                    }
                    db.Hoi_dong_thi.Remove(record);
                    db.SaveChanges();
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(-2, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getDataJS(string id)
        {
            if (id == null)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Hoi_dong_thi record = db.Hoi_dong_thi.Find(Int32.Parse(id));
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