using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModels
{
    public class RegisterVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Lozinku")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name ="Porvrdite lozinku")]
        [Compare("Password",ErrorMessage ="Lozinke nisu iste!")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Ime")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Prezime")]
        public string LastName { get; set; }
        
        public int GradID { get; set; }
        public List<SelectListItem> gradovi { get; set; }

        [Display(Name = "Adresa stanovanja")]
        public string Adress { get; set; }

        [Phone]
        [Display(Name = "Broj telefona")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Datum rodjenja")]
        public DateTime BirthDate { get; set; }

        public int GenderID { get; set; }
        public List<SelectListItem> genders { get; set; }

    }
}
