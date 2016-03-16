using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NI.Apps.Hr.Repository.Interface;
using NI.Apps.Hr.Entity;
using NI.Apps.Hr.Entity.Models;

namespace NI.Apps.Hr.Repository
{
    public class HeadCountRepository : IHeadCountRepository
    {
        public Table_Headcount FindHeadCountByCode(int? code)
        {
            using (var db = new HrDbContext())
            {
                var result = (from l in db.Table_Headcount
                          where (code==null || l.Headcount_Code == code)
                          select l).FirstOrDefault();

                return result;
            }       
        }
        public Table_Headcount FindHeadCountByID(int id) {
            using (var db = new HrDbContext())
            {
                var result = db.Table_Headcount.Find(id); 
                return result;
            }  
        }

        public IEnumerable<HeadCount> FindHeadCountList(int? code, string position, string department) { 
            using (var db = new HrDbContext())
            {
                var result = (from h in db.Table_Headcount
                              where (code==null || h.Headcount_Code==code)
                              && (string.IsNullOrEmpty(position) || h.Headcount_Position==position)
                              && (string.IsNullOrEmpty(department) || h.Headcount_Department==department)
                              select new HeadCount()
                              {
                                  Code = h.Headcount_Code,
                                  Position = h.Headcount_Position,
                                  Number = h.Headcount_Number??0,         //有点奇怪
                                  CostCenter = h.Headcount_CostCenter,
                                  Department = h.Headcount_Department,
                                  InternalLevel = h.Headcount_InternalLevel
                              }).ToList();                   

                return result;
            }
        }

        public int Add(Table_Headcount entity) 
        {
            using (var db = new HrDbContext())
            {
                entity.Headcount_CreatedAt = DateTime.Now;
                entity.Headcount_CreatedBy = "NIA Support";

                db.Table_Headcount.Add(entity);
                db.SaveChanges();

                return entity.Headcount_ID;
            }     
        }

        public int GetId(int headcountCode) {
            using (var db = new HrDbContext())
            {
                var result = (from h in db.Table_Headcount
                              where h.Headcount_Code == headcountCode
                              select h.Headcount_ID).First();

                return result;
            }  
        }

        public void Update(Table_Headcount entity) {
            using (var db = new HrDbContext()) {
                Table_Headcount hc = (from h in db.Table_Headcount
                              where h.Headcount_ID == entity.Headcount_ID
                              select h).First();

                hc.Headcount_Code = entity.Headcount_Code;
                hc.Headcount_Position = entity.Headcount_Position;
                hc.Headcount_Number = entity.Headcount_Number;
                hc.Headcount_CostCenter = entity.Headcount_CostCenter??hc.Headcount_CostCenter;
                hc.Headcount_Department = entity.Headcount_Department??hc.Headcount_Department;
                hc.Headcount_InternalLevel = entity.Headcount_InternalLevel??hc.Headcount_InternalLevel;
                hc.Headcount_ModifiedAt = DateTime.Now;
                hc.Headcount_ModifiedBy = "NIA Support";

                db.SaveChanges();
            }
        }
    }
}
