using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShop.ViewModels;
using OnlineShopPodaci;
using OnlineShopPodaci.Model;

namespace OnlineShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private OnlineShopContext _db;

        public AccountController(UserManager<User> userManager,SignInManager<User> signInManager,OnlineShopContext db)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _db = db;
        }

        [HttpGet]       
        public IActionResult Register()
        {
            var model = new RegisterVM { gradovi = _db.city.Select(c => new SelectListItem {Value=c.CityID.ToString(),Text=c.CityName })
                .ToList(),
            genders = _db.gender.Select(c => new SelectListItem { Value = c.GenderID.ToString(), Text = c._Gender})
                .ToList()
            };
            return View(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if(ModelState.IsValid)
            {
                var user = new User
                {
                    Email = model.Email,
                    UserName = model.Email,
                    Name=model.FirstName,
                    Surname=model.LastName,
                    BirthDate=model.BirthDate,
                    Adress=model.Adress,
                    CityID=model.GradID,
                    PhoneNumber=model.PhoneNumber,
                    GenderID=model.GenderID

                };

                var result = await userManager.CreateAsync(user, model.Password);
                if(result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var err in result.Errors)
                    ModelState.AddModelError("", err.Description);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model,string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email,model.Password,model.RememberMe,false);
                if (result.Succeeded)
                    if (!string.IsNullOrEmpty(returnUrl))
                        return LocalRedirect(returnUrl);
                    else
                        return RedirectToAction("Index", "Home");
                
                ModelState.AddModelError("", "Neuspješan pokušaj prijave!");
            }
            return View(model);
        }
    }

}