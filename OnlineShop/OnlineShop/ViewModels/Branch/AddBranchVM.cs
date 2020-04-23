using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModels
{
    public class AddBranchVM
    {
        [Required(ErrorMessage ="Polje naziv poslovnice je obavezno za unos")]
        [MinLength(4, ErrorMessage="Polje treba sadržavati minimalno 4 karaktera")]
        public string branchName { get; set; }

        public int cityID { get; set; }
        public List<SelectListItem> _cities { get; set; }

        [Required(ErrorMessage = "Polje broj telefona je obavezno za unos")]
        public string phoneNumber { get; set; }

        [Required(ErrorMessage = "Polje adresa je obavezno za unos")]
        public string adress { get; set; }
        public string open { get; set; }
        public string close { get; set; }
    }
}
