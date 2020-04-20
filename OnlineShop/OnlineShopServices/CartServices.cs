using OnlineShopPodaci;
using OnlineShopPodaci.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace OnlineShopServices
{
    public class CartServices : ICart
    {
        private OnlineShopContext _database;

        public CartServices(OnlineShopContext db)
        {
            _database = db;
        }

        public void AddToCart(int productid,int userid,int q)
        {
            //var userid = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            Cart singlerecord = _database.cart.SingleOrDefault(u => u.UserID == userid && u.ProductID == productid);
            Product product = _database.product.Find(productid);
            if (singlerecord != null)
            {
                singlerecord.Quantity += q;
                singlerecord.TotalPrice = (singlerecord.Quantity + q) * product.UnitPrice;
            }
            else
            {
                Cart newrecord = new Cart
                {
                    UserID = userid,
                    ProductID = productid,
                    Quantity = q,
                    TotalPrice = product.UnitPrice * q
                };
                _database.Add(newrecord);
            }
            _database.SaveChanges();
        }

        public void RemoveAllCartItems(int userid)
        {
            //var userid = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            _database.cart.RemoveRange(_database.cart.Where(p => p.UserID == userid));
            _database.SaveChanges();
        }

        public List<Cart> GetAllCartItemsByUser(int userid)
        {
            //var userid = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            List<Cart> listacart = _database.cart.Where(u => u.UserID == userid).ToList();
            return listacart;

        }

        public void RemoveCartItem(int productid,int userid)
        {
            //var userid = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            _database.cart.Remove(_database.cart.SingleOrDefault(p => p.ProductID == productid && p.UserID == userid));
            _database.SaveChanges();
        }

        public void ChangeQuantity(int productid,int userid, int q)
        {
            //var userid = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (q == 0) { RemoveCartItem(productid,userid); return; }
            var obj = _database.cart.SingleOrDefault(s => s.UserID == userid && s.ProductID == productid);
            obj.Quantity = q;
            _database.SaveChanges();
        }
    }
}
