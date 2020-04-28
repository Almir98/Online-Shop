using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModels
{
    public class SendSmsVM
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string PhoneNumber { get; set; }
    }
}
