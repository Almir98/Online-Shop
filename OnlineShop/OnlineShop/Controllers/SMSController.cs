using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nexmo.Api;
using OnlineShop.ViewModels;
using OnlineShopPodaci;

namespace OnlineShop.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SMSController : Controller
    {
        private OnlineShopContext _db;

        public SMSController(OnlineShopContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SendSms(int id)
        {
            var admin = _db.user.Find(id);

            var model = new SendSmsVM
            {
                Id = id,
                PhoneNumber = admin.PhoneNumber,
                Text=null
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult SendSms(SendSmsVM model)
        {

            var client = new Client(creds: new Nexmo.Api.Request.Credentials
            {
                ApiKey = "59fdbb8e",
                ApiSecret = "qf0kqV4nryVpmOJF"
            });
            var results = client.SMS.Send(request: new SMS.SMSRequest
            {
                from = "OnlineShop.Service",
                to = model.PhoneNumber,
                text = model.Text
            });

            return Redirect("/Administration/AdminDetails?id=" + model.Id);
        }

    }
}