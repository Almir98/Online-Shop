using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineShopPodaci.Model
{
    public class CardType
    {
        [Key]
        public int CardTypeID { get; set; }
        public string TypeName { get; set; }
    }
}
