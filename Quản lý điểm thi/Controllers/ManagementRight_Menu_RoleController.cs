using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Quản_lý_điểm_thi.Models;

namespace Quản_lý_điểm_thi.Controllers
{
    public class ManagementRight_Menu_RoleController : Controller
    {
        private QLDTEntities1 db = new QLDTEntities1();

        /// <summary>
        /// Search trong list
        /// </summary>
        /// <param name="lst"></param>
        /// <param name="rID"></param>
        /// <returns></returns>
        private bool Search(List<Quản_lý_điểm_thi.Models.Role_MenuItem> lst, int rID, int mID)
        {
            for(int i=0; i<lst.Count; i++)
            {
                if (lst[i].RoleId == rID && lst[i].MenuItemId == mID)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Search itemMenu trong list item of role
        /// </summary>
        /// <param name="lst"></param>
        /// <param name="rID"></param>
        /// <returns></returns>
        private bool Search(List<Quản_lý_điểm_thi.Models.Role_MenuItem> lst, int mID)
        {
            for (int i = 0; i < lst.Count; i++)
            {
                if (lst[i].MenuItemId == mID)
                {
                    return true;
                }
            }

            return false;
        }

        // GET: ManagementRight_Menu_Role
        public ActionResult Index(string roleID, string[] lstItemOfRole)
        {
            var lstMenu = db.MenuItems.ToList();            
            int id = 1;
            //test
            string str = "";

            if (!string.IsNullOrEmpty(roleID))
            {
                id = Convert.ToInt32(roleID);
                ViewBag.RoleCurID = id;
            }

            // update database
            if (lstItemOfRole != null)
            {
                IEnumerable<Role_MenuItem> lstMIofR = db.Role_MenuItem.Where(p => p.RoleId == id);
                db.Role_MenuItem.RemoveRange(lstMIofR);
                db.SaveChanges();

                for (int i = 0; i < lstItemOfRole.Length; i++)
                {
                    Role_MenuItem curAdd = new Role_MenuItem();
                    curAdd.RoleId = id;
                    curAdd.MenuItemId = Convert.ToInt32(lstItemOfRole[i]);
                    db.Role_MenuItem.Add(curAdd);

                    //str += " " + lstItemOfRole[i];                
                }

                db.SaveChanges();
                str = "Phân quyền đã lưu thành công";
                ViewBag.KQ = str;

                //ViewBag.SavedID = id;
                //return RedirectToAction("Index", "ManagementRight_Menu_Role");
            }


            List<Quản_lý_điểm_thi.Models.Role_MenuItem> lstItemIDofRole = db.Role_MenuItem.Where(p => p.RoleId == id).ToList();
            

            List<MnuItem> menu = new List<MnuItem>();
            List<MnuItem> lstParent = new List<MnuItem>();

            foreach (MenuItem item in lstMenu)
                {
                    MnuItem cur = new MnuItem();

                    cur.Id = item.Id.ToString();
                    cur.MenuName = item.Name;
                    cur.Url = item.Url;
                    cur.ParentId = item.ParentId.ToString();
                    cur.OrderIndex = item.OrderIndex.ToString();
                    cur.Icon = "";
                    if(Search(lstItemIDofRole, item.Id))
                    {
                        cur.Checked = true;
                    }

                    menu.Add(cur);

                    if(cur.ParentId == "0")
                    {
                        lstParent.Add(cur);
                    }
                }

            ViewBag.ListMenu = menu;
            ViewBag.ListParent = lstParent;

            ViewBag.RoleID = id;           

            

            //str += ". Cho RoleDI: " + roleID;
            ViewBag.KQ = str;

            return View();
        }
    }
}