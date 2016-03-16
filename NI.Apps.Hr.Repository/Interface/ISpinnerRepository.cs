using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NI.Apps.Hr.Repository.Interface
{
    public interface ISpinnerRepository
    {
        IEnumerable<string> GetCostCenterList();
        IEnumerable<string> GetDepartmentList();
        IEnumerable<string> GetInternalLevelList();
        IEnumerable<string> GetOfferStatusList();
    }
}
