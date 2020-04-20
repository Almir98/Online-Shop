using OnlineShopPodaci.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModels
{
    public class EditOrderVM
    {
        public int OrderID { get; set; }
        public int UserId { get; set; }
        public string UserInfo { get; set; }
        public string OrderStatus { get; set; } 
        public string OrderDate { get; set; }
        public List<Products> items { get; set; }
        public class Products
        {
            public int ProductID { get; set; }
            public string ProductName { get; set; }
            public int RequiredQuantity { get; set; }
            public List<Branches> branches { get; set; }
            public class Branches
            {
                public int BranchID { get; set; }
                public string BranchName { get; set; }
                public int AvailableQuantity { get; set; }
                public int Input { get; set; }
            }
        }
    }
}
