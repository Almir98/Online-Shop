using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShopPodaci.Model
{
    public class Stock
    {
        public int StockID { get; set; }
        public string StockName { get; set; }
        public string Adress { get; set; }
        
        public int CityID { get; set; }
        public City City { get; set; }

    }
}
