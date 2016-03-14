using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NI.Apps.Hr.Service.Interface;
using NI.Apps.Hr.Entity;
using NI.Apps.Hr.Entity.Models;
using NI.Apps.Hr.BussinessLogic;
using NI.Apps.Hr.Repository;
using NI.Apps.Hr.Repository.Interface;

namespace NI.Apps.Hr.Service
{
    public class HeadCountService:IHeadCountService
    {
        HeadCountLogic bussiness = new HeadCountLogic();

        private IHeadCountRepository _headCountRepository;

        public IHeadCountRepository HeadCountRepository
        {
            get { return _headCountRepository ?? (_headCountRepository = new HeadCountRepository()); }
        }

        public int GetOfferQuota(int HcCode) {
            return this.HeadCountRepository.FindHeadCountByCode(HcCode).Headcount_Number??0;
        }
        public Table_Headcount AddHeadCount(HeadCount newHeadCount)
        {
            Table_Headcount hc = new Table_Headcount();
            hc.Headcount_Code = newHeadCount.Code;
            hc.Headcount_Position = newHeadCount.Position;
            hc.Headcount_Number = newHeadCount.Number;
            hc.Headcount_CostCenter = newHeadCount.CostCenter;
            hc.Headcount_Department = newHeadCount.Department;
            hc.Headcount_InternalLevel = newHeadCount.InternalLevel;
            hc.Headcount_CreatedAt = DateTime.Now;

            return bussiness.InsertNewHeadCount(hc);
            
        }
        public void UpdateHeadCount(Table_Headcount updatedHeadcount)
        {
            this.HeadCountRepository.Update(updatedHeadcount);
        }
        public Table_Headcount FindHeadCountByCode(int? code) {
            return this.HeadCountRepository.FindHeadCountByCode(code);
        }
        public Table_Headcount FindHeadCountByID(int id) {
            return this.HeadCountRepository.FindHeadCountByID(id);
        }
        public IEnumerable<HeadCount> FindHeadCounts(int? code, string position, string department) {
            return this.HeadCountRepository.FindHeadCountList(code,position,department);
        }

        public int GetIDByCode(int headcountCode) {
            return this.HeadCountRepository.GetId(headcountCode);
        }
    }
}
