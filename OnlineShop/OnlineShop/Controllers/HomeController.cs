using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnlineShop.Models;
using OnlineShop.ViewModels;
using OnlineShopPodaci;
using OnlineShopPodaci.Model;

namespace OnlineShop.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly IProduct _Iproduct;
        private readonly OnlineShopContext _database;

        public HomeController(ILogger<HomeController> logger,OnlineShopContext onlineShopContext,IProduct product)
        {
            _logger = logger;
            _database = onlineShopContext;
            _Iproduct = product;
        }

        public IActionResult Index()
        {
            List<ShowCategoriesVM> data = _database.category.Select(c => new ShowCategoriesVM
            {
                CategoryID = c.CategoryID,
                CategoryName = c.CategoryName,
                imageurl = c.ImageUrl
            }).ToList();


            return View(data);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0,
            Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // LOGIN 
    }
}
