using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModels
{
    public class AddManufacturerVM
    {
        public int productID { get; set; }
        
        [Required(ErrorMessage = "Polje naziv proizvođača je obavezno za unos")]
        public string manufacturerName { get; set; }
        public IFormFile Image { get; set; }
    }
}
