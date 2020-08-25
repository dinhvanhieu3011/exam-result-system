using Quản_lý_điểm_thi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace Quản_lý_điểm_thi.Controllers
{
    public class SettingsController : Controller
    {
        QLDTEntities1 db = new QLDTEntities1();
        // GET: Settings
        public ActionResult Index()
        {
            List<string> lst_MT = db.MT_VALUE_NAME.Select(d => d.ID_MT_VALUE_NAME).Distinct().ToList();
            ViewBag.lst_MT = lst_MT;
            List<string> lst_PT = db.PT_VALUE_NAME.Select(d => d.ID_PT_VALUE_NAME).Distinct().ToList();
            ViewBag.lst_PT = lst_PT;
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
                    var listData = (from a in _context.Settings
                                    select a);

                    //Sorting    
                    if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                    {
                        listData = listData.OrderBy(sortColumn + " " + sortColumnDir);
                    }
                    //Search    
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        listData = listData.Where(m => m.Loai.Contains(searchValue) || m.Ten.Contains(searchValue));
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

        public ActionResult Edit(string id)
        {

            int i = Int32.Parse(id);
            Setting setting = db.Settings.Find(i);
            ViewBag.setting = setting;
            string loai = setting.Loai;
            ViewBag.loai = loai;
            List<MT_VALUE_NAME> listMT = db.MT_VALUE_NAME.Where(x => x.ID_MT_VALUE_NAME == id).ToList();
            ViewBag.listMT = listMT;

            List<PT_VALUE_NAME> listPT = db.PT_VALUE_NAME.Where(x => x.ID_PT_VALUE_NAME == id).ToList();
            ViewBag.listPT = listPT;


            return View();
        }
        public ActionResult Detail(string id)
        {

            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
        public JsonResult deleteJS(string id)
        {

            if (id == null)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            else
            {

                Setting record = db.Settings.Find(Int32.Parse(id));
                string loai = record.Loai;

                if (loai == "MT")
                {
                    int a1 = Int32.Parse(id);
                    Exam e = db.Exams.Where(x => x.ID_MT_VALUE_NAME == a1).FirstOrDefault();
                    if (e != null)
                    {
                        return Json(-1, JsonRequestBehavior.AllowGet);
                    }
                    List<MT_VALUE_NAME> a = db.MT_VALUE_NAME.Where(x => x.ID_MT_VALUE_NAME == id).ToList();
                    foreach (MT_VALUE_NAME b in a)
                    {
                        db.MT_VALUE_NAME.Remove(b);
                        db.SaveChanges();

                    }
                    db.Settings.Remove(record);
                    db.SaveChanges();
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
                else

                {
                    int a1 = Int32.Parse(id);
                    Exam e = db.Exams.Where(x => x.ID_PT_VALUE_NAME == a1).FirstOrDefault();
                    if (e != null)
                    {
                        return Json(-1, JsonRequestBehavior.AllowGet);
                    }
                    List<PT_VALUE_NAME> a = db.PT_VALUE_NAME.Where(x => x.ID_PT_VALUE_NAME == id).ToList();
                    foreach (PT_VALUE_NAME b in a)
                    {
                        db.PT_VALUE_NAME.Remove(b);
                        db.SaveChanges();

                    }
                    db.Settings.Remove(record);
                    db.SaveChanges();
                    return Json(1, JsonRequestBehavior.AllowGet);
                }


            }
        }
        public JsonResult saveJS(string Id, string Loai, string Ten, string status1, string name1, string type1,
            string status2, string name2, string type2,
            string status3, string name3, string type3,
            string status4, string name4, string type4,
            string status5, string name5, string type5,
            string status6, string name6, string type6,
            string status7, string name7, string type7,
            string status8, string name8, string type8,
            string status9, string name9, string type9,
            string status10, string name10, string type10,
            string status11, string name11, string type11,
            string status12, string name12, string type12,
                        string status13, string name13, string type13,
            string status14, string name14, string type14,
            string status15, string name15, string type15,
            string status16, string name16, string type16,
            string status17, string name17, string type17,
            string status18, string name18, string type18,

            string status19, string name19, string type19,
            string status20, string name20, string type20,
            string status21, string name21, string type21,
            string status22, string name22, string type22,

            string status23, string name23, string type23,
            string status24, string name24, string type24,
            string status25, string name25, string type25,
            string status26, string name26, string type26,
            string status27, string name27, string type27,
            string status28, string name28, string type28,
            string status29, string name29, string type29,
            string status30, string name30, string type30,
            string status31, string name31, string type31,
            string status32, string name32, string type32,

            string status33, string name33, string type33,
            string status34, string name34, string type34,
            string status35, string name35, string type35,
            string status36, string name36, string type36,
            string status37, string name37, string type37,
            string status38, string name38, string type38,
            string status39, string name39, string type39,
            string status40, string name40, string type40,
            string status41, string name41, string type41,
            string status42, string name42, string type42,
                        string status43, string name43, string type43,
            string status44, string name44, string type44,
            string status45, string name45, string type45,
            string status46, string name46, string type46,
            string status47, string name47, string type47,
            string status48, string name48, string type48,
            string status49, string name49, string type49,
            string status50, string name50, string type50
            )
        {
            if (!string.IsNullOrEmpty(Id))
            {
                int id = Int32.Parse(Id);
                Setting st = db.Settings.Find(id);
                string OLDLOAI = st.Loai;
                st.Loai = Loai;
                st.Ten = Ten;
                db.SaveChanges();
                if (OLDLOAI == "MT")
                {
                    List<MT_VALUE_NAME> a = db.MT_VALUE_NAME.Where(x => x.ID_MT_VALUE_NAME == Id).ToList();
                    foreach (MT_VALUE_NAME b in a)
                    {
                        db.MT_VALUE_NAME.Remove(b);
                        db.SaveChanges();
                    }
                }
                else

                {
                    List<PT_VALUE_NAME> a = db.PT_VALUE_NAME.Where(x => x.ID_PT_VALUE_NAME == Id).ToList();
                    foreach (PT_VALUE_NAME b in a)
                    {
                        db.PT_VALUE_NAME.Remove(b);
                        db.SaveChanges();
                    }
                }
                if (Loai == "MT")
                {
                    MT_VALUE_NAME mt = new MT_VALUE_NAME();
                    #region Save
                    mt.name = "value_1";
                    mt.mo_ta = name1;
                    mt.type = type1;
                    mt.status = (status1 != "true") ? "hidden" : "";
                    mt.ID_MT_VALUE_NAME = st.Id.ToString();
                    db.MT_VALUE_NAME.Add(mt);
                    db.SaveChanges();
                    //-------------
                    mt.name = "value_2";
                    mt.mo_ta = name2;
                    mt.type = type2;
                    mt.status = (status2 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_3";
                    mt.mo_ta = name3;
                    mt.type = type3;
                    mt.status = (status3 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_4";
                    mt.mo_ta = name4;
                    mt.type = type4;
                    mt.status = (status4 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_5";
                    mt.mo_ta = name5;
                    mt.type = type5;
                    mt.status = (status5 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_6";
                    mt.mo_ta = name6;
                    mt.type = type6;
                    mt.status = (status6 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_7";
                    mt.mo_ta = name7;
                    mt.type = type7;
                    mt.status = (status7 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_8";
                    mt.mo_ta = name8;
                    mt.type = type8;
                    mt.status = (status8 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_9";
                    mt.mo_ta = name9;
                    mt.type = type9;
                    mt.status = (status9 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    mt.name = "value_10";
                    mt.mo_ta = name10;
                    mt.type = type10;
                    mt.status = (status10 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    mt.name = "value_11";
                    mt.mo_ta = name11;
                    mt.type = type11;
                    mt.status = (status11 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_12";
                    mt.mo_ta = name12;
                    mt.type = type12;
                    mt.status = (status12 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_13";
                    mt.mo_ta = name13;
                    mt.type = type13;
                    mt.status = (status13 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_14";
                    mt.mo_ta = name14;
                    mt.type = type14;
                    mt.status = (status14 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_15";
                    mt.mo_ta = name15;
                    mt.type = type15;
                    mt.status = (status15 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_16";
                    mt.mo_ta = name16;
                    mt.type = type16;
                    mt.status = (status16 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_17";
                    mt.mo_ta = name17;
                    mt.type = type17;
                    mt.status = (status17 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_18";
                    mt.mo_ta = name18;
                    mt.type = type18;
                    mt.status = (status18 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_19";
                    mt.mo_ta = name19;
                    mt.type = type19;
                    mt.status = (status19 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_20";
                    mt.mo_ta = name20;
                    mt.type = type20;
                    mt.status = (status20 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_21";
                    mt.mo_ta = name21;
                    mt.type = type21;
                    mt.status = (status21 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_22";
                    mt.mo_ta = name22;
                    mt.type = type22;
                    mt.status = (status22 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_23";
                    mt.mo_ta = name23;
                    mt.type = type23;
                    mt.status = (status23 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_24";
                    mt.mo_ta = name24;
                    mt.type = type24;
                    mt.status = (status24 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_25";
                    mt.mo_ta = name25;
                    mt.type = type25;
                    mt.status = (status25 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_26";
                    mt.mo_ta = name26;
                    mt.type = type26;
                    mt.status = (status26 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_27";
                    mt.mo_ta = name27;
                    mt.type = type27;
                    mt.status = (status27 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_28";
                    mt.mo_ta = name28;
                    mt.type = type28;
                    mt.status = (status28 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_29";
                    mt.mo_ta = name29;
                    mt.type = type29;
                    mt.status = (status29 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_30";
                    mt.mo_ta = name30;
                    mt.type = type30;
                    mt.status = (status30 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_31";
                    mt.mo_ta = name31;
                    mt.type = type31;
                    mt.status = (status31 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_32";
                    mt.mo_ta = name32;
                    mt.type = type32;
                    mt.status = (status32 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_33";
                    mt.mo_ta = name33;
                    mt.type = type33;
                    mt.status = (status33 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_34";
                    mt.mo_ta = name34;
                    mt.type = type34;
                    mt.status = (status34 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_35";
                    mt.mo_ta = name35;
                    mt.type = type35;
                    mt.status = (status35 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_36";
                    mt.mo_ta = name36;
                    mt.type = type36;
                    mt.status = (status36 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_37";
                    mt.mo_ta = name37;
                    mt.type = type37;
                    mt.status = (status37 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_38";
                    mt.mo_ta = name38;
                    mt.type = type38;
                    mt.status = (status38 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_39";
                    mt.mo_ta = name39;
                    mt.type = type39;
                    mt.status = (status39 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_40";
                    mt.mo_ta = name40;
                    mt.type = type40;
                    mt.status = (status40 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_41";
                    mt.mo_ta = name41;
                    mt.type = type41;
                    mt.status = (status41 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_42";
                    mt.mo_ta = name42;
                    mt.type = type42;
                    mt.status = (status42 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_43";
                    mt.mo_ta = name43;
                    mt.type = type43;
                    mt.status = (status43 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_44";
                    mt.mo_ta = name44;
                    mt.type = type44;
                    mt.status = (status44 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_45";
                    mt.mo_ta = name45;
                    mt.type = type45;
                    mt.status = (status45 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_46";
                    mt.mo_ta = name46;
                    mt.type = type46;
                    mt.status = (status46 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_47";
                    mt.mo_ta = name47;
                    mt.type = type47;
                    mt.status = (status47 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_48";
                    mt.mo_ta = name48;
                    mt.type = type48;
                    mt.status = (status48 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_49";
                    mt.mo_ta = name49;
                    mt.type = type49;
                    mt.status = (status49 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_50";
                    mt.mo_ta = name50;
                    mt.type = type50;
                    mt.status = (status50 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    #endregion
                }
                else
                {

                    PT_VALUE_NAME mt = new PT_VALUE_NAME();
                    #region Save
                    mt.name = "value_1";
                    mt.mo_ta = name1;
                    mt.type = type1;
                    mt.status = (status1 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_2";
                    mt.mo_ta = name2;
                    mt.type = type2;
                    mt.status = (status2 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_3";
                    mt.mo_ta = name3;
                    mt.type = type3;
                    mt.status = (status3 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_4";
                    mt.mo_ta = name4;
                    mt.type = type4;
                    mt.status = (status4 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_5";
                    mt.mo_ta = name5;
                    mt.type = type5;
                    mt.status = (status5 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_6";
                    mt.mo_ta = name6;
                    mt.type = type6;
                    mt.status = (status6 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_7";
                    mt.mo_ta = name7;
                    mt.type = type7;
                    mt.status = (status7 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_8";
                    mt.mo_ta = name8;
                    mt.type = type8;
                    mt.status = (status8 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_9";
                    mt.mo_ta = name9;
                    mt.type = type9;
                    mt.status = (status9 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    mt.name = "value_10";
                    mt.mo_ta = name10;
                    mt.type = type10;
                    mt.status = (status10 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    mt.name = "value_11";
                    mt.mo_ta = name11;
                    mt.type = type11;
                    mt.status = (status11 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_12";
                    mt.mo_ta = name12;
                    mt.type = type12;
                    mt.status = (status12 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_13";
                    mt.mo_ta = name13;
                    mt.type = type13;
                    mt.status = (status13 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_14";
                    mt.mo_ta = name14;
                    mt.type = type14;
                    mt.status = (status14 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_15";
                    mt.mo_ta = name15;
                    mt.type = type15;
                    mt.status = (status15 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_16";
                    mt.mo_ta = name16;
                    mt.type = type16;
                    mt.status = (status16 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_17";
                    mt.mo_ta = name17;
                    mt.type = type17;
                    mt.status = (status17 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_18";
                    mt.mo_ta = name18;
                    mt.type = type18;
                    mt.status = (status18 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_19";
                    mt.mo_ta = name19;
                    mt.type = type19;
                    mt.status = (status19 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_20";
                    mt.mo_ta = name20;
                    mt.type = type20;
                    mt.status = (status20 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_21";
                    mt.mo_ta = name21;
                    mt.type = type21;
                    mt.status = (status21 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_22";
                    mt.mo_ta = name22;
                    mt.type = type22;
                    mt.status = (status22 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_23";
                    mt.mo_ta = name23;
                    mt.type = type23;
                    mt.status = (status23 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_24";
                    mt.mo_ta = name24;
                    mt.type = type24;
                    mt.status = (status24 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_25";
                    mt.mo_ta = name25;
                    mt.type = type25;
                    mt.status = (status25 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_26";
                    mt.mo_ta = name26;
                    mt.type = type26;
                    mt.status = (status26 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_27";
                    mt.mo_ta = name27;
                    mt.type = type27;
                    mt.status = (status27 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_28";
                    mt.mo_ta = name28;
                    mt.type = type28;
                    mt.status = (status28 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_29";
                    mt.mo_ta = name29;
                    mt.type = type29;
                    mt.status = (status29 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_30";
                    mt.mo_ta = name30;
                    mt.type = type30;
                    mt.status = (status30 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_31";
                    mt.mo_ta = name31;
                    mt.type = type31;
                    mt.status = (status31 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_32";
                    mt.mo_ta = name32;
                    mt.type = type32;
                    mt.status = (status32 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_33";
                    mt.mo_ta = name33;
                    mt.type = type33;
                    mt.status = (status33 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_34";
                    mt.mo_ta = name34;
                    mt.type = type34;
                    mt.status = (status34 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_35";
                    mt.mo_ta = name35;
                    mt.type = type35;
                    mt.status = (status35 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_36";
                    mt.mo_ta = name36;
                    mt.type = type36;
                    mt.status = (status36 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_37";
                    mt.mo_ta = name37;
                    mt.type = type37;
                    mt.status = (status37 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_38";
                    mt.mo_ta = name38;
                    mt.type = type38;
                    mt.status = (status38 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_39";
                    mt.mo_ta = name39;
                    mt.type = type39;
                    mt.status = (status39 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_40";
                    mt.mo_ta = name40;
                    mt.type = type40;
                    mt.status = (status40 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_41";
                    mt.mo_ta = name41;
                    mt.type = type41;
                    mt.status = (status41 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_42";
                    mt.mo_ta = name42;
                    mt.type = type42;
                    mt.status = (status42 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_43";
                    mt.mo_ta = name43;
                    mt.type = type43;
                    mt.status = (status43 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_44";
                    mt.mo_ta = name44;
                    mt.type = type44;
                    mt.status = (status44 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_45";
                    mt.mo_ta = name45;
                    mt.type = type45;
                    mt.status = (status45 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_46";
                    mt.mo_ta = name46;
                    mt.type = type46;
                    mt.status = (status46 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_47";
                    mt.mo_ta = name47;
                    mt.type = type47;
                    mt.status = (status47 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_48";
                    mt.mo_ta = name48;
                    mt.type = type48;
                    mt.status = (status48 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_49";
                    mt.mo_ta = name49;
                    mt.type = type49;
                    mt.status = (status49 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_50";
                    mt.mo_ta = name50;
                    mt.type = type50;
                    mt.status = (status50 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    #endregion

                    return Json(1, JsonRequestBehavior.AllowGet);
                }


            }
            else
            {
                Setting st = new Setting();
                st.Loai = Loai;
                st.Ten = Ten;
                st.create_user = "";
                st.create_date = DateTime.Now.ToShortDateString();
                db.Settings.Add(st);
                db.SaveChanges();
                if (Loai == "MT")
                {
                    MT_VALUE_NAME mt = new MT_VALUE_NAME();
                    #region Save
                    mt.name = "value_1";
                    mt.mo_ta = name1;
                    mt.type = type1;
                    mt.status = (status1 != "true") ? "hidden" : "";
                    mt.ID_MT_VALUE_NAME = st.Id.ToString();
                    db.MT_VALUE_NAME.Add(mt);
                    db.SaveChanges();
                    //-------------
                    mt.name = "value_2";
                    mt.mo_ta = name2;
                    mt.type = type2;
                    mt.status = (status2 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_3";
                    mt.mo_ta = name3;
                    mt.type = type3;
                    mt.status = (status3 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_4";
                    mt.mo_ta = name4;
                    mt.type = type4;
                    mt.status = (status4 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_5";
                    mt.mo_ta = name5;
                    mt.type = type5;
                    mt.status = (status5 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_6";
                    mt.mo_ta = name6;
                    mt.type = type6;
                    mt.status = (status6 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_7";
                    mt.mo_ta = name7;
                    mt.type = type7;
                    mt.status = (status7 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_8";
                    mt.mo_ta = name8;
                    mt.type = type8;
                    mt.status = (status8 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_9";
                    mt.mo_ta = name9;
                    mt.type = type9;
                    mt.status = (status9 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    mt.name = "value_10";
                    mt.mo_ta = name10;
                    mt.type = type10;
                    mt.status = (status10 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    mt.name = "value_11";
                    mt.mo_ta = name11;
                    mt.type = type11;
                    mt.status = (status11 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_12";
                    mt.mo_ta = name12;
                    mt.type = type12;
                    mt.status = (status12 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_13";
                    mt.mo_ta = name13;
                    mt.type = type13;
                    mt.status = (status13 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_14";
                    mt.mo_ta = name14;
                    mt.type = type14;
                    mt.status = (status14 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_15";
                    mt.mo_ta = name15;
                    mt.type = type15;
                    mt.status = (status15 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_16";
                    mt.mo_ta = name16;
                    mt.type = type16;
                    mt.status = (status16 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_17";
                    mt.mo_ta = name17;
                    mt.type = type17;
                    mt.status = (status17 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_18";
                    mt.mo_ta = name18;
                    mt.type = type18;
                    mt.status = (status18 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_19";
                    mt.mo_ta = name19;
                    mt.type = type19;
                    mt.status = (status19 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_20";
                    mt.mo_ta = name20;
                    mt.type = type20;
                    mt.status = (status20 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_21";
                    mt.mo_ta = name21;
                    mt.type = type21;
                    mt.status = (status21 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_22";
                    mt.mo_ta = name22;
                    mt.type = type22;
                    mt.status = (status22 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_23";
                    mt.mo_ta = name23;
                    mt.type = type23;
                    mt.status = (status23 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_24";
                    mt.mo_ta = name24;
                    mt.type = type24;
                    mt.status = (status24 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_25";
                    mt.mo_ta = name25;
                    mt.type = type25;
                    mt.status = (status25 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_26";
                    mt.mo_ta = name26;
                    mt.type = type26;
                    mt.status = (status26 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_27";
                    mt.mo_ta = name27;
                    mt.type = type27;
                    mt.status = (status27 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_28";
                    mt.mo_ta = name28;
                    mt.type = type28;
                    mt.status = (status28 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_29";
                    mt.mo_ta = name29;
                    mt.type = type29;
                    mt.status = (status29 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_30";
                    mt.mo_ta = name30;
                    mt.type = type30;
                    mt.status = (status30 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_31";
                    mt.mo_ta = name31;
                    mt.type = type31;
                    mt.status = (status31 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_32";
                    mt.mo_ta = name32;
                    mt.type = type32;
                    mt.status = (status32 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_33";
                    mt.mo_ta = name33;
                    mt.type = type33;
                    mt.status = (status33 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_34";
                    mt.mo_ta = name34;
                    mt.type = type34;
                    mt.status = (status34 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_35";
                    mt.mo_ta = name35;
                    mt.type = type35;
                    mt.status = (status35 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_36";
                    mt.mo_ta = name36;
                    mt.type = type36;
                    mt.status = (status36 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_37";
                    mt.mo_ta = name37;
                    mt.type = type37;
                    mt.status = (status37 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_38";
                    mt.mo_ta = name38;
                    mt.type = type38;
                    mt.status = (status38 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_39";
                    mt.mo_ta = name39;
                    mt.type = type39;
                    mt.status = (status39 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_40";
                    mt.mo_ta = name40;
                    mt.type = type40;
                    mt.status = (status40 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_41";
                    mt.mo_ta = name41;
                    mt.type = type41;
                    mt.status = (status41 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_42";
                    mt.mo_ta = name42;
                    mt.type = type42;
                    mt.status = (status42 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_43";
                    mt.mo_ta = name43;
                    mt.type = type43;
                    mt.status = (status43 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_44";
                    mt.mo_ta = name44;
                    mt.type = type44;
                    mt.status = (status44 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_45";
                    mt.mo_ta = name45;
                    mt.type = type45;
                    mt.status = (status45 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_46";
                    mt.mo_ta = name46;
                    mt.type = type46;
                    mt.status = (status46 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_47";
                    mt.mo_ta = name47;
                    mt.type = type47;
                    mt.status = (status47 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_48";
                    mt.mo_ta = name48;
                    mt.type = type48;
                    mt.status = (status48 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_49";
                    mt.mo_ta = name49;
                    mt.type = type49;
                    mt.status = (status49 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_50";
                    mt.mo_ta = name50;
                    mt.type = type50;
                    mt.status = (status50 != "true") ? "hidden" : ""; mt.ID_MT_VALUE_NAME = st.Id.ToString(); db.MT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    #endregion
                }
                else
                {

                    PT_VALUE_NAME mt = new PT_VALUE_NAME();
                    #region Save
                    mt.name = "value_1";
                    mt.mo_ta = name1;
                    mt.type = type1;
                    mt.status = (status1 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_2";
                    mt.mo_ta = name2;
                    mt.type = type2;
                    mt.status = (status2 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_3";
                    mt.mo_ta = name3;
                    mt.type = type3;
                    mt.status = (status3 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_4";
                    mt.mo_ta = name4;
                    mt.type = type4;
                    mt.status = (status4 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_5";
                    mt.mo_ta = name5;
                    mt.type = type5;
                    mt.status = (status5 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_6";
                    mt.mo_ta = name6;
                    mt.type = type6;
                    mt.status = (status6 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_7";
                    mt.mo_ta = name7;
                    mt.type = type7;
                    mt.status = (status7 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_8";
                    mt.mo_ta = name8;
                    mt.type = type8;
                    mt.status = (status8 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_9";
                    mt.mo_ta = name9;
                    mt.type = type9;
                    mt.status = (status9 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    mt.name = "value_10";
                    mt.mo_ta = name10;
                    mt.type = type10;
                    mt.status = (status10 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    mt.name = "value_11";
                    mt.mo_ta = name11;
                    mt.type = type11;
                    mt.status = (status11 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_12";
                    mt.mo_ta = name12;
                    mt.type = type12;
                    mt.status = (status12 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_13";
                    mt.mo_ta = name13;
                    mt.type = type13;
                    mt.status = (status13 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_14";
                    mt.mo_ta = name14;
                    mt.type = type14;
                    mt.status = (status14 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_15";
                    mt.mo_ta = name15;
                    mt.type = type15;
                    mt.status = (status15 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_16";
                    mt.mo_ta = name16;
                    mt.type = type16;
                    mt.status = (status16 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_17";
                    mt.mo_ta = name17;
                    mt.type = type17;
                    mt.status = (status17 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_18";
                    mt.mo_ta = name18;
                    mt.type = type18;
                    mt.status = (status18 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_19";
                    mt.mo_ta = name19;
                    mt.type = type19;
                    mt.status = (status19 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_20";
                    mt.mo_ta = name20;
                    mt.type = type20;
                    mt.status = (status20 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_21";
                    mt.mo_ta = name21;
                    mt.type = type21;
                    mt.status = (status21 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_22";
                    mt.mo_ta = name22;
                    mt.type = type22;
                    mt.status = (status22 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_23";
                    mt.mo_ta = name23;
                    mt.type = type23;
                    mt.status = (status23 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_24";
                    mt.mo_ta = name24;
                    mt.type = type24;
                    mt.status = (status24 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_25";
                    mt.mo_ta = name25;
                    mt.type = type25;
                    mt.status = (status25 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_26";
                    mt.mo_ta = name26;
                    mt.type = type26;
                    mt.status = (status26 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_27";
                    mt.mo_ta = name27;
                    mt.type = type27;
                    mt.status = (status27 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_28";
                    mt.mo_ta = name28;
                    mt.type = type28;
                    mt.status = (status28 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_29";
                    mt.mo_ta = name29;
                    mt.type = type29;
                    mt.status = (status29 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_30";
                    mt.mo_ta = name30;
                    mt.type = type30;
                    mt.status = (status30 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_31";
                    mt.mo_ta = name31;
                    mt.type = type31;
                    mt.status = (status31 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_32";
                    mt.mo_ta = name32;
                    mt.type = type32;
                    mt.status = (status32 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_33";
                    mt.mo_ta = name33;
                    mt.type = type33;
                    mt.status = (status33 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_34";
                    mt.mo_ta = name34;
                    mt.type = type34;
                    mt.status = (status34 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_35";
                    mt.mo_ta = name35;
                    mt.type = type35;
                    mt.status = (status35 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_36";
                    mt.mo_ta = name36;
                    mt.type = type36;
                    mt.status = (status36 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_37";
                    mt.mo_ta = name37;
                    mt.type = type37;
                    mt.status = (status37 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_38";
                    mt.mo_ta = name38;
                    mt.type = type38;
                    mt.status = (status38 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_39";
                    mt.mo_ta = name39;
                    mt.type = type39;
                    mt.status = (status39 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_40";
                    mt.mo_ta = name40;
                    mt.type = type40;
                    mt.status = (status40 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_41";
                    mt.mo_ta = name41;
                    mt.type = type41;
                    mt.status = (status41 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_42";
                    mt.mo_ta = name42;
                    mt.type = type42;
                    mt.status = (status42 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_43";
                    mt.mo_ta = name43;
                    mt.type = type43;
                    mt.status = (status43 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_44";
                    mt.mo_ta = name44;
                    mt.type = type44;
                    mt.status = (status44 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_45";
                    mt.mo_ta = name45;
                    mt.type = type45;
                    mt.status = (status45 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_46";
                    mt.mo_ta = name46;
                    mt.type = type46;
                    mt.status = (status46 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_47";
                    mt.mo_ta = name47;
                    mt.type = type47;
                    mt.status = (status47 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_48";
                    mt.mo_ta = name48;
                    mt.type = type48;
                    mt.status = (status48 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_49";
                    mt.mo_ta = name49;
                    mt.type = type49;
                    mt.status = (status49 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    mt.name = "value_50";
                    mt.mo_ta = name50;
                    mt.type = type50;
                    mt.status = (status50 != "true") ? "hidden" : ""; mt.ID_PT_VALUE_NAME = st.Id.ToString(); db.PT_VALUE_NAME.Add(mt); db.SaveChanges();
                    //-------------
                    #endregion

                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(1, JsonRequestBehavior.AllowGet);
        }
    }
}