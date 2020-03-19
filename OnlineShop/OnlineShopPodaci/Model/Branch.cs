using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineShopPodaci.Model
{
    public class Branch
    {
        [Key]
        public int BranchID { get; set; }

        public string BranchName { get; set; }

        public int CityID { get; set; }
        public City City { get; set; }

        public string PhoneNumber { get; set; }
        public string Adress { get; set; }
        public string Open { get; set; }
        public string Close { get; set; }

    }
}
