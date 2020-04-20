﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModels
{
    public class ShowOrdersVM
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ShipTime { get; set; }
        public int UserID { get; set; }  //za pregled profila, napravit nesto tipa /ShowInfo?userid=666 cisto da admin ima uvida u tog korisnika, njegove informacije neke
        public string UserInfo { get; set; } //u stringu spojeno ime prezime i adresa sa brojem telefona
        public double TotalPrice { get; set; }
        public int OrderStatusID { get; set; }
        public string Status { get; set; }
    }

}
