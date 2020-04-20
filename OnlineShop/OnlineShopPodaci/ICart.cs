using OnlineShopPodaci.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShopPodaci
{
    public interface ICart
    {
        public void AddToCart(int productid,int userid,int q);

        public List<Cart> GetAllCartItemsByUser(int userid);

        public void RemoveCartItem(int productid,int userid);

        public void RemoveAllCartItems(int userid);

        public void ChangeQuantity(int productid,int userid, int q);
        
        
    }
}
