using Microsoft.EntityFrameworkCore;
using OnlineShopPodaci;
using OnlineShopPodaci.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlineShopServices
{
    public class ProductServices:IProduct
    {
        private readonly OnlineShopContext _context;
        public ProductServices(OnlineShopContext b)
        {
            _context = b;
        }

        public void AddCategory(Category category)
        {
            _context.Add(category);
            _context.SaveChanges();
        }

        public void AddManufacturer(Manufacturer manufacturer)
        {
            _context.Add(manufacturer);
            _context.SaveChanges();
        }

        public void AddProduct(Product p)
        {
            _context.Add(p);
            //_context.SaveChanges();
        }

        public void AddSubCategory(SubCategory subcategory)
        {
            _context.Add(subcategory);
            _context.SaveChanges();
        }

        public List<Product> GetAllProducts()
        {
            return _context.product.Include(p => p.SubCategory).Include(p => p.Manufacturer).Include(e=>e.SubCategory.Category). ToList();
        }

        public Category GetCategoryID(int id)
        {
            return _context.category.Where(e=>e.CategoryID==id).FirstOrDefault();
        }

        public Product GetProductByID(int id)
        {
            return GetAllProducts().Where(p => p.ProductID == id).FirstOrDefault();
        }

        public SubCategory GetSubCategoryID(int id)
        {
            return _context.subcategory.Where(e=>e.SubCategoryID==id).FirstOrDefault();
        }

        public void RemoveProduct(int id)
        {
            _context.Remove(GetProductByID(id));
            _context.SaveChanges();
        }
    }
}
