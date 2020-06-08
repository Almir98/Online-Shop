using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModels
{
    public class OrdersExcelVM
    {
        public int OrderID { get; set; }
        public int UserID { get; set; }
        public double OrderDate { get; set; }
        public double ShipDate { get; set; }
        public string UserInfo { get; set; }
        public double TotalPrice { get; set; }

    }
}
