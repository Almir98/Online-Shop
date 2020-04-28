using System;
using System.Collections.Generic;
using System.Text;
using OnlineShopPodaci.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineShopPodaci.Model
{
    public class Notification
    {   [Key]
        public int NotificationID { get; set; }
        [ForeignKey("UserID")]
        public User User { get; set; }
        public int UserID { get; set; }
        public string Text { get; set; }
        public bool Read { get; set; }
        
    }
}
