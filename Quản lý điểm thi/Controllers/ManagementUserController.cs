using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using Quản_lý_điểm_thi.Models;

namespace Quản_lý_điểm_thi.Controllers
{
    public class ManagementUserController : Controller
    {
        private QLDTEntities1 db = new QLDTEntities1();

        // GET: ManagementUser
        /// <summary>
        /// Updated 2018-10-31
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public ActionResult Index(string searchString)
        {
            int roleID = Convert.ToInt32(Session["RoleIDofUser"]);
            int unitIDOfUser = Convert.ToInt32(Session["IdUnitOfUser"]);
            
            var user = from l in db.Users // lấy toàn bộ 
                           select l;

            ViewBag.searchString = searchString;

            if (!String.IsNullOrEmpty(searchString)) // kiểm tra chuỗi tìm kiếm có rỗng/null hay không
            {
                user = user.Where(s => s.Username.Contains(searchString));
            }

            //chi quan ly nguoi dung cua don vi
            if (roleID != 1)
            {
                user = user.Where(s => s.UnitId == unitIDOfUser);
            }

            return View(user);
        }

        // GET: ManagementUser/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: ManagementUser/Create


        // POST: ManagementUser/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Updated 2018-10-31
        /// </summary>
        /// <param name="user1"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Username,Password,FullName,Birthday,Phone,Mail,Address,CMND")] User user)
        {
            if (ModelState.IsValid)
            {
                //tao doi tuong unit voi parentUnitID ngam dinh
                int unitIDOfUser = Convert.ToInt32(Session["IdUnitOfUser"]);
                //cap nhat parent ID
                user.UnitId = unitIDOfUser;
                user.IsAdmin = false;
                user.Image = "";

                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Username,Password,FullName,Birthday,Phone,Mail,Address,CMND")]User user1)
        {
            User user;

            try
            {
                if (user1 != null)
                {
                    //tao doi tuong unit voi parentUnitID ngam dinh
                    int unitIDOfUser = Convert.ToInt32(Session["IdUnitOfUser"]);
                    //cap nhat parent ID                    

                    user = db.Users.Where(p => p.Id == user1.Id).FirstOrDefault();

                    user.FullName = user1.FullName;
                    user.Username = user1.Username;
                    user.Password = user1.Password;
                    user.Mail = user1.Mail;
                    user.Phone = user1.Phone;
                    user.Address = user1.Address;
                    user.Birthday = user1.Birthday;
                    user.CMND = user1.CMND;
                    //user.UnitId = user1.UnitId;
                    //user.Image = user1.Image;
                    //user.IsAdmin = user1.IsAdmin;

                    user.UnitId = unitIDOfUser;
                    user.IsAdmin = false;
                    user.Image = "";

                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
            
            return View();
            
        }

        // GET: ManagementUser/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id >= 0)
            {
                User user = db.Users.Where(p => p.Id == id).FirstOrDefault();
                if (user.Username != "admin" && user.Username != "u1" && user.Username != "u2" && user.Username != "u3")
                {
                    db.Users.Remove(user);
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Index", "ManagementUser");
        }

        // POST: ManagementUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);

            if (user.Username != "admin" && user.Username != "u1" && user.Username != "u2" && user.Username != "u3")
            {
                db.Users.Remove(user);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
