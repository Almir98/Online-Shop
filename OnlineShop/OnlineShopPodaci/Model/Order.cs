using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OnlineShopPodaci.Model
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }
        public int UserID { get; set; }
        public User User { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ShipDate { get; set; }
        public double TotalPrice { get; set; }
        [ForeignKey("OrderStatus")]
        public int OrderStatusID { get; set; }
        public OrderStatus OrderStatus { get; set; }

    }
}
