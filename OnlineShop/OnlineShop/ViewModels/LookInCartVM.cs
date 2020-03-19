using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModels
{
    public class LookInCartVM
    {
        public int ProductID { get; set; }
        public string ProductNumber { get; set; }
        public string ProductName { get; set; }
        public string SubCategoryName { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
        

    }
}
