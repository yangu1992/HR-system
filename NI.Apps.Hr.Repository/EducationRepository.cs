using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NI.Apps.Hr.Repository.Interface;
using NI.Apps.Hr.Entity;

namespace NI.Apps.Hr.Repository
{
    public class EducationRepository:IEducationRepository
    {

        public int Add(Table_EducationInfo entity)
        {
            using (var db = new HrDbContext()) {
                entity.EducationInfo_CreatedAt = DateTime.Now;
                entity.EducationInfo_CreatedBy = "NIA Support";

                db.Table_EducationInfo.Add(entity);
                db.SaveChanges();

                return entity.EducationInfo_ID;
            }
        }
    }
}
