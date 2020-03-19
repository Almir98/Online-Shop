using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineShopPodaci.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShopPodaci
{
    public class OnlineShopContext:IdentityDbContext<User,Role,int>
    {
        public OnlineShopContext(DbContextOptions<OnlineShopContext> opcije):base(opcije){
        
        }
        public OnlineShopContext()
        {

        }

        public DbSet<User> user { get; set; }
        public DbSet<CardType> cardtype { get; set; }
        public DbSet<CreditCard> creditcard { get; set; }
        public DbSet<Category> category { get; set; }
        public DbSet<City> city { get; set; }
        public DbSet<Cart> cart { get; set; }
        public DbSet<Gender> gender { get; set; }
        public DbSet<Manufacturer> manufacturer { get; set; }
        public DbSet<OrderDetails> orderdetails { get; set; }
        public DbSet<Order> order { get; set; }
        public DbSet<Product> product { get; set; }
        public DbSet<SubCategory> subcategory { get; set; }
        public DbSet<Stock> stock { get; set; }
        public DbSet<StockProduct> stockproduct { get; set; }
        public DbSet<BranchProduct> branchproduct { get; set; }
        public DbSet<Branch> branch { get; set; }
        public DbSet<Role> role { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<CartDetails>().HasKey(c => new { c.CartID, c.ProductID });

            //modelBuilder.Entity<CartDetails>()
            //    .HasOne(cd => cd.Product)
            //    .WithMany(c => c._CartDetails)
            //    .HasForeignKey(cd => cd.ProductID);

            //modelBuilder.Entity<CartDetails>()
            //    .HasOne(cd => cd.Cart)
            //    .WithMany(c => c._CartDetails)
            //    .HasForeignKey(cd => cd.CartID);

            //fluent api for composite keys

            base.OnModelCreating(modelBuilder);
            modelBuilder.Ignore<IdentityUserLogin<string>>();
            modelBuilder.Ignore<IdentityUserRole<string>>();
            modelBuilder.Ignore<IdentityUserClaim<string>>();
            modelBuilder.Ignore<IdentityUserToken<string>>();
            modelBuilder.Ignore<IdentityUser<string>>();

            modelBuilder.Entity<Cart>()
                .HasKey(c => new { c.ProductID, c.UserID });

            modelBuilder.Entity<OrderDetails>()
                .HasKey(c => new { c.ProductID,c.OrderID });

            modelBuilder.Entity<BranchProduct>().HasKey(e => new { e.BranchID, e.ProductID });

            modelBuilder.Entity<StockProduct>().HasKey(o => new { o.ProductID, o.StockID });



        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(@"Server=app.fit.ba,1431;Database=OnlineShopDB;Trusted_Connection=False; MultipleActiveResultSets=true;User=OnlineShopUser;Password=ANA116m125");
            
            //OVDJE VISE NIJE POTREBNO NAVODITI KONEKCIJSKI STRING
           
            // optionsBuilder.UseSqlServer(@"Server=localhost;Database=nova2;Trusted_Connection=True;MultipleActiveResultSets=true;");

        }



    }
}
