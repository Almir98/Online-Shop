using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.ViewModels;
using OnlineShopPodaci;
using OnlineShopPodaci.Model;

namespace OnlineShop.Controllers
{
    public class CartController : Controller
    {   
        private ICart _cart;
        private OnlineShopContext _database;

        public CartController(ICart cart,OnlineShopContext db)
        {
            _cart = cart;
            _database = db;
        }

        [Authorize(Roles = "Customer")]

        public IActionResult AddToCart(int productid,int q = 1)
        {
            var userid = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            _cart.AddToCart(productid,userid,q);
            return View("ItemAdded");
        }

        public IActionResult LookInCart()
        {
            return View();
        }
        public IActionResult GetCartItems()  
        {
            var userid = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var listacart = _cart.GetAllCartItemsByUser(userid);
            List<LookInCartVM> listavm = listacart
            .Select(s => new LookInCartVM
            {
                ProductID = s.ProductID,
                ProductNumber = _database.product.Find(s.ProductID).ProductNumber,
                ProductName = _database.product.Find(s.ProductID).ProductName,
                SubCategoryName = _database.subcategory.Find(_database.product.Find(s.ProductID).SubCategoryID).SubCategoryName,
                UnitPrice = _database.product.Find(s.ProductID).UnitPrice,
                Quantity = s.Quantity,
                ActualQuantity= _database.product.Find(s.ProductID).UnitsInStock
            }
            ).ToList();
            return PartialView(listavm);
        }
      
        public IActionResult RemoveFromCart(int productid)
        {
            var userid = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            _cart.RemoveCartItem(productid,userid);
            return Redirect("/Cart/GetCartItems");
        }
        public IActionResult DeleteCart()
        {
            var userid = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            _cart.RemoveAllCartItems(userid);
            return Redirect("GetCartItems");
        }
        
        [HttpGet]
        public IActionResult SetQuantity(int productid,int q)
        {
            var userid = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            _cart.ChangeQuantity(productid,userid, q);
            var listacart = _cart.GetAllCartItemsByUser(userid);
            List<LookInCartVM> listavm = listacart
            .Select(s => new LookInCartVM
            {
                ProductID = s.ProductID,
                ProductNumber = _database.product.Find(s.ProductID).ProductNumber,
                ProductName = _database.product.Find(s.ProductID).ProductName,
                SubCategoryName = _database.subcategory.Find(_database.product.Find(s.ProductID).SubCategoryID).SubCategoryName,
                UnitPrice = _database.product.Find(s.ProductID).UnitPrice,
                Quantity = s.Quantity,
                ActualQuantity=_database.product.Find(s.ProductID).UnitsInStock
            }
            ).ToList();
            return PartialView("GetCartItems",listavm);

        }


        public IActionResult Index()
        {
            return View();
        }
    }
}