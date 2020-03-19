using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineShopPodaci.Model
{
    public class Manufacturer
    {
        [Key]
        public int ManufacturerID { get; set; }
        public string ManufacturerName { get; set; }
        public string LogoUrl { get; set; }
    }
}
