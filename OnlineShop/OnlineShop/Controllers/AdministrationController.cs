using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using OnlineShop.ViewModels;
using OnlineShopPodaci;
using OnlineShopPodaci.Model;
using Rotativa.AspNetCore;
using Twilio.AspNet;
using Twilio;
using X.PagedList;
using Twilio.Types;
using Twilio.Rest.Api.V2010.Account;
using System.Configuration;
using Microsoft.Extensions.Configuration;

namespace OnlineShop.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdministrationController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;
        private IOrder _order;
        private OnlineShopContext _database;
        private readonly IWebHostEnvironment _env;
        [Obsolete]
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _config;

        [Obsolete]
        public AdministrationController(UserManager<User> userManager, RoleManager<Role> roleManager, IOrder order,
            OnlineShopContext _database, IHostingEnvironment hostingEnvironment,IWebHostEnvironment env,IConfiguration config)
        {
            _config = config;
            _env = env;
            this.userManager = userManager;
            this.roleManager = roleManager;
            _order = order;
            this._database = _database;
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            int id = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var u = _database.user.Find(id);
            AdminDetailsVM model = new AdminDetailsVM
            {
                Id = id,
                Name = u.Name,
                Surname = u.Surname,
                BirthDate = u.BirthDate,
                Adress = u.Adress,
                PhoneNumber = u.PhoneNumber,
                CityName = _database.city.Find(u.CityID).CityName,
                Gender = _database.gender.Find(u.GenderID)._Gender,
                Email = u.Email,
                ShowButton = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)) == id,
                NumberOfActivities = _database.adminactivity.Where(aa => aa.AdminID == id).Count(),
                ImageUrl = u.ImageUrl,
                rows = _database.adminactivity.Where(aa => aa.AdminID == id).Select(aa => new AdminDetailsVM.ROW
                {
                    Description = aa.Activity.Description,
                    DateOfActivity = aa.DateOfActivity
                }).ToList()

            };

            return View(model);
        }
        public IActionResult ShowOrders()
        {
            return View();
        }


        public IActionResult GetOrders()
        {
            var model = _database.order.Include(i => i.OrderStatus).Include(a => a.User).Select(s => new ShowOrdersVM
            {
                OrderID = s.OrderID,
                OrderDate = s.OrderDate,
                ShipTime = s.ShipDate,
                OrderStatusID = s.OrderStatusID,
                Status = s.OrderStatus.Status,
                UserID = s.UserID,
                UserInfo = s.User.Name + " " + s.User.Surname + " | " + s.User.Adress + " | " + s.User.PhoneNumber,
                TotalPrice = s.TotalPrice

            }).OrderByDescending(s => s.OrderDate).ToList();

            return PartialView(model);
        }
        public IActionResult EditOrder(int id, bool again = false)
        {

            //ovdje ide neki VM
            var order = _database.order.Include(i => i.OrderStatus).FirstOrDefault(s => s.OrderID == id);
            var User = _database.user.Find(order.UserID);
            var model = new EditOrderVM
            {
                OrderID = id,
                UserId = order.UserID,
                OrderStatus = _database.orderstatus.Find(order.OrderStatusID).Status,
                OrderDate = order.OrderDate.ToString(),
                UserInfo = User.Name + " " + User.Surname + ", " + User.Adress + "| " + User.PhoneNumber,
                items = _database.orderdetails.Include(i => i.Product).Where(w => w.OrderID == id).Select(o => new EditOrderVM.Products
                {
                    ProductID = o.ProductID,
                    ProductName = o.Product.ProductName,
                    RequiredQuantity = o.Quantity
                }).ToList(),

            };
            foreach (var x in model.items)
            {
                x.branches = _database.branchproduct.Include(b => b.Branch).Where(a => a.ProductID == x.ProductID && a.UnitsInBranch > 0).Select(s => new EditOrderVM.Products.Branches
                {
                    BranchID = s.BranchID,
                    BranchName = s.Branch.BranchName,
                    AvailableQuantity = s.UnitsInBranch ?? 0,
                    Input = 0
                }).ToList();
            }
            if (again == true)
            {
                TempData["msg"] = "Raspodjela nije ispravna.Pokušajte ponovo !";
            }
            return PartialView(model);
        }



        public IActionResult SaveOrderChanges(EditOrderVM model)
        {
            var order = _database.order.Find(model.OrderID);

            foreach (var x in model.items)
            {
                var a = 0;
                foreach (var y in x.branches)
                {
                    a += y.Input;
                    _database.branchproduct.FirstOrDefault(a => a.BranchID == y.BranchID && x.ProductID == a.ProductID).UnitsInBranch -= y.Input;

                }
                if (a != x.RequiredQuantity)
                {
                    return Redirect("/Administration/EditOrder?id=" + model.OrderID + "&again=" + true);
                }
            }
            order.OrderStatusID = 2;
            order.OrderStatus = _database.orderstatus.Find(2);
            order.ShipDate = DateTime.Now;
            Notification nova = new Notification
            {
                UserID = order.UserID,
                Text = "Vaša narudžba (" + model.OrderID + ") je isporučena."
            };
            Parallel.Invoke(() => Sms("Narudzba <"+model.OrderID+"> je isporucena."));
            _database.Add(nova);
            _database.Add(new AdminActivity
            {
                ActivityID = 10,
                AdminID = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                DateOfActivity = DateTime.Now
            });

            _database.SaveChanges();
            foreach(var x in model.items)
            {
                if (_database.product.Find(x.ProductID).UnitsInStock == 0)
                {
                    Sms("Proizvod " + _database.product.Find(x.ProductID).ProductName + " ("+x.ProductID+") vise nije dostupan.");
                }
            }
            return PartialView("SuccessMessage");
        }
        public void Sms(string poruka)
        {
            TwilioClient.Init(_config["accountSid"], _config["authToken"]);
            var message = MessageResource.Create(
                to: new PhoneNumber("+38761931043"),
                from: new PhoneNumber("+14108835319"),
                body:poruka
            );
        }
        public IActionResult CancelOrder(int orderid)
        {
            var order = _database.order.FirstOrDefault(a => a.OrderID == orderid);
            order.OrderStatusID = 3;
            order.OrderStatus = _database.orderstatus.Find(3);
            var list = _database.orderdetails.Include(p => p.Product).Where(s => s.OrderID == orderid).ToList();
            foreach (var x in list)
            {
                _database.product.Find(x.ProductID).UnitsInStock += x.Quantity;  //vracamo tu kolicinu u UnitsInStock jer je s tog mjesta prividno oduzeto, dok je narudzba potvrdjena. U slucaju da se narudzba odobri, onda se oduzima i iz poslovnica. U slucaju otkazivanja narudzbe, vraca se kolicina na UnitsInStock
            }
            Notification nova = new Notification
            {
                UserID = order.UserID,
                Text = "Vaša narudžba (" + orderid + ") je otkazana."
            };
            _database.Add(nova);
            _database.Add(new AdminActivity
            {
                ActivityID = 11,
                AdminID = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                DateOfActivity = DateTime.Now
            });
            _database.SaveChanges();

            return PartialView();
        }



        public async Task<IActionResult> ListOfCustomers(int? page)
        {
            List<ListOfCustomersVM> model = new List<ListOfCustomersVM>();
            foreach (var user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user, "Customer"))
                {
                    model.Add(new ListOfCustomersVM
                    {
                        Id = user.Id,
                        Email = user.Email,
                        Firstname = user.Name,
                        LastName = user.Surname,
                        PhoneNumber = user.PhoneNumber,
                        ImageUrl = user.ImageUrl
                    });
                }

            }
            IPagedList<ListOfCustomersVM> lista = model.ToPagedList(page ?? 1, 9);
            return View(lista);
        }
        public async Task<IActionResult> ListOfAdmins(int? page)
        {
            List<ListOfAdminsVM> model = new List<ListOfAdminsVM>();
            foreach (var user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user, "Admin"))
                {
                    model.Add(new ListOfAdminsVM
                    {
                        Id = user.Id,
                        Email = user.Email,
                        Firstname = user.Name,
                        LastName = user.Surname,
                        PhoneNumber = user.PhoneNumber,
                        ImageUrl = user.ImageUrl
                    });
                }

            }
            IPagedList<ListOfAdminsVM> lista = model.ToPagedList(page ?? 1, 6);
            return View(lista);
        }

        public async Task<IActionResult> SetForAdmin(int id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());

            await userManager.RemoveFromRoleAsync(user, "Customer");
            await userManager.AddToRoleAsync(user, "Admin");

            _database.Add(new AdminActivity
            {
                ActivityID = 6,
                AdminID = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                DateOfActivity = DateTime.Now
            });
            _database.SaveChanges();

            return RedirectToAction("ListOfCustomers", "Administration");

        }
        public IActionResult UserDetails(int id)
        {

            var u = _database.user.Where(u => u.Id == id).Include(u => u.Gender).Include(u => u.City).FirstOrDefault();
            UserDetailsVM model = new UserDetailsVM
            {
                Id = id,
                Name = u.Name,
                Surname = u.Surname,
                BirthDate = u.BirthDate,
                Adress = u.Adress,
                PhoneNumber = u.PhoneNumber,
                CityName = _database.city.Find(u.CityID).CityName,
                Gender = _database.gender.Find(u.GenderID)._Gender,
                Email = u.Email,
                NumberOfTransactions = _database.order.Where(o => o.UserID == id).Count(),
                ImageUrl = u.ImageUrl,
                rows = _database.order.Where(o => o.UserID == id).Select(o => new UserDetailsVM.ROW
                {
                    TransactionID = o.OrderID,
                    OrderDate = o.OrderDate,
                    ShipDate = o.ShipDate,
                    TotalPrice = o.TotalPrice,
                    NumberOfProducts = _database.orderdetails.Where(od => od.OrderID == o.OrderID).Sum(od => od.Quantity)
                }).ToList()

            };

            return View(model);

        }

        [HttpGet]
        public IActionResult EditAdminProfile()
        {
            var id = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = _database.user.Where(u => u.Id == id).Include(u => u.City).Include(u => u.Gender).FirstOrDefault();
            var model = new EditAdminProfileVM
            {
                Id = id,
                Name = user.Name,
                Surname = user.Surname,
                BirthDate = user.BirthDate,
                CityID = user.CityID,
                City = _database.city.Select(c => new SelectListItem { Value = c.CityID.ToString(), Text = c.CityName }).ToList(),
                Adress = user.Adress,
                PhoneNumber = user.PhoneNumber,
                GenderID = user.GenderID,
                Gender = _database.gender.Select(c => new SelectListItem { Value = c.GenderID.ToString(), Text = c._Gender }).ToList()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult EditAdminProfile(EditAdminProfileVM model)
        {
            if (ModelState.IsValid)
            {
                var user = _database.user.Find(model.Id);
                user.Name = model.Name;
                user.Surname = model.Surname;
                user.BirthDate = model.BirthDate;
                user.CityID = model.CityID;
                user.Adress = model.Adress;
                user.PhoneNumber = model.PhoneNumber;
                user.GenderID = model.GenderID;
                _database.Add(new AdminActivity
                {
                    ActivityID = 4,
                    AdminID = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                    DateOfActivity = DateTime.Now
                });
                _database.SaveChanges();
                return RedirectToAction("Index", "Administration");
            }
            return View(model);
        }
        public IActionResult AdminDetails(int id)
        {
            var u = _database.user.Where(u => u.Id == id).Include(u => u.Gender).Include(u => u.City).FirstOrDefault();
            AdminDetailsVM model = new AdminDetailsVM
            {
                Id = id,
                Name = u.Name,
                Surname = u.Surname,
                BirthDate = u.BirthDate,
                Adress = u.Adress,
                PhoneNumber = u.PhoneNumber,
                CityName = _database.city.Find(u.CityID).CityName,
                Gender = _database.gender.Find(u.GenderID)._Gender,
                Email = u.Email,
                ShowButton = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)) == id,
                NumberOfActivities = _database.adminactivity.Where(aa => aa.AdminID == id).Count(),
                ImageUrl = u.ImageUrl,
                rows = _database.adminactivity.Where(aa => aa.AdminID == id).Select(aa => new AdminDetailsVM.ROW
                {
                    Description = aa.Activity.Description,
                    DateOfActivity = aa.DateOfActivity
                }).ToList()

            };

            return View(model);
        }
        public async Task<IActionResult> RemoveAdmin(int id)
        {
            var user = _database.user.Find(id);
            if (await userManager.IsInRoleAsync(user, "Admin"))
            {
                await userManager.RemoveFromRoleAsync(user, "Admin");
                await userManager.AddToRoleAsync(user, "Customer");
            }
            _database.Add(new AdminActivity
            {
                ActivityID = 5,
                AdminID = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                DateOfActivity = DateTime.Now
            });
            _database.SaveChanges();
            var vrijeme = DateTime.Now.ToString();
            string text = "Poštovani, obavještavamo vas da je vaša uloga administratora na stranici OnlineShop-a oduzeta. Uloga vam je oduzeta u "
                + vrijeme + ". OnlineShop Service!";

            return Redirect("/Account/Contact?textForMesage=" + text + "&mail=" + user.Email + "&ime=" + user.Name + "&adresa=/Administration/ListOfAdmins");

        }

        public async Task<IActionResult> Export()
        {
            // query data from database  
            await Task.Yield();
            var list = _database.order.Include(o=>o.OrderStatus).Include(a=>a.User).Where(s => s.OrderStatusID == 2).Select(n=>new OrdersExcelVM { 
            OrderID=n.OrderID,
            OrderDate=n.OrderDate.ToOADate(),
            ShipDate=n.ShipDate.ToOADate(),
            UserID=n.UserID,
            UserInfo=n.User.Name+" "+n.User.Surname,
            TotalPrice=n.TotalPrice,
            }).ToList();
            var stream = new MemoryStream();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(list, true);
                for (int i = 0; i < list.Count(); i++)
                {
                    workSheet.Cells["C" + (i+2).ToString()].Style.Numberformat.Format = "dd-mm-yyyy";
                    workSheet.Cells["D" + (i+2).ToString()].Style.Numberformat.Format = "dd-mm-yyyy";
                }
             
                package.Save();
            }
            stream.Position = 0;
            string excelName = $"Orders-{DateTime.Now.ToShortDateString()}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

       
        public async Task<IActionResult> ConvertToPdf(int id)
        {
            await Task.Yield();

            var order = _database.order.Include(i => i.OrderStatus).FirstOrDefault(s => s.OrderID == id);
            if (order.OrderStatusID == 3) { return View("Error"); } 
            var User = _database.user.Find(order.UserID);
            var model = new OrderAsPdfVM
            {
                OrderID = id,
                OrderStatus = _database.orderstatus.Find(order.OrderStatusID).Status,
                OrderDate = order.OrderDate.ToString(),
                UserInfo = User.Name + " " + User.Surname + ", " + User.Adress + "| " + User.PhoneNumber,
                TotalPrice=order.TotalPrice,
                items = _database.orderdetails.Include(i => i.Product).Where(w => w.OrderID == id).Select(o => new OrderAsPdfVM.Rows
                {
                    ProductNumber = o.Product.ProductNumber,
                    ProductName = o.Product.ProductName,
                    Quantity = o.Quantity,
                    UnitPrice = o.Product.UnitPrice
                }).ToList(),

            };
            return new ViewAsPdf("OrderAsPdf",model);
        }
        //public async Task<IActionResult>DailyReport() {
        
        //    ITrigger trigger = TriggerBuilder.Create()
        //        .WithIdentity($"DailyReport.{DateTime.Now}")
        //        .StartNow()
        //        //.WithSimpleSchedule(y=>y.WithIntervalInSeconds(5).WithRepeatCount(3))
        //        //.WithSimpleSchedule(x=>x.WithIntervalInHours(24).RepeatForever())
        //        .Build();

        //    IJobDetail job = JobBuilder.Create<DailyReport>()
        //        .WithIdentity("DailyReport")
        //        .Build();
        //     await _scheduler.ScheduleJob(job, trigger);

        //    return RedirectToAction("Index"); 
        //}
      

    }
}