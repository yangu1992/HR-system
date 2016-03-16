using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NI.Apps.Hr.Repository.Interface;
using NI.Apps.Hr.Entity;

namespace NI.Apps.Hr.Repository
{
    public class SalaryRepository:ISalaryRepository
    {
        public int Add(Table_SalaryInfo entity)
        {
            using (var db = new HrDbContext())
            {
                entity.SalaryInfo_CreatedAt = DateTime.Now;
                entity.SalaryInfo_CreatedBy = "NIA Support";

                db.Table_SalaryInfo.Add(entity);
                db.SaveChanges();

                return entity.SalaryInfo_ID;
            }   
        }

        public Table_SalaryInfo FindByID(int? id) {
            using (var db = new HrDbContext()) {
                var result = db.Table_SalaryInfo.Find(id);
                return result;
            }
        }
    }
}
