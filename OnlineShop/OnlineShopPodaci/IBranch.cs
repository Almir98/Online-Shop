using OnlineShopPodaci.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShopPodaci
{
    public interface IBranch
    {
        //test
        List<Branch> GetAllBranches();

        void AddBranch(Branch branch);

        Branch GetBranchByID(int id);
        
        void DeleteBranch(int id);


    }
}
