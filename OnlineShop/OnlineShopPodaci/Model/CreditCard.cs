using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OnlineShopPodaci.Model
{
    public class CreditCard
    {
        [Key]
        public int CreditCardID { get; set; }

        [ForeignKey("CardType")]
        public int CartTypeID { get; set; }
        public CardType CardType { get; set; }
        
        public int CreditCardNumber { get; set; }
        public int CSC { get; set; }
        public int ExpMonth { get; set; }
        public int ExpYear { get; set; }
    }
}
