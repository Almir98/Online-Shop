using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShop.ViewModels;
using OnlineShopPodaci;
using OnlineShopPodaci.Hubs;
using OnlineShopPodaci.Model;
using X.PagedList;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace OnlineShop.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IProduct _Iproduct;
        private readonly OnlineShopContext _database;
        private readonly IHostingEnvironment hosting;
        private  INotification _notificationService;

        public ProductController(IProduct p, OnlineShopContext b, IHostingEnvironment hostingEnvironment, INotification notification)
        {
            _Iproduct = p;
            _database = b;
            hosting = hostingEnvironment;
            _notificationService = notification;
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

        [HttpGet]
        public async Task<IActionResult> Show(string search)
        {
            ViewData["data"] = search;

            var products = _Iproduct.GetAllProducts().Select(p => new ShowProductForManage
            {
                ProductID = p.ProductID,
                ProductNumber = p.ProductNumber,
                SubCategoryName = p.SubCategory.SubCategoryName,
                ManufacturerName = p.Manufacturer.ManufacturerName,
                ProductName = p.ProductName,
                Description = p.Description,
                UnitPrice = p.UnitPrice
            }).ToList();

            var query = from x in products select x;

            if (!String.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.ProductNumber.Contains(search) || x.ProductName.Contains(search));
            }
            return View(await query.ToListAsync());
        }
        
        public IActionResult Delete(int ID)
        {
            _Iproduct.RemoveProduct(ID);
            return Redirect("/Branch/ShowAllBranches");
        }

        private string SaveFile(IFormFile file)
        {
            string uploadFileName = null;
            string filePath = null;
            if (file != null)
            {
                string uploadFoloder = Path.Combine(hosting.WebRootPath, "images");
                uploadFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                filePath = Path.Combine(uploadFoloder, uploadFileName);
                file.CopyTo(new FileStream(filePath, FileMode.Create));
                return "~/images/" + uploadFileName;
            }
            return null;
        }
        
        public IActionResult AddProduct(int ProductID)
        {
            Product temp;
            if (ProductID != 0)
                temp= _database.product.Find(ProductID);
            else
                temp= new Product();
            var data = new AddOrUpdateProductVM
            {
                ProductID = temp.ProductID,
                ProductNumber = temp.ProductNumber,
                SubCategoryID = temp.SubCategoryID,
                Subcategories = _database.subcategory.Select(s => new SelectListItem { Value = s.SubCategoryID.ToString(), Text = s.SubCategoryName }).ToList(),
                ManufacturerID = temp.ManufacturerID,
                Manufacturers = _database.manufacturer.Select(s => new SelectListItem { Value = s.ManufacturerID.ToString(), Text = s.ManufacturerName }).ToList(),
                ProductName = temp.ProductName,
                //Image = temp.ImageUrl,
                Description = temp.Description,
                UnitPrice = temp.UnitPrice,
                UnitsInStock = temp.UnitsInStock
            };
            _database.Add(new AdminActivity
            {
                ActivityID = 1,
                AdminID = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                DateOfActivity=DateTime.Now
            });
            _database.SaveChanges();
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> SaveProduct(AddOrUpdateProductVM model)
        {
            if (ModelState.IsValid)
            {
                string uniquefileName = null;
                if (model.Image != null)
                {
                    string uploadsFolder = Path.Combine(hosting.WebRootPath, "images");
                    uniquefileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniquefileName);
                    model.Image.CopyTo(new FileStream(filePath, FileMode.Create));
                }
                Product neki;
                if (model.ProductID == 0)
                {
                    neki = new Product();
                    _Iproduct.AddProduct(neki);
                }
                else
                    neki = _database.product.Find(model.ProductID);

                neki.ProductNumber = model.ProductNumber;
                neki.SubCategoryID = model.SubCategoryID;
                neki.ManufacturerID = model.ManufacturerID;     
                neki.ProductName = model.ProductName;
                neki.ImageUrl = uniquefileName;                  
                neki.Description = model.Description;
                neki.UnitPrice = model.UnitPrice;
                neki.UnitsInStock = model.UnitsInStock;

                await _database.SaveChangesAsync();        
                if (model.ProductID != neki.ProductID)
                {
                    var st_pr = new StockProduct            
                    {
                        StockID = 1,                                                                        
                        ProductID = neki.ProductID,
                        Quantity = neki.UnitsInStock
                    };
                    _database.Add(st_pr);
                    await _database.SaveChangesAsync();
                }
            }
            await _notificationService.SendNotification($"Dodan je novi artikal ili je izmjenjen postojeći");
            return Redirect("/Product/Show");
        }

        public IActionResult AddManufacturer(int ProductID)
        {
            var product = _Iproduct.GetProductByID(ProductID);
            var model = new AddManufacturerVM
            {
                productID=ProductID
            };
            _database.Add(new AdminActivity
            {
                ActivityID = 8,
                AdminID = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                DateOfActivity = DateTime.Now
            });
            _database.SaveChanges();

            return View(model);
        }

        public IActionResult SaveManufacturer(AddManufacturerVM model)
        {
            string uniquefileName = null;
            if (model.Image != null)
            {
                string uploadsFolder = Path.Combine(hosting.WebRootPath, "images");
                uniquefileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniquefileName);
                model.Image.CopyTo(new FileStream(filePath, FileMode.Create));
            }

            Manufacturer manufacturer = new Manufacturer
            {
                ManufacturerName = model.manufacturerName,
                LogoUrl = uniquefileName
            };
            _Iproduct.AddManufacturer(manufacturer);
            return Redirect($"/Product/AddProduct?={model.productID}");
        }

        public IActionResult AddCategory()
        {
            var model = new AddCategoryVM
            {
            };
            _database.Add(new AdminActivity
            {
                ActivityID = 2,
                AdminID = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                DateOfActivity = DateTime.Now
            });
            _database.SaveChanges();

            return View(model);
        }

        public IActionResult SaveCategory(AddCategoryVM model)
        {
            string uniquefileName = null;
            if (model.Image != null)
            {
                string uploadsFolder = Path.Combine(hosting.WebRootPath, "images");
                uniquefileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniquefileName);
                model.Image.CopyTo(new FileStream(filePath, FileMode.Create));
            }
            Category category = new Category
            {
                CategoryName=model.categoryName,
                ImageUrl=uniquefileName
            };
            _Iproduct.AddCategory(category);
            return Redirect("/Administration/Index");
        }

        public IActionResult AddSubCategory()
        {

            var model = new AddSubCategoryVM
            {
                _lista=_database.category.Select(e=>new SelectListItem
                {
                    Value=e.CategoryID.ToString(),
                    Text=e.CategoryName
                }).ToList()
            };
            _database.Add(new AdminActivity
            {
                ActivityID = 3,
                AdminID = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                DateOfActivity = DateTime.Now
            });
            _database.SaveChanges();

            return View(model);
        }
        public IActionResult SaveSubCategory(AddSubCategoryVM model)
        {
            string uniquefileName = null;
            if (model.Image != null)
            {
                string uploadsFolder = Path.Combine(hosting.WebRootPath, "images");
                uniquefileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniquefileName);
                model.Image.CopyTo(new FileStream(filePath, FileMode.Create));
            }

            SubCategory subcategory = new SubCategory
            {
                SubCategoryName=model.subcategoryName,
                CategoryID=model.categoryID,
                ImageUrl=uniquefileName
            };
            _Iproduct.AddSubCategory(subcategory);
            return Redirect("/Administration/Index");
        }

        public IActionResult Show2()       
        {
           List<ShowCategoriesVM >data = _database.category.Select(c => new ShowCategoriesVM
           {
               CategoryID = c.CategoryID,
               CategoryName = c.CategoryName,
               imageurl=c.ImageUrl
           }).ToList();
            return View(data);
        }

        [AllowAnonymous]
        public IActionResult ShowSubcategories(int id,string search,int? page)          // ID kategorije
        {

            var product = _Iproduct.GetCategoryID(id);
            ViewBag.Name = product.CategoryName;

            var c = _database.subcategory.Where(s => s.CategoryID == id).
                Select(s => new ShowSubCategoriesVM
                {
                 ID=id,
                    CategoryName = s.Category.CategoryName,
                    SubCategoryID = s.SubCategoryID,
                    SubCategoryName = s.SubCategoryName,
                    imageurl=s.ImageUrl
                }).Where(e => e.SubCategoryName.Contains(search) || search == null);

            IPagedList<ShowSubCategoriesVM> lista = c.ToPagedList(page ?? 1, 6);
            return View(lista);
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ShowProducts(int ID,string search)       // ID podkategorije 
        {
            ViewData["data"] = search;
            ViewBag.ID = ID;
            var product = _Iproduct.GetSubCategoryID(ID);
            ViewBag.Name = product.SubCategoryName;

            var products =  _Iproduct.GetAllProducts().Where(s => s.SubCategoryID == ID).
                Select(p => new ShowProductsVM
                {
                    productID = p.ProductID,
                    productName = p.ProductName,
                    manufacturerName = p.Manufacturer.ManufacturerName,
                    unitPrice = p.UnitPrice,
                    unitsInStock = p.UnitsInStock,
                    imageUrl = p.ImageUrl
                }).ToList();

            var query = from x in products select x;

            if (!String.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.productName.Contains(search) || x.manufacturerName.Contains(search));
            }
            return View(await query.ToListAsync());
        }

        [AllowAnonymous]
        public IActionResult ProductDetails(int ID)     // ID proizvoda
        {
            ProductDetailsVM model = _database.product.Where(s=>s.ProductID==ID).Select(a => new ProductDetailsVM
            {
                ProductID =a.ProductID,
                ProductName=a.ProductName,
                ProductNumber =a.ProductNumber,
                SubCategoryName=a.SubCategory.SubCategoryName,
                ManufacturerName=a.Manufacturer.ManufacturerName,
                ImageUrl=a.ImageUrl,
                Description=a.Description,
                UnitPrice=a.UnitPrice,
                UnitsInStock=a.UnitsInStock         
            }).SingleOrDefault();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ShowStock(string search)
        {
            ViewData["data"] = search;

            var products = _Iproduct.GetAllProducts();

            var model = products.Select(e => new ShowProductsInStockVM
            {
                ProductID = e.ProductID,
                ProductName = e.ProductName,
                Price = e.UnitPrice,
                Quantity = _database.stockproduct.Where(a => a.ProductID == e.ProductID).Select(a => a.Quantity).FirstOrDefault()

            }).ToList();

            var query = from x in model select x;

            if (!String.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.ProductName.Contains(search));
            }
            return View(await query.ToListAsync());
        }

        public IActionResult DistributeProduct(int productID)
        {
            var product = _Iproduct.GetProductByID(productID);

            var model = new DistributeProductVM
            {
                productID = product.ProductID,
                productName = product.ProductName,
                price = product.UnitPrice,
                manufacturer = product.Manufacturer.ManufacturerName,
                unitsInStock = product.UnitsInStock,
                category = product.SubCategory.Category.CategoryName,
                subcategory = product.SubCategory.SubCategoryName,

                _list=_database.branch.Select(e=> new rows
                {
                    branchID=e.BranchID,
                    cityname=e.City.CityName,
                    quntityPerBranch=_database.branchproduct.Where(a=>a.BranchID==e.BranchID).Select(a=>a.UnitsInBranch).FirstOrDefault()??0      
                }).ToList(),

                _stock=_database.stock.Select(e=>new rows2
                {
                    stockID=e.StockID,
                    stockName=e.StockName,
                    stockQuanttity=_database.stockproduct.Where(a=>a.StockID==e.StockID && product.ProductID==a.ProductID).Select(a=>a.Quantity).FirstOrDefault()
                }).ToList()
            };
            ViewData["err"] = TempData["error"];
            return View(model);
        }

        public IActionResult SaveBranchProduct(DistributeProductVM model)
        {
            var product = _Iproduct.GetProductByID(model.productID);
            var sum = 0;

            foreach (var i in model._list)
            {
                var bp = _database.branchproduct.Where(e => e.BranchID == i.branchID && e.ProductID==product.ProductID).FirstOrDefault();
                // ide u tabelu jel u toj prodavnici taj proizvod vec postoji

                if (bp!=null && model.productID == bp.ProductID){
                    bp.UnitsInBranch += i.quntityPerBranch;     
                }
                else
                {
                   var testing = new BranchProduct  
                   {
                       BranchID = i.branchID,
                       ProductID = product.ProductID,
                       UnitsInBranch = i.quntityPerBranch,
                   };
                   _database.Add(testing);
                }
                sum+=i.quntityPerBranch;        
            }
            var stock = _database.stockproduct.Where(e => e.ProductID == model.productID).FirstOrDefault();

            if (stock.Quantity >= sum)
            {
                stock.Quantity =stock.Quantity - sum;   
                _database.Add(new AdminActivity
                {
                    ActivityID = 7,
                    AdminID = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                    DateOfActivity = DateTime.Now
                });
                _database.SaveChanges();
            }
            else
            {
                TempData["error"] = "Unijeta količina nije dostupna, pokušajte ponovni unos";
                return Redirect("/Product/DistributeProduct?productID="+product.ProductID);
            }
            _database.SaveChanges();
            return Redirect("/Product/ShowStock");
        }
    }
}