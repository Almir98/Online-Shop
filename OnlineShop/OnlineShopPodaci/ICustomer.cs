using System;
using System.Collections.Generic;
using System.Text;
using OnlineShopPodaci.Model;
using OnlineShopPodaci;
namespace OnlineShopPodaci
{
    public interface ICustomer
    {
        List<Notification> GetNotifications(int userid);
    }
}
