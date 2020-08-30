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
    public class ExamRoomController : Controller
    {
        QLDTEntities1 db = new QLDTEntities1();
        // GET: ExamRoom
        public ActionResult Index(int id)
        {
            Hoi_dong_thi Hdt = db.Hoi_dong_thi.Where(x => x.Id == id).First();
            ViewBag.TenHoiDongThi = Hdt.value_1;
            ViewBag.IDHoiDongThi = Hdt.Id;
            string ID_Exam = db.Hoi_dong_thi.Where(x => x.Id == id).First().value_11.ToString();
            int id_Exam_int = Int32.Parse(ID_Exam);
            Exam Exam = db.Exams.Where(x => x.Id == id_Exam_int).First();
            ViewBag.ID_Exam = ID_Exam;
            ViewBag.TenExam = Exam.khoa_thi;




            string ID_PT = Exam.ID_PT_VALUE_NAME.ToString();
            List<PT_VALUE_NAME> lst_PT = db.PT_VALUE_NAME.Where(x => x.ID_PT_VALUE_NAME == ID_PT.ToString()).ToList();
            ViewBag.lst_PT = lst_PT;

            ViewBag.id = id;

            return View();
        }

        public ActionResult LoadData(string id)
        {
            int ma = Int32.Parse(id);

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
                    var listData = (from a in _context.ExamRooms
                                    where a.ID_Exam == ma
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
                        || m.value_12.Contains(searchValue)
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

        public JsonResult saveJS(string id, string value_1, string value_2, string value_3, string value_4, string value_5, string value_6,
            string value_7, string value_8, string value_9, string value_10, string value_11,
            string value_12, string value_13, string value_14, string value_15, string value_16, string value_17,
             string value_18, string value_19, string value_20, string value_21, string value_22,
              string value_23, string value_24, string value_25, string value_26, string value_27,
               string value_28, string value_29, string value_30, string value_31, string value_32,
                string value_33, string value_34, string value_35, string value_36, string value_37,
                 string value_38, string value_39, string value_40, string value_41, string value_42,
             string value_43, string value_44, string value_45, string value_46, string value_47, string value_48, string value_49, string value_50)
        {

            if (CheckRole(UserRole.Edit))
            {
                if (id == null)
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    int Nid = Int32.Parse(id);
                    ExamRoom record = db.ExamRooms.Where(x => x.Id == Nid).FirstOrDefault();
                    //record.Id = Nid;
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
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(-2, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CreateNewJS(string id_exam, string value_1, string value_2, string value_3, string value_4, string value_5, string value_6,
            string value_7, string value_8, string value_9, string value_10, string value_11,
            string value_12, string value_13, string value_14, string value_15, string value_16, string value_17,
             string value_18, string value_19, string value_20, string value_21, string value_22,
              string value_23, string value_24, string value_25, string value_26, string value_27,
               string value_28, string value_29, string value_30, string value_31, string value_32,
                string value_33, string value_34, string value_35, string value_36, string value_37,
                 string value_38, string value_39, string value_40, string value_41, string value_42,
             string value_43, string value_44, string value_45, string value_46, string value_47, string value_48, string value_49, string value_50)
        {
            if (CheckRole(UserRole.Create))
            {
                ExamRoom record = new ExamRoom();
                record.ID_Exam = Int32.Parse(id_exam);
                record.create_date = DateTime.Now.ToString();
                record.create_user = Session["curUser"].ToString();
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
                db.ExamRooms.Add(record);
                db.SaveChanges();
                Directory.CreateDirectory(Server.MapPath("~/PDF/" + record.Id));
                Directory.CreateDirectory(Server.MapPath("~/Excel/" + record.Id));
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(-2, JsonRequestBehavior.AllowGet);
        }


        public JsonResult deleteJS(string id)
        {
            if (CheckRole(UserRole.Delete))
            {
                int Niid = Int32.Parse(id);
                if (id == null)
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ExamRoom record = db.ExamRooms.Find(Niid);
                    db.ExamRooms.Remove(record);
                    List<Student> student = db.Students.Where(x => x.ID_Exam_Room == Niid).ToList();
                    db.Students.RemoveRange(student);
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
                ExamRoom record = db.ExamRooms.Find(Int32.Parse(id));
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