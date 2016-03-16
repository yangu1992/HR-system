using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NI.Apps.Hr.Entity;

namespace NI.Apps.Hr.Service.Interface
{
    public interface IReportService
    {
        Table_ReportingInfo FindReportLineByID(int? lineID);
    }
}
