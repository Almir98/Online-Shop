using OnlineShopPodaci.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShopPodaci
{
    public interface IProduct
    {
        void AddProduct(Product p);
        List<Product> GetAllProducts();
        
        Product GetProductByID(int id);
        SubCategory GetSubCategoryID(int id);
        Category GetCategoryID(int id);

        void RemoveProduct(int id);

        void AddCategory(Category category);

        void AddSubCategory(SubCategory category);

        void AddManufacturer(Manufacturer manufacturer);
    }
}
