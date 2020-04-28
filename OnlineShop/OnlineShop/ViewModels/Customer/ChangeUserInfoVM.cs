using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShopPodaci.Model;
namespace OnlineShop.ViewModels
{
    public class ChangeUserInfoVM
    {
        public string name { get; set; }
        public string surname { get; set; }
        public int choosencity { get; set; }
        public List<SelectListItem> cities { get; set; }
        public string adress { get; set; }
        public string phonenumber { get; set; }
    }
}
