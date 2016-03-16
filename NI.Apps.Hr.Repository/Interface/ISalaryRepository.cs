using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NI.Apps.Hr.Entity;

namespace NI.Apps.Hr.Repository.Interface
{
    public interface ISalaryRepository
    {
        int Add(Table_SalaryInfo newInfo);

        Table_SalaryInfo FindByID(int? id);
    }
}
