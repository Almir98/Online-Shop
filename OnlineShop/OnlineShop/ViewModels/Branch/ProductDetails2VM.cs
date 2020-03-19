using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModels
{
    public class ProductDetails2VM
    {
        public int productID { get; set; }
        public string productName { get; set; }
        public string manufacturer { get; set; }
        public string categoryName { get; set; }
        public string subCategoryName { get; set; }
        public double productPrice { get; set; }
        public int? quantity { get; set; }
    }
}
