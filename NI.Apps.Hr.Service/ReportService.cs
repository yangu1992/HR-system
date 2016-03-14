using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NI.Apps.Hr.Service.Interface;
using NI.Apps.Hr.Entity;
using NI.Apps.Hr.Repository;
using NI.Apps.Hr.Repository.Interface;

namespace NI.Apps.Hr.Service
{
    public class ReportService:IReportService
    {
        private IReportRepository _reportRepository;

        public IReportRepository ReportRepository
        {
            get { return _reportRepository ?? (_reportRepository = new ReportRepository()); }
        }
        public Table_ReportingInfo FindReportLineByID(int? lineID)
        {
            return this.ReportRepository.FindReportByID(lineID);
        }
    }
}
