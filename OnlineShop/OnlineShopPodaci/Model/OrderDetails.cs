using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OnlineShopPodaci.Model
{
    public class OrderDetails
    {
        public int OrderID { get; set; }
        public Order Order { get; set; } 
        
        public int ProductID { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }


    }
}
