using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModels
{
    public class AddSubCategoryVM
    {
        public int categoryID { get; set; }
        public List<SelectListItem> _lista { get; set; }

        public string subcategoryName { get; set; }
        public IFormFile Image { get; set; }
    }
}
