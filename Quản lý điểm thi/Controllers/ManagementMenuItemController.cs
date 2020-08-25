using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Quản_lý_điểm_thi.Models;
using System.Data.Entity;
using System.Data;

namespace Quản_lý_điểm_thi.Controllers
{
    public class ManagementMenuItemController : Controller
    {
        QLDTEntities1 db = new QLDTEntities1();

        #region Quan ly menu ----------------------------------------------
        /// <summary>
        /// List
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.Message = "List Menu";

            var lstMenu = db.MenuItems.ToList();
            return View(lstMenu);
        }

        /// <summary>
        /// View add form
        /// </summary>
        /// <returns></returns>
        public ActionResult MenuAdd()
        {
            //Menu menu = new Menu();
            return View();// menu);
        }

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="menuAdd"></param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult MenuAdd(MenuItem menuAdd)
        {
            if (menuAdd != null)
            {
                MenuItem menu = new MenuItem();
                
                menu.Name = menuAdd.Name;
                menu.ParentId = menuAdd.ParentId;
                menu.OrderIndex= menuAdd.OrderIndex;
                menu.Url = menuAdd.Url;
                menu.Icon = menuAdd.Icon;

                db.MenuItems.Add(menu);
                db.SaveChanges();
            }
            return View();
        }

        /// <summary>
        /// Edit form view
        /// </summary>
        /// <returns></returns>
        public ActionResult MenuEdit(int id)
        {
            MenuItem menu = db.MenuItems.Where(p => p.Id == id).FirstOrDefault();

            return View(menu);
        }

        /// <summary>
        /// Edit process
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MenuEdit(MenuItem mnuEdit)
        {
            if (mnuEdit != null)
            {
                //Shop_Customers cusChk = db.Shop_Customers.Where(p => p.CustomerCode == customer.CustomerCode).FirstOrDefault();
                MenuItem menu = db.MenuItems.Where(p => p.Id == mnuEdit.Id).FirstOrDefault();

                menu.Name = mnuEdit.Name;
                menu.ParentId = mnuEdit.ParentId;
                menu.OrderIndex = mnuEdit.OrderIndex;
                menu.Url = mnuEdit.Url;
                menu.Icon = mnuEdit.Icon;

                db.Entry(menu).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index", "ManagementMenuItem");
            }

            return View();
        }


        [HttpGet]
        /// <summary>
        /// Edit form view
        /// </summary>
        /// <returns></returns>
        public ActionResult MenuDelete(int id)
        {
            if (id >= 0)
            {
                MenuItem menu = db.MenuItems.Where(p => p.Id == id).FirstOrDefault();
                db.MenuItems.Remove(menu);
                db.SaveChanges();
            }

            return RedirectToAction("Index", "ManagementMenuItem");
            //return null;//  View();
        }

        /// <summary>
        /// Edit form view
        /// </summary>
        /// <returns></returns>
        public ActionResult MenuDetail(int id)
        {
            MenuItem menu = db.MenuItems.Where(p => p.Id == id).FirstOrDefault();

            return View(menu);
        }
        #endregion -------------------------------------------------------
    }
}