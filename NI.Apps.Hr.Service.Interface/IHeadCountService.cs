using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NI.Apps.Hr.Entity;
using NI.Apps.Hr.Entity.Models;

namespace NI.Apps.Hr.Service.Interface
{
    public interface IHeadCountService
    {
        int GetOfferQuota(int HcCode);
        Table_Headcount AddHeadCount(HeadCount newHeadCount);
        Table_Headcount FindHeadCountByCode(int? HcCode);
        IEnumerable<HeadCount> FindHeadCounts(int? code, string position, string department);
        int GetIDByCode(int HcCode);
        Table_Headcount FindHeadCountByID(int id);
        void UpdateHeadCount(Table_Headcount updatedHeadcount);
    }
}
