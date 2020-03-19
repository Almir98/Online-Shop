using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OnlineShopPodaci.Model
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }
        public string ProductNumber { get; set; }

        [ForeignKey("SubCategory")]
        public int SubCategoryID { get; set; }
        public SubCategory SubCategory { get; set; }

        [ForeignKey("Manufacturer")]
        public int ManufacturerID { get; set; }
        public Manufacturer Manufacturer { get; set; }
        
        public string ProductName { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public double UnitPrice { get; set; }
        public int UnitsInStock { get; set; }
        
    }
}
