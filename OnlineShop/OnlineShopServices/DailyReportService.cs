using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml;
using OnlineShopPodaci;
using OnlineShopPodaci.Model;
namespace OnlineShopServices
{
    public class DailyReportService : IDailyReport 
    {
        private readonly OnlineShopContext _database;
        private readonly IHostingEnvironment _hostingEnvironment;
        public DailyReportService(OnlineShopContext database,IHostingEnvironment hostingEnvironment)
        {
            _database = database;
            _hostingEnvironment = hostingEnvironment;

        }
        public void GetDailyReport()
        {
            var list = _database.order.Include(o => o.OrderStatus).Include(a => a.User).Where(s => s.OrderStatusID == 2 && s.ShipDate.Year == DateTime.Now.Year && s.ShipDate.Month == DateTime.Now.Month && s.ShipDate.Day == DateTime.Now.Day).Select(n => new OrdersExcelVM
            {
                OrderID = n.OrderID,
                OrderDate = n.OrderDate.ToOADate(),
                ShipDate = n.ShipDate.ToOADate(),
                UserID = n.UserID,
                UserInfo = n.User.Name + " " + n.User.Surname,
                TotalPrice = n.TotalPrice,
            }).ToList();
            //var stream = new MemoryStream();
            string folder = Path.Combine(_hostingEnvironment.WebRootPath, "DailyReports");
            string excelName = $"Dnevni-{ DateTime.Now.ToShortDateString()}.xlsx";
            FileInfo file = new FileInfo(Path.Combine(folder, excelName));


            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(file))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(list, true);
                for (int i = 0; i < list.Count(); i++)
                {
                    workSheet.Cells["C" + (i + 2).ToString()].Style.Numberformat.Format = "dd-mm-yyyy";
                    workSheet.Cells["D" + (i + 2).ToString()].Style.Numberformat.Format = "dd-mm-yyyy";
                }
                workSheet.Cells["A" + list.Count() + 1.ToString()].Value = "Ukupno: ";
                workSheet.Cells["A" + list.Count() + 2.ToString()].Value = list.Sum(a => a.TotalPrice) + "KM";
                package.Save();
            }
        }
    }

    internal class OrdersExcelVM
    {
        public int OrderID { get; set; }
        public int UserID { get; set; }
        public double OrderDate { get; set; }
        public double ShipDate { get; set; }
        public string UserInfo { get; set; }
        public double TotalPrice { get; set; }

    }
}
