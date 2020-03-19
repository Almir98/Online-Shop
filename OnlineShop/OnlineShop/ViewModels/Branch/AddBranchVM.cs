using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModels
{
    public class AddBranchVM
    {
        public string branchName { get; set; }
        public int cityID { get; set; }
        public List<SelectListItem> _cities { get; set; }

        public string phoneNumber { get; set; }
        public string adress { get; set; }
        public string open { get; set; }
        public string close { get; set; }
    }
}
