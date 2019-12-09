using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Carts.Models
{
    public static class Operation
    {
        public static Cart GetCurrentCart() //取得目前Session中的Cart物件
        {
            //客戶的網站若是沒啟用session的話直接用this.session!=null做判斷會拋出錯誤，所以使用HttpContext.Current做判斷
            if (HttpContext.Current != null)  //確認System.Web.HttpContext.Current是否為空
            {
                //如果Session["Cart"]不存在，則新增一個空的Cart物件
                if (HttpContext.Current.Session["Cart"] == null)
                {
                    var order = new Cart();
                    HttpContext.Current.Session["Cart"] = order;
                }
                //回傳Session["Cart"]
                return (Cart)HttpContext.Current.Session["Cart"];
            }
            else
            {
                throw new InvalidOperationException("HttpContext.Current為空，請檢察");
            }
        }
    }
}