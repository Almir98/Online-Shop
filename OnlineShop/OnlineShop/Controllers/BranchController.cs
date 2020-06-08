using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.ViewModels;
using OnlineShopPodaci;
using OnlineShopPodaci.Model;
using X.PagedList;

namespace OnlineShop.Controllers
{
    public class BranchController : Controller
    {
        private readonly IBranch _branch;
        private readonly IProduct _product;
        private readonly OnlineShopContext _context;

        public BranchController(IBranch branch,IProduct product,OnlineShopContext context)
        {
            _branch = branch;
            _product = product;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> ShowAllBranches(string search)
        {
            ViewData["data"] = search;
            var branches = _branch.GetAllBranches();

            var model = _branch.GetAllBranches().Select(e => new ShowAllBranchesVM
            {
                branchID = e.BranchID,
                branchName = e.BranchName,
                city = _context.city.Where(x => x.CityID == e.CityID).Select(a => a.CityName).FirstOrDefault()
            }).ToList();

            var query = from x in model select x;

            if (!String.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.city.Contains(search) || x.branchName.Contains(search));
            }
            return View(await query.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> ShowProductsInBranch(int branchID,string search)
        {
            ViewBag.ID =branchID;
            
            var branch = _branch.GetBranchByID(branchID);
            var products = _context.branchproduct.Where(e=>e.BranchID== branchID).Select(e=>e.Product).ToList();

            var model = new ShowProductsInBranchVM
            {
                branchID=branch.BranchID,
                branchName=branch.BranchName,
                branchAdress=branch.Adress,
                branchPhoneNumber=branch.PhoneNumber,
                branchCity=_context.city.Where(e=>e.CityID==branch.CityID).Select(e=>e.CityName).FirstOrDefault(),
                openTime=branch.Open,
                closeTime=branch.Close,

                _list=products.Select(e=>new ShowProductsInBranchVM.rows
                {
                    productID=e.ProductID,
                    productName=e.ProductName,
                    imageUrl=_context.product.Where(a=>a.ProductID==e.ProductID).Select(a=>a.ImageUrl).FirstOrDefault()
                }).ToList()
            };

            return View(model);
        }

        public IActionResult ProductDetail(int productID,int branchID)
        {
            var product = _product.GetProductByID(productID);

            var branchproduct = _context.branchproduct.Where(e => e.ProductID == productID && e.BranchID==branchID).FirstOrDefault();

            var model = new ProductDetails2VM
            {
                productID=productID,
                productName=product.ProductName,
                manufacturer=product.Manufacturer.ManufacturerName,
                categoryName=product.SubCategory.Category.CategoryName,
                subCategoryName=product.SubCategory.SubCategoryName,
                productPrice=product.UnitPrice,
                quantity=branchproduct.UnitsInBranch
            };
            return View(model);
        }

        public IActionResult AdminOptions(int productID)
        {
            var model = new AdminOptionsVM
            {
                productID=productID
            };
                return PartialView(model);
        }

        public IActionResult AddBranch()
        {
            var model = new AddBranchVM
            {
                _cities= _context.city.Select(e=>new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value=e.CityID.ToString(),
                    Text=e.CityName
                }).ToList()
            };
            _context.Add(new AdminActivity
            {
                ActivityID = 9,
                AdminID = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                DateOfActivity = DateTime.Now
            });
            _context.SaveChanges();
            return View(model);
        }

        public IActionResult SaveBranch(AddBranchVM model)
        {
            var newone = new Branch
            {
                BranchName=model.branchName,
                CityID=model.cityID,
                PhoneNumber=model.phoneNumber,
                Adress=model.adress,
                Open=model.open,
                Close=model.close
            };
            _branch.AddBranch(newone);

            return Redirect("/Branch/ShowAllBranches");
        }

    }
}