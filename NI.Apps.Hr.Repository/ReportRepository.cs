using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NI.Apps.Hr.Repository.Interface;
using NI.Apps.Hr.Entity;

namespace NI.Apps.Hr.Repository
{
    public class ReportRepository:IReportRepository
    {
        public int Add(Table_ReportingInfo entity)
        {
            using (var db = new HrDbContext()) {
                entity.ReportingInfo_CreatedAt = DateTime.Now;
                entity.ReportingInfo_CreatedBy = "NIA Support";

                db.Table_ReportingInfo.Add(entity);
                db.SaveChanges();

                return entity.ReportingInfo_ID;
            }
        }

        public Table_ReportingInfo FindReportByID(int? lineID)
        {
            using (var db = new HrDbContext())
            {
                var result = db.Table_ReportingInfo.Find(lineID);
                return result;
            }
        }
    }
}
