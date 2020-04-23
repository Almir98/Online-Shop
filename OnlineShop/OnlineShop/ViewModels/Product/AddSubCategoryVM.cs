using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModels
{
    public class AddSubCategoryVM
    {
        [Range(1, int.MaxValue, ErrorMessage = "Polje kategorija je obavezno za odabir")]
        public int categoryID { get; set; }
        public List<SelectListItem> _lista { get; set; }
        
        [Required(ErrorMessage = "Polje naziv podkategorije je obavezno za unos")]
        public string subcategoryName { get; set; }
        
        public IFormFile Image { get; set; }
    }
}
