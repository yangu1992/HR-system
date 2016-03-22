using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NI.Apps.Hr.Entity;

namespace NI.Apps.Hr.Repository.Interface
{
    public interface IBonusRepository
    {
        void Add(Table_BonusInfo entity);

        List<Table_BonusInfo> FindBySalaryID(int salaryID);
    }
}
