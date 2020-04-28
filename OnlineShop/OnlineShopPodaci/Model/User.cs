using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OnlineShopPodaci.Model
{
    public class User : IdentityUser<int> 
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        
        [ForeignKey("City")]
        public int CityID { get; set; }
        public City City { get; set; }

        public string Adress { get; set; }
        public string PhoneNumber { get; set; }

        [ForeignKey("Gender")]
        public int GenderID { get; set; }
        public Gender Gender { get; set; }

        [ForeignKey("CreditCard")]
        public int? CreditCardID { get; set; }
        public CreditCard? CreditCard { get; set; }
        public string ImageUrl { get; set; }


        public static ReadOnlySpan<char> FindFirstValue(string nameIdentifier)
        {
            throw new NotImplementedException();
        }
    }
}
