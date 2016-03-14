using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NI.Apps.Hr.Entity;
using NI.Apps.Hr.Repository;
using NI.Apps.Hr.Repository.Interface;

namespace NI.Apps.Hr.BussinessLogic
{
    public class HeadCountLogic
    {
        IHeadCountRepository repo = new HeadCountRepository();
        public Table_Headcount InsertNewHeadCount(Table_Headcount newHC) {
            Table_Headcount tmp = repo.FindHeadCountByCode(newHC.Headcount_Code);

            if (tmp == null) {
                repo.Add(newHC);

                return newHC;
            }

            return tmp;
        }
    }
}
