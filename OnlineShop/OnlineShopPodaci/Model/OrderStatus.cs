using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace OnlineShopPodaci.Model
{
    public class OrderStatus
    {   [Key]
        public int OrderStatusId { get; set; }
        public string Status { get; set; }
    }
}
