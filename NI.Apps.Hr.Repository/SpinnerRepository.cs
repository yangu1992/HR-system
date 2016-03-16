using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NI.Apps.Hr.Repository.Interface;
using NI.Apps.Hr.Entity;

namespace NI.Apps.Hr.Repository
{
    public class SpinnerRepository : ISpinnerRepository
    {
        public IEnumerable<string> GetCostCenterList()
        {
            using (var db = new HrDbContext())
            {
                List<string> result = (from l in db.Table_CostCenter
                                           select l.CostCenter_Name).ToList();

                return result;
            }
        }

        public IEnumerable<string> GetDepartmentList()
        {
            using (var db = new HrDbContext())
            {
                List<string> result = (from l in db.Table_Department
                                       select l.Department_Name).ToList();

                return result;
            }
        }

        public IEnumerable<string> GetInternalLevelList()
        {
            using (var db = new HrDbContext())
            {
                List<string> result = (from l in db.Table_InternalLevel
                                       select l.InternalLevel_Name).ToList();

                return result;
            }
        }

        public IEnumerable<string> GetOfferStatusList() {
            using (var db = new HrDbContext())
            {
                List<string> result = (from l in db.Table_Offer
                                       select l.Offer_Status).Distinct().ToList();

                return result;
            }
        }
    }
}
