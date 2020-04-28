using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModels
{
    public class AdminDetailsVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public string Adress { get; set; }
        public string PhoneNumber { get; set; }
        public string CityName { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public int NumberOfActivities { get; set; }
        public bool ShowButton { get; set; }
        public string ImageUrl { get; set; }
        public List<ROW> rows { get; set; }

        public class ROW
        {
            public string Description { get; set; }
            public DateTime DateOfActivity { get; set; }
        }
    }
}
