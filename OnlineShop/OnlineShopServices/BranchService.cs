using OnlineShopPodaci;
using OnlineShopPodaci.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace OnlineShopServices
{
    public class BranchService : IBranch
    {
        protected OnlineShopContext _context;

        public BranchService(OnlineShopContext context)
        {
            _context = context;
        }

        public List<Branch> GetAllBranches()
        {
            return _context.branch.Include(e => e.City).ToList();
        }

        public Branch GetBranchByID(int id)
        {
            return GetAllBranches().Where(e => e.BranchID == id).FirstOrDefault();
        }

        public void AddBranch(Branch branch)
        {
            _context.Add(branch);
            _context.SaveChanges();
        }

        public void DeleteBranch(int id)
        {
            _context.Remove(GetBranchByID(id));
            _context.SaveChanges();
        }
    }
}
