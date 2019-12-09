using Carts.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Carts.Controllers
{
    public class HomeController : Controller
    {
        private CartsEntities db = new CartsEntities();
        public ActionResult Index()
        {
            var result = db.Product.ToList();
            return View(result);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Details(int id)
        {
            var result = db.Product.Where(p => p.Id == id).FirstOrDefault();
            if(result == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(result);
            }
        }

        [HttpPost] //限定使用POST
        [Authorize] //登入會員才可留言
        public ActionResult AddComment(int id, string Content)
        {
            //取得目前登入使用者Id
            var userId = HttpContext.User.Identity.GetUserId();

            var currentDateTime = DateTime.Now;

            var comment = new ProductComment()
            {
                ProductId = id,
                Content = Content,
                UserId = userId,
                CreateDate = currentDateTime
            };

            db.ProductComment.Add(comment);
            db.SaveChanges();
            return RedirectToAction("Details",new { id=id});
        }
        
    }
}