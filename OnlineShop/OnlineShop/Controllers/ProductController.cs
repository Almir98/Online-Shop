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
using OnlineShopPodaci.Model;
using X.PagedList;

namespace OnlineShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProduct Iproduct;
        private readonly OnlineShopContext _database;
        private readonly IHostingEnvironment hosting;

        public ProductController(IProduct p, OnlineShopContext b, IHostingEnvironment hostingEnvironment)
        {
            Iproduct = p;
            _database = b;
            hosting = hostingEnvironment;
        }


        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Show()
        {
            var proizvodi = Iproduct.GetAllProducts();
            var productForView = proizvodi.Select(p => new ShowProductForManage
            {
                ProductID = p.ProductID,
                ProductNumber = p.ProductNumber,
                SubCategoryName = p.SubCategory.SubCategoryName,
                ManufacturerName = p.Manufacturer.ManufacturerName,
                ProductName = p.ProductName,
                Description = p.Description,
                UnitPrice = p.UnitPrice
            });
            var data = new SohwProductForManageLIST { ListOfProducts = productForView };
            return View(data);
        }

        public IActionResult Delete(int ID)
        {
            Iproduct.RemoveProduct(ID);
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
            return View(data);
        }

        public IActionResult SaveProduct(AddOrUpdateProductVM model)
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
                    Iproduct.AddProduct(neki);
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

                _database.SaveChanges();        // da bi proizvod dobio svoj id ! 

                if (model.ProductID != neki.ProductID)
                {
                    var st_pr = new StockProduct            // medjutabela
                    {
                        StockID = 5,                                                                         // jer je samo 1 skladiste
                        ProductID = neki.ProductID,
                        Quantity = neki.UnitsInStock
                    };
                    _database.Add(st_pr);
                    _database.SaveChanges();
                }
            }
            return Redirect("/Product/Show");
        }

        public IActionResult AddManufacturer(int ProductID)
        {
            ViewData["idP"] =ProductID;
            return View("AddManufacturer");
        }

        public IActionResult SaveManufacturer(string manufacturerName, string logoURL,int ProductID)
        {
            Manufacturer manufacturer = new Manufacturer
            {
                ManufacturerName = manufacturerName,
                LogoUrl = logoURL
            };

            _database.manufacturer.Add(manufacturer);
            _database.SaveChanges();
            return Redirect($"/Product/AddProduct?={ProductID}");

        }

        public IActionResult Show2()       
        {
           List<ShowCategoriesVM >data = _database.category.Select(c => new ShowCategoriesVM { CategoryID = c.CategoryID, CategoryName = c.CategoryName }).ToList();
            
            return View(data);
        }

        public IActionResult ShowSubcategories(int id,string search,int? page)          // ID kategorije
        {
            var c = _database.subcategory.Where(s => s.CategoryID == id).
                Select(s => new ShowSubCategoriesVM
                {
                 ID=id,
                    CategoryName = s.Category.CategoryName,
                    SubCategoryID = s.SubCategoryID,
                    SubCategoryName = s.SubCategoryName,
                    imageurl=s.ImageUrl
                }).Where(e => e.SubCategoryName.StartsWith(search) || search == null);

            IPagedList<ShowSubCategoriesVM> lista = c.ToPagedList(page ?? 1, 6);
            return View(lista);
        }

        public IActionResult ShowProducts(int ID)       // ID podkategorije 
        {
            List<ShowProductsVM> products = _database.product.Where(s => s.SubCategoryID == ID).
                Select(p => new ShowProductsVM
                {
                    productID = p.ProductID,
                    productName = p.ProductName,
                    manufacturerName = p.Manufacturer.ManufacturerName,
                    unitPrice = p.UnitPrice,
                    unitsInStock = p.UnitsInStock,
                    imageUrl = p.ImageUrl
                }).ToList();
            
            return View(products);
        }
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


        public IActionResult ShowStock()
        {
            var products = Iproduct.GetAllProducts();

            var model = new ShowProductsInStockVM
            {
                _lista = products.Select(e => new ShowProductsInStockVM.rows
                {
                    ProductID=e.ProductID,
                    ProductName=e.ProductName,
                    Price=e.UnitPrice,
                    Quantity=_database.stockproduct.Where(a=>a.ProductID==e.ProductID).Select(a=>a.Quantity).FirstOrDefault()
                }).ToList()
            };
            return View(model);
        }

        public IActionResult DistributeProduct(int productID)
        {
            var product = Iproduct.GetProductByID(productID);

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
            return View(model);
        }

        public IActionResult SaveBranchProduct(DistributeProductVM model)
        {
            var product = Iproduct.GetProductByID(model.productID);
            var sum = 0;
            // za svaku prodavnicu rasporedjujemo proizvode

            foreach (var i in model._list)
            {
                var bp = _database.branchproduct.Where(e => e.BranchID == i.branchID).FirstOrDefault();
                if (bp!=null && model.productID == bp.ProductID)
                {
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
                sum = sum + i.quntityPerBranch;        // sabiraju se kolicine po prodavnicama
            }
            _database.SaveChanges();
            var stock = _database.stockproduct.Where(e => e.ProductID == model.productID).FirstOrDefault();

            stock.Quantity =stock.Quantity - sum;         
            _database.SaveChanges();
            return Redirect("/Product/ShowStock");
        }

    }
}