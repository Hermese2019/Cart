using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Carts.Models
{
    public class PartialClass
    {
    }

    //定義Order的部分類別
    public partial class Order
    {
        //取得訂單中的使用者暱稱
        public string GetUserName()
        {
            //使用Order類別中的UserId到AspNetUsers資料表中搜尋出UserName
            using (Models.UserEntities db =new UserEntities())
            {
                var result = db.AspNetUsers.Where(p => p.Id == this.UserId).FirstOrDefault().UserName;
                //回傳找到的UserName
                return result;
            }
        }
    }

    //ProductComment
    public partial class ProductComment
    {
        //取得訂單中的使用者暱稱
        public string GetUserName()
        {
            //使用ProductComment類別中的UserId到AspNetUsers資料表中搜尋出UserName
            using (Models.UserEntities db = new UserEntities())
            {
                var result = db.AspNetUsers.Where(p => p.Id == this.UserId).FirstOrDefault().UserName;
                //回傳找到的UserName
                return result;
            }
        }
    }
}