using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModels
{
    public class OrderAsPdfVM
    {
        public int OrderID { get; set; }
        public string OrderDate { get; set; }
        public string UserInfo { get; set; }
        public string OrderStatus { get; set; }
        public double TotalPrice { get; set; }
        public List<Rows> items { get; set; }
        public class Rows
        {
            public string ProductNumber { get; set; }
            public string ProductName { get; set; }
            public int Quantity { get; set; }
            public double UnitPrice { get; set; }
        }

    }
}
