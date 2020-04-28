using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModels
{
    public class BranchQuantityVM
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int RequiredQuantity { get; set; }
        public bool IsFlag { get; set; }
        public List<Rows> branches { get; set; }
        public class Rows
        {
            public int BranchID { get; set; }
            public string BranchName { get; set; }
            public int UnitsInBranch { get; set; }
            public int Input { get; set; }
            
        }

    }
}
