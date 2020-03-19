using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OnlineShopPodaci;
using OnlineShopPodaci.Model;

namespace OnlineShopServices
{
    public class OrderServices:IOrder
    {
        private OnlineShopContext _database;
        public OrderServices(OnlineShopContext database)
        {
            _database = database;
        }

        public List<Cart> GetAllCartItemsByUser(int userid)
        {
            List<Cart> listacart = _database.cart.Where(u => u.UserID == userid).ToList();
            return listacart;
        }
    }
}
