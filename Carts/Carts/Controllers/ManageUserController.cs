using Carts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Carts.Controllers
{
    public class ManageUserController : Controller
    {
        private UserEntities db = new UserEntities();
        // GET: ManageUser
        public ActionResult Index()
        {
            ViewBag.ResultMessage = TempData["ResultMessage"];
            //抓取所有AspNetUsers中的資料，並且放入Models.ManageUser模型中
            var result = (from s in db.AspNetUsers
                          select new Models.ManageUser
                          {
                              Id = s.Id,
                              UserName = s.UserName,
                              Email = s.Email
                          }).ToList();
            return View(result);
        }

        public ActionResult Edit(string id)
        {
            var result = (from s in db.AspNetUsers
                          where s.Id == id
                          select new ManageUser
                          {
                              Id = s.Id,
                              UserName = s.UserName,
                              Email = s.Email
                          }).FirstOrDefault();
            //個人覺得寫 if (result != default(Models.ManageUser))滿有事的
                if (result != null)
            {
                return View(result);
            }
            TempData["ResultMessage"] = String.Format("使用者[{0}]不存在，請重新操作", id);
            return RedirectToAction("Index");

        }

        [HttpPost]
        public ActionResult Edit(ManageUser manageUser)
        {
            var result = db.AspNetUsers.Where(p => p.Id == manageUser.Id).FirstOrDefault();
            if (result != null)
            {
                result.UserName = manageUser.UserName;
                result.Email = manageUser.Email;
                db.SaveChanges();
                TempData["ResultMessage"] = String.Format("使用者[{0}]成功編輯", manageUser.UserName);
                return RedirectToAction("Index");
            }
            TempData["ResultMessage"]=String.Format("使用者[{0}]不存在，請重新操作",manageUser.UserName);
            return RedirectToAction("Index");
        }


    }
}