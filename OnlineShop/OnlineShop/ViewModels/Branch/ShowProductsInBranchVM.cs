using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModels
{
    public class ShowProductsInBranchVM
    {
        public int branchID { get; set; }
        public string branchName { get; set; }
        public string branchAdress { get; set; }
        public string branchPhoneNumber { get; set; }
        public string branchCity { get; set; }

        public string openTime { get; set; }
        public string closeTime { get; set; }


        public List<rows> _list { get; set; }

        public class rows
        {
            public int productID { get; set; }
            public string productName { get; set; }
            public string imageUrl { get; set; }


            // moze se dodat 

        }
    }
}
