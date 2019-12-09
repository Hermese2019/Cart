using Carts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Carts.Controllers
{
    public class MVCController : Controller
    {
        private CartsEntities db = new CartsEntities();
        // GET: MVC
        public ActionResult Index()
        {
            //宣告回傳商品列表result
            List<Product> result = new List<Product>();

            //使用CartsEntities類別，名稱為db
            using (CartsEntities db = new CartsEntities())
            {
                //使用LinQ語法抓取目前Products資料庫中所有資料
                result = db.Product.ToList();
                return View(result);
            }

        }

        //建立商品頁面
        public ActionResult Create()
        {
            return View();
        }

        //建立商品頁面 - 資料傳回處理
        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (this.ModelState.IsValid)    //如果資料驗證成功
            {
                using (CartsEntities db = new CartsEntities())
                {
                    db.Product.Add(product);
                    db.SaveChanges();
                    TempData["ResultMessage"] = String.Format("商品[{0}]成功建立", product.Name);
                    return RedirectToAction("Index");
                }
            }
            //失敗訊息
            ViewBag.ResultMessage = "資料有誤，請檢查";
            //停留在Create頁面
            return View(product);
        }

        //編輯商品頁面
        public ActionResult Edit(int id)
        {

            Product product = db.Product.Where(p => p.Id == id).FirstOrDefault();
            if (product != default(Models.Product))
            //判斷id是否有資料(類別的默認值是null，所以當product找不到資料時會等於null，product 會等於 default(Models.Product))
            //就跳到else
            {
                return View(product);
            }
            else
            {
                //如果沒資料則傳回錯誤訊息，並導回Index頁面
                TempData["resultMessage"] = "資料有誤，請重新操作";
                return RedirectToAction("Index");
            }

        }

        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (this.ModelState.IsValid)
            {
                //這邊不知為何，string類型為空，ModelState.IsValid不會變成false，但product該位置的資料的確是null
                //導致後面畫面顯示資料庫存處錯誤，解法:到product新增[Required]
                Product result = db.Product.Where(p => p.Id == product.Id).FirstOrDefault();
                result.Name = product.Name;
                result.Price = product.Price;
                result.PublishDate = product.PublishDate;
                result.Quantity = product.Quantity;
                result.Status = product.Status;
                result.CategoryId = product.CategoryId;
                result.DefaultImageId = product.DefaultImageId;
                result.Description = product.Description;
                result.DefaultImageURL = product.DefaultImageURL;
                db.SaveChanges();
                TempData["ResultMessage"] = String.Format("商品[{0}]成功編輯", product.Name);
                return RedirectToAction("Index");
            }
            ViewBag.Errmessage = "修改資料失敗";
            return View();
        }

        [HttpPost]
        //為什麼刪除需要使用Post呢？這是因為刪除是很重要的操作，如果使用者直接在網址列輸入網址就可以刪除的話
        //，其實是很危險的一件事情，所以我們選擇使用Post來完成。
        public ActionResult Delete(int id)
        {
            Product result = db.Product.Where(p => p.Id == id).FirstOrDefault();
            if (result != default(Models.Product))
            {

                db.Product.Remove(result);
                db.SaveChanges();
                TempData["resultMessage"] = $"已刪除 編號:{result.Id} 名稱:{result.Name} 該商品資料";
                return RedirectToAction("Index");
            }
            else
            {
                //如果沒資料則傳回錯誤訊息，並導回Index頁面
                TempData["resultMessage"] = "指定資料不存在，無法刪除，請重新操作";
                return RedirectToAction("Index");
            }
        }
    }
}