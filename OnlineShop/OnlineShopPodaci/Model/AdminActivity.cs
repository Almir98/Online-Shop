using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OnlineShopPodaci.Model
{
    public class AdminActivity
    {
        public int ID { get; set; }
        
        [ForeignKey("Admin")]
        public int AdminID { get; set; }
        public User Admin { get; set; }

        [ForeignKey("Activity")]
        public int ActivityID { get; set; }
        public ActivitY Activity { get; set; }
        public DateTime DateOfActivity { get; set; }

    }
}
