using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NI.Apps.Hr.Entity;
using NI.Apps.Hr.Repository;
using NI.Apps.Hr.Repository.Interface;
using NI.Apps.Hr.Service.Interface;

namespace NI.Apps.Hr.Service
{
      
    public class SalaryService:ISalaryService
    {
        private  ISalaryRepository _salaryRepository;
        private IBonusRepository _bonusRepository;
        public ISalaryRepository SalaryRepository
        {
         get { return _salaryRepository ?? (_salaryRepository = new SalaryRepository()); }
        }
        public IBonusRepository BonusRepository
        {
            get { return _bonusRepository ?? (_bonusRepository = new BonusRepository()); }
        }
        public Table_SalaryInfo FindSalaryByID(int? id)
        {
            return this.SalaryRepository.FindByID(id);
        }

        public List<Table_BonusInfo> FindBonusBySalaryID(int salaryID) {
            return this.BonusRepository.FindBySalaryID(salaryID);
        }
    }
}
