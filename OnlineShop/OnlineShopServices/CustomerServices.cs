using System;
using System.Collections.Generic;
using System.Text;
using OnlineShopPodaci.Model;
using OnlineShopPodaci;
using System.Linq;

namespace OnlineShopServices
{
    public class CustomerServices: ICustomer
    {
        private OnlineShopContext _database;

        public CustomerServices(OnlineShopContext db)
        {
            _database = db;
        }
        public List<Notification>GetNotifications(int userid)
        {
            return _database.notification.Where(a => a.UserID == userid).ToList(); 
        }

        
    }
}
