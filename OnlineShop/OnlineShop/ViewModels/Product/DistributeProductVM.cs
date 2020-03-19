using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShopPodaci.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModels
{
    public class DistributeProductVM
    {
        public int productID { get; set; }
        public string productName { get; set; }
        public double  price { get; set; }
        public int unitsInStock { get; set; }
        public string manufacturer { get; set; }
        public string category { get; set; }
        public string subcategory { get; set; }

        public List<rows> _list { get; set; }
        public List<rows2> _stock { get; set; }
    }

    public class rows
    {
        public int branchID { get; set; }
        public string cityname { get; set; }
        public int quntityPerBranch { get; set; }
    }

    public class rows2
    {
        public int stockID { get; set; }
        public string stockName { get; set; }
        public int? stockQuanttity { get; set; }
    }
}
