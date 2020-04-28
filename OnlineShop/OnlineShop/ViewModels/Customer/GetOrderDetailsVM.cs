using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModels
{
    public class GetOrderDetailsVM
    {
        public int productid { get; set; }
        public string productnumber { get; set; }
        public string productname { get; set; }
        public double price { get; set; }
        public int quantity { get; set; }

    }
}
