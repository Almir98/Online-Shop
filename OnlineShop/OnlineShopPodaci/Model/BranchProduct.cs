using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OnlineShopPodaci.Model
{
    public class BranchProduct
    {
        public int BranchID { get; set; }
        public Branch Branch { get; set; }

        public int ProductID { get; set; }
        public Product Product { get; set; }

        public int? UnitsInBranch { get; set; }

    }
}
