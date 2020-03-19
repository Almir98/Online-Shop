using System;
using System.Collections.Generic;
using System.Text;
using OnlineShopPodaci.Model;

namespace OnlineShopPodaci
{
    public interface IOrder
    {
        List<Cart> GetAllCartItemsByUser(int userid);

    }
}
