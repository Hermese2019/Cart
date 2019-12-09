using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Carts.Models
{
    public class Cart :IEnumerable<CartItem>  //購物車類別
    {
        //建構子
        public Cart()
        {
            this.cartItems = new List<CartItem>();
        }
        //儲存所有商品
        private List<CartItem> cartItems;
        public int Count
        {
            get
            {
                return this.cartItems.Count;
            }
        }
        //取得商品總價
        public decimal TotalAmount
        {
            get
            {
                decimal totalAmount = 0.0m;
                foreach(var cartItem in this.cartItems)
                {
                    totalAmount = totalAmount + cartItem.Amount;
                }
                return totalAmount;
            }
        }

        //新增一筆Product，使用ProductId
        public bool AddProduct(int ProductId)
        {
            var findItem = cartItems.Where(p => p.Id == ProductId).FirstOrDefault();
            //判斷相同Id的CartItem是否已經存在購物車內
            if(findItem == null)
            {
                using(CartsEntities db=new CartsEntities())
                {
                    var product = db.Product.Where(p => p.Id == ProductId).FirstOrDefault();
                    if(product != null)
                    {
                        this.AddProduct(product);
                    }
                }
            }
            else
            {
                //存在購物車內，則將商品數量增加
                findItem.Quantity += 1;
            }
            return true;
        }

        //新增一筆Product，使用Product物件
        private bool AddProduct(Product product)
        {
            //將Prodcut轉為CartItem
            var cartItem = new CartItem()
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Quantity = 1,
                DefaultImageURL=product.DefaultImageURL
            };
            //加入CartItem至購物車
            this.cartItems.Add(cartItem);
            return true;
        }

        //移除一筆Product，使用ProductId
        public bool RemoveProduct(int ProductId)
        {
            var findItem = this.cartItems.Where(p => p.Id == ProductId).FirstOrDefault();
            //判斷相同Id的CartItem是否已經存在購物車內
            if(findItem == null)
            {
                //不存在購物車內，不需做任何動作
            }
            else
            {
                this.cartItems.Remove(findItem);
            }
            return true;
        }

        //清空購物車
        public bool ClearCart()
        {
            this.cartItems.Clear();
            return true;
        }

        //將購物車商品轉成OrderDetail的List
        public List<OrderDetail> ToOrderDetaiList(int orderId)
        {
            var result = new List<OrderDetail>();
            foreach(var cartItem in cartItems)
            {
                result.Add(new OrderDetail()
                {
                    Name = cartItem.Name,
                    Price = cartItem.Price,
                    Quantity = cartItem.Quantity,
                    OrderId = orderId
                });
            }
            return result;
        }

        #region IEnumerator
        public IEnumerator<CartItem> GetEnumerator()
        {
            return this.cartItems.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.cartItems.GetEnumerator();
        }
        #endregion


    }
}