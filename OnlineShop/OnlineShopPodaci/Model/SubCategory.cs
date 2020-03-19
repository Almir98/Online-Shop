using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OnlineShopPodaci.Model
{
    public class SubCategory
    {
        [Key]
        public int SubCategoryID { get; set; }

        [ForeignKey("Category")]
        public int CategoryID { get; set; }     
        public Category Category { get; set; }

        public string SubCategoryName { get; set; }
        public string ImageUrl { get; set; }
    }
}
