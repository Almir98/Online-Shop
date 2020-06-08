using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OnlineShopPodaci;
using OnlineShopPodaci.Hubs;
using OnlineShopPodaci.Model;
using OnlineShopServices;

using Rotativa.AspNetCore;
namespace OnlineShop
{
    public class Startup
    {
       
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
           
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddIdentity<User, Role>(options =>
             {
                 options.User.RequireUniqueEmail = true;
             }).AddEntityFrameworkStores<OnlineShopContext>();
            
            services.AddSignalR();
            services.AddScoped<IDailyReport, DailyReportService>();
            services.AddScoped<IProduct, ProductServices>();
            services.AddScoped<ICart, CartServices>();
            services.AddScoped<IOrder, OrderServices>();
            services.AddScoped<IBranch, BranchService>();
            services.AddScoped<INotification, NotificationService>();
            services.AddScoped<ICustomer, CustomerServices>();
            services.AddDbContext<OnlineShopContext>(c => c.UseSqlServer(Configuration.GetConnectionString("OnlineShopCS")));
            services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);
            //services.AddSingleton(provider => _quartz); //staro

            //services.AddSingleton<IJobFactory, QuartzJonFactory>();
            //services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            // Add our job
            //services.AddSingleton<DailyReport>();
            //services.AddSingleton(new JobSchedule(
            //    jobType: typeof(DailyReport),
            //    cronExpression: "0/5 * * * * ?")); // run every 5 seconds

        }
        /*   
         0 0 20/24 ? * * *         
        svaka 24h   */



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<NotificationHub>("/notification");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            RotativaConfiguration.Setup(env);

        }
    }
}
