using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Carts.Controllers
{
    public class OrderController : Controller
    {
        private Models.CartsEntities db = new Models.CartsEntities();
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Models.OrderModel.Ship ship)
        {
            if (this.ModelState.IsValid)
            {
                //取得目前購物車
                var currentcart = Models.Operation.GetCurrentCart();
                //取得目前登入使用者Id
                var userId = HttpContext.User.Identity.GetUserId();
                //建立order物件
                var order = new Models.Order()
                {
                    UserId = userId,
                    RecieverName = ship.RecieveName,
                    RecieverPhone = ship.RecievePhone,
                    RecieverAddress = ship.RecieveAddress
                };
                //加其入Orders資料表後，儲存變更
                db.Order.Add(order);
                db.SaveChanges();

                //取得購物車中OderDetail物件
                var orderDetails = currentcart.ToOrderDetaiList(order.Id);

                //將其加入OrderDetails資料表後儲存變更
                db.OrderDetail.AddRange(orderDetails);
                db.SaveChanges();
                currentcart = Models.Operation.GetCurrentCart();
                currentcart.ClearCart();
                return Content("訂購成功");
            }
            return View();
        }

        public ActionResult MyOrder()
        {
            //取得目前登入使用者Id
            var userId = HttpContext.User.Identity.GetUserId();
            var result = db.Order.Where(p => p.UserId == userId).ToList();
            return View(result);
        }

        public ActionResult MyOrderDetail(int id)
        {
            var result = db.OrderDetail.Where(p => p.OrderId == id).ToList();
            if(result.Count == 0)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(result);
            }
        }
    }
}