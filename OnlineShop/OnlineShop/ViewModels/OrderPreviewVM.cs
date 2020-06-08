using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineShop.ViewModels;
using OnlineShop.Models;
namespace OnlineShop.ViewModels
{
    public class OrderPreviewVM
    {
        public List<LookInCartVM> cartitems { get; set; }
        public int userid { get; set; }
        public string fname { get; set; }
        public string lname { get; set; }
        public string adress { get; set; }
        public string phonenumber { get; set; }
        public bool FilledInfo { get; set; }  //if false, order could not be made
        public string logourl { get; set; }

    }
}
