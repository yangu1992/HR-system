using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NI.Apps.Hr.Repository.Interface;
using NI.Apps.Hr.Entity;

namespace NI.Apps.Hr.Repository
{
    public class WorkExperienceRepository:IWorkExperienceRepository
    {

        public int Add(Table_WorkExperienceInfo entity)
        {
            using (var db = new HrDbContext()) {
                entity.WorkExperienceInfo_CreatedAt = DateTime.Now;
                entity.WorkExperienceInfo_CreatedBy = "NIA Support";

                db.Table_WorkExperienceInfo.Add(entity);
                db.SaveChanges();

                return entity.WorkExperienceInfo_ID;
            }
        }
    }
}
