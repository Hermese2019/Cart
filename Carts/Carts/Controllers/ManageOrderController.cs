using Carts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Carts.Controllers
{
    public class ManageOrderController : Controller
    {
        private CartsEntities db = new CartsEntities();
        private UserEntities dbUser = new UserEntities();
        // GET: ManageOrder
        public ActionResult Index()
        {
            //取得Order中所有資料
            var result = db.Order.ToList();
            return View(result);
        }

        public ActionResult Details(int id)
        {
            //取得OrderId為傳入Id的所有商品列表
            var result = db.OrderDetail.Where(p => p.OrderId == id).ToList();
            if(result.Count == 0)
            {
                //如果商品數目為0，代表該訂單異常(無商品)，則導回商品列表
                return RedirectToAction("Index");
            }
            else
            {
                return View(result);
            }
        }

        public ActionResult SearchByUserName(string UserName)
        {
            //儲存查詢出來的UserId
            string searchUserId = null;
            //查詢目前網站使用者暱稱符合UserName的UserId
            searchUserId = dbUser.AspNetUsers.Where(p => p.UserName == UserName).Select(p=>p.Id).FirstOrDefault() ;
            //如果有存在UserId
            if (!String.IsNullOrWhiteSpace(searchUserId))
            {
                //則將此UserId的所有訂單找出
                var result = db.Order.Where(p => p.UserId == searchUserId).ToList();
                //回傳結果至Index()的View
                return View("Index", result);
            }
            else
            {
                //回傳空結果至Index()的View
                return View("Index", new List<Order>());
            }
        }
    }
}