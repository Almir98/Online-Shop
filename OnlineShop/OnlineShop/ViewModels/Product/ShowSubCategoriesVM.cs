using OnlineShopPodaci.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModels
{
    public class ShowSubCategoriesVM
    {
        public int ID{ get; set; }

        public string CategoryName { get; set; }
        public int SubCategoryID { get; set; }
        public string  SubCategoryName { get; set; }
        public string imageurl { get; set; }

    }
}
