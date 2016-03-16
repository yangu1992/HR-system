using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NI.Apps.Hr.Service.Interface
{
    public interface ISpinnerService
    {
        IEnumerable<string> GetCostCenterList();
        IEnumerable<string> GetDepartmentList();
        IEnumerable<string> GetInternalLevelList();
        IEnumerable<string> GetOfferStatusList();
        IEnumerable<string> GetEmployeeList();
    }
}
