using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModels
{
    public class AddOrUpdateProductVMedit
    {
        public int ProductID { get; set; }
        [Required]
        public string ProductNumber { get; set; }
        public int SubCategoryID { get; set; }
        public List<SelectListItem> Subcategories { get; set; }
        public int ManufacturerID { get; set; }
        public List<SelectListItem> Manufacturers { get; set; }
        [Required]
        public string ProductName { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public double UnitPrice { get; set; }
    }
}
