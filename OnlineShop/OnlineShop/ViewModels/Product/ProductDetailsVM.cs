using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModels
{
    public class ProductDetailsVM
    {
        public int ProductID { get; set; }

        public string ProductNumber { get; set; }

        public string ProductName { get; set; }

        // public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }

        public string ManufacturerName { get; set; }

        public string ImageUrl { get; set; }

        public string Description { get; set; }

        public double UnitPrice { get; set; }
        
        public int UnitsInStock { get; set; }

    }
}
