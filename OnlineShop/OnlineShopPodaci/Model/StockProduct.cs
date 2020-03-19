using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShopPodaci.Model
{
    public class StockProduct
    {
        public int StockID { get; set; }
        public Stock Stock { get; set; }

        public int ProductID { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }
    }
}
