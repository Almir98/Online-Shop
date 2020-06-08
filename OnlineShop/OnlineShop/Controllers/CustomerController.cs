using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShopPodaci;
using OnlineShopPodaci.Model;
using OnlineShop.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace OnlineShop.Controllers
{
    public class CustomerController : Controller
    {
        private OnlineShopContext _database;
        private ICustomer _customer;
        
        public CustomerController(ICustomer customer,OnlineShopContext db)
        {
            _database = db;
            _customer = customer;
        }

        [Authorize(Roles = "Customer")]
        public IActionResult ChangeInfo()
        {
            int userid = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = _database.Users.Find(userid);
            var model = new ChangeUserInfoVM
            {
                name=user.Name,
                surname=user.Surname,
                phonenumber=user.PhoneNumber,
                choosencity=user.CityID,
                cities=_database.city.Select(c=>new SelectListItem {Text=c.CityName,Value=c.CityID.ToString() }).ToList(),
                adress=user.Adress,
            };
            return View(model);
        }

        public IActionResult SaveUserInfo(ChangeUserInfoVM model)
        {
            int userid = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = _database.Users.Find(userid);

            user.Name = model.name;
            user.Surname = model.surname;
            user.Adress = model.adress;
            user.CityID = model.choosencity;
            user.PhoneNumber = model.phonenumber;

            _database.SaveChanges();
            return Redirect("/Customer/Panel");
        }
        public IActionResult Panel()
        {   int id = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            TempData["key"] = _database.Users.Include(a => a.City).FirstOrDefault(i => i.Id == id);
            TempData["key2"] = _database.notification.Where(u => u.UserID == id).ToList();
            return View();
        }
        public IActionResult GetOrders()
        {
            int id = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var model = _database.order.Where(s=>s.OrderStatusID==2 && s.UserID==id).Select(o => new GetOrdersVM
            {
                orderid=o.OrderID,
                orderdate=o.OrderDate.ToShortDateString(),
                shipdate=o.ShipDate.ToShortDateString(),
                totalprice=o.TotalPrice
            }).ToList();
            return PartialView(model);
        }
        public IActionResult GetOrderDetails(int orderid)
        {
            int id = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var model = _database.orderdetails.Include(a=>a.Product).Where(od => od.OrderID == orderid).Select(s => new GetOrderDetailsVM
            {
                productid=s.ProductID,
                productnumber=s.Product.ProductNumber,
                productname=s.Product.ProductName,
                price=s.Product.UnitPrice,
                quantity=s.Quantity
            }).ToList();

            return PartialView(model);
        }
     
        public IActionResult GetNotifications()
        {
            int id = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var lista = _customer.GetNotifications(id);
            TempData["key2"] = lista;
            return PartialView();
        }
        public int GetNotifNum()
        {
            var userid = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return _database.notification.Where(a => a.UserID == userid).Count(s => s.Read == false);
        }
        public void SetAllNotifRed()
        {
            var userid = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            _database.notification.Where(u => u.UserID == userid && u.Read == false).ToList().ForEach(s=>s.Read=true);
            _database.SaveChanges();
        }


    } 
}