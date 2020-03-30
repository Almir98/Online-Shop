using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineShopPodaci.Model;
using OnlineShopPodaci;
using OnlineShop.ViewModels;

namespace OnlineShop.Controllers
{
    public class OrderController : Controller
    {
        private OnlineShopContext _database;
        private IOrder _order;
        public OrderController(OnlineShopContext database,IOrder order)
        {
            _order = order; _database = database;
        }

        public IActionResult OrderPreview(int userid)
        {
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
                    Quantity = s.Quantity
                }).ToList(),
                userid=userid,
                fname=user.Name,
                lname=user.Surname,
                adress=user.Adress,
                phonenumber=user.PhoneNumber??"(nema)"
            };
            return View(model);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}