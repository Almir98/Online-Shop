using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModels
{
    public class ShowAllBranchesVM
    {
        public List<rows> _list { get; set; }

        public class rows
        {
            public int branchID { get; set; }
            public string branchName { get; set; }
            public string city { get; set; }
        }
    }
}
