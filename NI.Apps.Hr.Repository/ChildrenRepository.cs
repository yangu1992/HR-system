using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NI.Apps.Hr.Repository.Interface;
using NI.Apps.Hr.Entity;

namespace NI.Apps.Hr.Repository
{
    public class ChildrenRepository:IChildrenRepository
    {

        public int Add(Table_ChildrenInfo entity)
        {
            using (var db = new HrDbContext()) {
                entity.ChildrenInfo_CreatedAt = DateTime.Now;
                entity.ChildrenInfo_CreatedBy = "NIA Support";

                db.Table_ChildrenInfo.Add(entity);
                db.SaveChanges();

                return entity.ChildrenInfo_ID;
            }
        }
    }
}
