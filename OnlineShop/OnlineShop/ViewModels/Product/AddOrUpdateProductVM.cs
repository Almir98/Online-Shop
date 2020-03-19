using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShopPodaci.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModels
{
    public class AddOrUpdateProductVM
    {
        public int ProductID { get; set; }
        [Required]
        public string ProductNumber { get; set; }

        public int SubCategoryID{ get; set; }
        public List<SelectListItem> Subcategories { get; set; }

        public int ManufacturerID { get; set; }
        public List<SelectListItem> Manufacturers { get; set; }
        [Required]
        public string ProductName { get; set; }
        public IFormFile Image { get; set; }
        public string Description { get; set; }
        public double UnitPrice{ get; set; }
        public int UnitsInStock { get; set; }
    }
}
