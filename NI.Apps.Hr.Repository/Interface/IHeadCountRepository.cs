using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NI.Apps.Hr.Entity;
using NI.Apps.Hr.Entity.Models;

namespace NI.Apps.Hr.Repository.Interface
{
    public interface IHeadCountRepository
    {
        Table_Headcount FindHeadCountByCode(int? code);

        IEnumerable<HeadCount> FindHeadCountList(int? code,string position,string department);

        int Add(Table_Headcount entity);

        int GetId(int headcountCode);

        Table_Headcount FindHeadCountByID(int id);

        void Update(Table_Headcount entity);
    }
}
