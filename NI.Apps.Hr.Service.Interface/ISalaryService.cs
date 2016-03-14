using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NI.Apps.Hr.Entity;

namespace NI.Apps.Hr.Service.Interface
{
    public interface ISalaryService
    {
        Table_SalaryInfo FindSalaryByID(int? id);
    }
}
