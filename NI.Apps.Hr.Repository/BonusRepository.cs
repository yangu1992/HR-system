using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NI.Apps.Hr.Repository.Interface;
using NI.Apps.Hr.Entity;

namespace NI.Apps.Hr.Repository
{
    public class BonusRepository:IBonusRepository
    {
        public void Add(Table_BonusInfo entity)
        {
            using (var db = new HrDbContext()) {
                entity.BonusInfo_CreatedAt = DateTime.Now;
                entity.BonusInfo_CreatedBy = "NIA Support";

                db.Table_BonusInfo.Add(entity);
                db.SaveChanges();
            }
        }

        public List<Table_BonusInfo> FindBySalaryID(int salaryID) {
            using (var db = new HrDbContext()) {
                var result= (from b in db.Table_BonusInfo
                         where b.BonusInfo_SalaryInfoID == salaryID
                         select b).ToList();

                if (result.Count == 0)
                    return null;

                return result;
            }
        }
    }
}
