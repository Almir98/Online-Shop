using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineShopPodaci.Model
{
    public class Gender
    {
        [Key]
        public int GenderID { get; set; }
        public string _Gender { get; set; }
    }
}
