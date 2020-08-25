using Quản_lý_điểm_thi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace Quản_lý_điểm_thi.Controllers
{
    public class XepLoaiTotNghiepController : Controller
    {
        QLDTEntities1 db = new QLDTEntities1();
        // GET: XepLoaiTotNghiep
        public ActionResult Index()
        {
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
                    var listData = (from a in _context.XepLoaiTotNghieps
                                    select a);

                    //Sorting    
                    if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                    {
                        listData = listData.OrderBy(sortColumn + " " + sortColumnDir);
                    }
                    //Search    
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        listData = listData.Where(m => m.Ten.Contains(searchValue) || m.Mo_Ta.Contains(searchValue));
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
        public JsonResult saveJS(string id, string Ten, string Mo_Ta, string Ghi_chu)
        {

            if (id == null)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            else
            {
                int Nid = Int32.Parse(id);
                XepLoaiTotNghiep record = db.XepLoaiTotNghieps.Where(x => x.Id == Nid).FirstOrDefault();
                record.Ten = Ten;
                record.Mo_Ta = Mo_Ta;
                record.Ghi_chu = Ghi_chu;
                db.SaveChanges();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult CreateNewJS(string Ten, string Mo_Ta, string Ghi_chu)
        {
            //DanhMuc_TaiKhoan record = new DanhMuc_TaiKhoan();
            XepLoaiTotNghiep record = new XepLoaiTotNghiep();
            record.Ten = Ten;
            record.Mo_Ta = Mo_Ta;
            record.Ghi_chu = Ghi_chu;
            record.create_date = DateTime.Now.ToString();
            record.create_user = Session["curUser"].ToString();
            db.XepLoaiTotNghieps.Add(record);
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);

        }
        public JsonResult deleteJS(string id)
        {

            if (id == null)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            else
            {
                XepLoaiTotNghiep record = db.XepLoaiTotNghieps.Find(Int32.Parse(id));
                db.XepLoaiTotNghieps.Remove(record);
                db.SaveChanges();
                return Json(1, JsonRequestBehavior.AllowGet);
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
                XepLoaiTotNghiep record = db.XepLoaiTotNghieps.Find(Int32.Parse(id));
                return Json(new
                {
                    id = record.Id,
                    Ten = record.Ten,
                    Mo_Ta = record.Mo_Ta,
                    Ghi_chu = record.Ghi_chu,
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}