using Microsoft.AspNetCore.Http;

namespace OnlineShop.ViewModels
{
    public class AddCategoryVM
    {
        public string categoryName { get; set; }
        public IFormFile Image { get; set; }
    }
}
