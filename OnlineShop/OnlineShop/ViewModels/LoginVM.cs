using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModels
{
    public class LoginVM
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required]
        [Display(Name ="Lozinka")]
        public string Password { get; set; }

        [Display(Name ="Zapamti me")]
        public bool RememberMe { get; set; }

    }
}
