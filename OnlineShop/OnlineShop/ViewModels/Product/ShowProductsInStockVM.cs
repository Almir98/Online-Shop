using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModels
{
    public class ShowProductsInStockVM
    {
        public List<rows> _lista { get; set; }

        public class rows
        {
            public int ProductID { get; set; }
            public string ProductName { get; set; }
            public double Price { get; set; }
            public int Quantity { get; set; }
        }

    }
}
