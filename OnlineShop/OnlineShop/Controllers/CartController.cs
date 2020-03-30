using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.ViewModels;
using OnlineShopPodaci;

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

        public IActionResult AddToCart(int productid, int userid = 6, int q = 1)
        {
            _cart.AddToCart(productid, userid, q);
            return View("ItemAdded");
        }

        public IActionResult LookInCart(int userid = 6)
        {
            return View();
        }
        public IActionResult GetCartItems(int userid = 6)
        {
            var listacart = _cart.GetAllCartItemsByUser(userid);
            List<LookInCartVM> listavm = listacart
            .Select(s => new LookInCartVM
            {
                ProductID = s.ProductID,
                ProductNumber = _database.product.Find(s.ProductID).ProductNumber,
                ProductName = _database.product.Find(s.ProductID).ProductName,
                SubCategoryName = _database.subcategory.Find(_database.product.Find(s.ProductID).SubCategoryID).SubCategoryName,
                UnitPrice = _database.product.Find(s.ProductID).UnitPrice,
                Quantity = s.Quantity
            }
            ).ToList();
            return PartialView(listavm);
        }
      


        public IActionResult RemoveFromCart(int productid, int userid)
        {
            _cart.RemoveCartItem(productid, userid);
            return Redirect("/Cart/GetCartItems?userid="+userid);
        }
        public IActionResult DeleteCart(int userid)
        {
            _cart.RemoveAllCartItems(userid);
            return Redirect("GetCartItems?userid=" + userid);
        }
        
        [HttpGet]
        public IActionResult SetQuantity(int productid,int userid,int q)
        {
            _cart.ChangeQuantity(productid, userid, q);
            var listacart = _cart.GetAllCartItemsByUser(userid);
            List<LookInCartVM> listavm = listacart
            .Select(s => new LookInCartVM
            {
                ProductID = s.ProductID,
                ProductNumber = _database.product.Find(s.ProductID).ProductNumber,
                ProductName = _database.product.Find(s.ProductID).ProductName,
                SubCategoryName = _database.subcategory.Find(_database.product.Find(s.ProductID).SubCategoryID).SubCategoryName,
                UnitPrice = _database.product.Find(s.ProductID).UnitPrice,
                Quantity = s.Quantity
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