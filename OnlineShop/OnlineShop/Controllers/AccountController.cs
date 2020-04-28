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
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace OnlineShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private OnlineShopContext _db;
        private readonly RoleManager<Role> roleManager;
        private readonly IHostingEnvironment hostingEnvironment;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, OnlineShopContext db, 
            RoleManager<Role>roleManager, IHostingEnvironment hostingEnvironment)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _db = db;
            this.roleManager = roleManager;
            this.hostingEnvironment = hostingEnvironment;
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
                string uniqueImageName=null;
                if (model.Image != null)
                {
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                    uniqueImageName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueImageName);
                    model.Image.CopyTo(new FileStream(filePath, FileMode.Create));
                }
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
                    GenderID=model.GenderID,
                    ImageUrl=uniqueImageName

                };

                var result = await userManager.CreateAsync(user, model.Password);
                if(result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    var role = await roleManager.FindByIdAsync(1.ToString());
                    await userManager.AddToRoleAsync(user, role.Name);

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
                    {
                        var user = await userManager.FindByNameAsync(model.Email);
                        if (await userManager.IsInRoleAsync(user, "Customer"))
                            return RedirectToAction("Index", "Home");
                        else
                            return RedirectToAction("Index", "Administration");
                    }

                ModelState.AddModelError("", "Neuspješan pokušaj prijave!");
            }
            return View(model);
        }
        public IActionResult Contact(string textForMesage,string mail,string ime,string adresa)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("OnlineShop", "rs1.onlineshop.service@gmail.com"));
            message.To.Add(new MailboxAddress(ime, mail));
            message.Subject = "OnlineShop Service Notification";
            message.Body = new TextPart("plain")
            {
                Text=textForMesage
            };
            using(var client=new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("rs1.onlineshop.service@gmail.com", "onlineShop!1");
                client.Send(message);
                client.Disconnect(true);

            }

            return Redirect(adresa);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }

}