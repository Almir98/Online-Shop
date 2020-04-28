using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModels
{
    public class GetOrdersVM
    {
        public int orderid { get; set; }
        public string orderdate { get; set; }
        public string shipdate { get; set; }
        public double totalprice { get; set; }

    }
}
