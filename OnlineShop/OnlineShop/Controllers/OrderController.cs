using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineShopPodaci.Model;
using OnlineShopPodaci;
using OnlineShop.ViewModels;
using System.Security.Claims;

namespace OnlineShop.Controllers
{
    public class OrderController : Controller
    {
        private OnlineShopContext _database;
        private IOrder _order;
        private ICart _cart;
        public OrderController(OnlineShopContext database,IOrder order,ICart cart)
        {
            _order = order; _database = database; _cart = cart;
        }

        public IActionResult OrderPreview()
        {
            var userid = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var listacart = _order.GetAllCartItemsByUser(userid);
            var user = _database.user.Find(userid);
            var model = new OrderPreviewVM
            {
                cartitems=listacart.Select(s=>new LookInCartVM
                {
                    ProductID = s.ProductID,
                    ProductNumber = _database.product.Find(s.ProductID).ProductNumber,
                    ProductName = _database.product.Find(s.ProductID).ProductName,
                    SubCategoryName = _database.subcategory.Find(_database.product.Find(s.ProductID).SubCategoryID).SubCategoryName,
                    UnitPrice = _database.product.Find(s.ProductID).UnitPrice,
                    Quantity = s.Quantity,
                    logourl= _database.product.Find(s.ProductID).ImageUrl
                }).ToList(),
                userid=userid,
                fname=user.Name,
                lname=user.Surname,
                adress=user.Adress,
                phonenumber=user.PhoneNumber??"(nije ispunjeno)",
                
            };
            if(user.PhoneNumber!=null && user.Adress!=null && user.Name != null && user.Surname != null){
                model.FilledInfo = true;
            }
            return View(model);
        }

        public IActionResult SaveOrder()  //funkcija povlaci sve cart items za ovog usera a nakon toga ih briše, obzirom da je dosao do mogucnosti da Zakljuci narudzbu znaci da su svi preduslovi osigurani da se kreira zapis u tabeli Order
        {
            var userid = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var listacart = _order.GetAllCartItemsByUser(userid);
            
            var order = new Order{
                UserID=userid,
                OrderDate=DateTime.Now,
                TotalPrice=_order.GetTotalPrice(userid),
                OrderStatusID=1,
                
            };
            _database.Add(order);
            _database.SaveChanges();
            var id = _database.order.ToList().Last().OrderID; //get orderid
            foreach (var x in listacart){
                var item = new OrderDetails{
                    OrderID=id,
                    ProductID=x.ProductID,
                    Quantity=x.Quantity,
                };
                _database.product.Find(x.ProductID).UnitsInStock -= x.Quantity;  //obzirom da je ovo neko kupio, taj item se umanjuje za datu kolicinu
                _database.Add(item); _database.SaveChanges();
            }
            _cart.RemoveAllCartItems(userid); //obzirom da je sve prešlo u orderdetails, briše se sve iz korpe za tog usera
            Notification nova = new Notification
            {
                UserID = userid,
                Text = "Vaša narudžba (" + id + ") se obrađuje."
            };
            _database.Add(nova);
            _database.SaveChanges();
            return Redirect("OrderMessage");
        }
        public IActionResult OrderMessage()
        {
            return View("SaveOrder");
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}