using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModels
{
    public class AddManufacturerVM
    {
        public int productID { get; set; }
        public string manufacturerName { get; set; }
        public IFormFile Image { get; set; }
    }
}
