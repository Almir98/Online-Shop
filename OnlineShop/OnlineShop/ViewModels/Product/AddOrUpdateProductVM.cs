using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineShop.ViewModels
{
    public class AddOrUpdateProductVM
    {
        public int ProductID { get; set; }
        
        [Required(ErrorMessage = "Polje šifra proizvoda je obavezno za unos")]
        public string ProductNumber { get; set; }
        
        [Range(1, int.MaxValue, ErrorMessage = "Polje podkategorija je obavezno za odabir")]
        public int SubCategoryID{ get; set; }
        public List<SelectListItem> Subcategories { get; set; }
        
        [Range(1, int.MaxValue, ErrorMessage = "Polje proizvođač je obavezno za odabir")]
        public int ManufacturerID { get; set; }
        public List<SelectListItem> Manufacturers { get; set; }

        [Required(ErrorMessage = "Polje naziv proizvoda je obavezno za unos")]
        public string ProductName { get; set; }
        public IFormFile Image { get; set; }
        
        [Required(ErrorMessage = "Polje dataljan opis je obavezno za unos")]
        public string Description { get; set; }
        
        [Range(1, double.MaxValue, ErrorMessage = "Polje cijena je obavezno za unos")]
        public double UnitPrice{ get; set; }
        
        [Range(1, int.MaxValue, ErrorMessage = "Polje količina je obavezno za unos")]
        public int UnitsInStock { get; set; }
    }
}
