using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.ViewModels;
using OnlineShopPodaci.Model;

namespace OnlineShop.Controllers
{
    [Authorize(Roles="Admin")]
    public class AdministrationController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;

        public AdministrationController(UserManager<User> userManager,RoleManager<Role>roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ListOfCustomers()
        {
            List<ListOfCustomersVM> model = new List<ListOfCustomersVM>();
            foreach(var user in userManager.Users)
            {
                if(await userManager.IsInRoleAsync(user,"Customer"))
                {
                    model.Add(new ListOfCustomersVM
                    {
                        Id=user.Id,
                        Email=user.Email,
                        Firstname=user.Name,
                        LastName=user.Surname,
                        PhoneNumber=user.PhoneNumber
                    });
                }

            }
            return View(model);
        }
        public async Task<IActionResult> SetForAdmin(int id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            await userManager.RemoveFromRoleAsync(user, "Customer");
            await userManager.AddToRoleAsync(user, "Admin");
            return RedirectToAction("Index", "Home");

        }

    }
}