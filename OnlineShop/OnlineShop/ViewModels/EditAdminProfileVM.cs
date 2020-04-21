using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModels
{
    public class EditAdminProfileVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public int CityID { get; set; }
        public List<SelectListItem> City { get; set; }
        public string Adress { get; set; }
        public string PhoneNumber { get; set; }
        public int GenderID { get; set; }
        public List<SelectListItem> Gender { get; set; }
    }
}
