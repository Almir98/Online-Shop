using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModels
{
    public class UserDetailsVM
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
        public int NumberOfTransactions { get; set; }
        public List<ROW> rows { get; set; }

        public class ROW
        {
            public int TransactionID { get; set; }
            public DateTime OrderDate { get; set; }
            public DateTime ShipDate { get; set; }
            public double TotalPrice { get; set; }
            public int NumberOfProducts { get; set; }
        }

    }
}
