using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace OnlineShop.ViewModels
{
    public class AddCategoryVM
    {
        [Required(ErrorMessage = "Polje naziv kategorije je obavezno za unos")]
        public string categoryName { get; set; }
        public IFormFile Image { get; set; }
    }
}
