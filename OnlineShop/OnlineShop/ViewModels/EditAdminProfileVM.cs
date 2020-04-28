using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModels
{
    public class EditAdminProfileVM
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }
        
        [Required]
        public DateTime BirthDate { get; set; }
        public int CityID { get; set; }

        [Required]
        public List<SelectListItem> City { get; set; }
        public string Adress { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        public int GenderID { get; set; }

        [Required]
        public List<SelectListItem> Gender { get; set; }
    }
}
