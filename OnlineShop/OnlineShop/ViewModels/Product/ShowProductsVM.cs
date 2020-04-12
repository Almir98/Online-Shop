using System.Collections.Generic;

namespace OnlineShop.ViewModels
{
    public class ShowProductsVM
    {
        public int subcategoryID { get; set; }

        
            public int productID { get; set; }
            public string productName { get; set; }
            public string manufacturerName { get; set; }
            public double unitPrice { get; set; }
            public int unitsInStock { get; set; }
            public string imageUrl { get; set; }
    }
}
