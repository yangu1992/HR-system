using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NI.Apps.Hr.Repository.Interface;
using NI.Apps.Hr.Entity;

namespace NI.Apps.Hr.Repository
{
    public class FamilyMembersRepository:IFamilyMembersRepository
    {
        public int Add(Table_FamilyMembersInfo entity)
        {
            using (var db = new HrDbContext()) {
                entity.FamilyMembersInfo_CreatedAt = DateTime.Now;
                entity.FamilyMembersInfo_CreatedBy = "NIA Support";

                db.Table_FamilyMembersInfo.Add(entity);
                db.SaveChanges();

                return entity.FamilyMembersInfo_ID;
            }
        }
    }
}
