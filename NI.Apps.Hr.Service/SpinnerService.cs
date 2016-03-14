using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NI.Apps.Hr.Service.Interface;
using NI.Apps.Hr.Repository;
using NI.Apps.Hr.Repository.Interface;

namespace NI.Apps.Hr.Service
{
    public class SpinnerService : ISpinnerService
    {
        private ISpinnerRepository _spinnerRepository;
        private IEmployeeRepository _employeeRepository;

        public ISpinnerRepository SpinnerRepository {
            get { return _spinnerRepository ?? (_spinnerRepository = new SpinnerRepository()); }
        }

        public IEmployeeRepository EmployeeRepository {
            get { return _employeeRepository ?? (_employeeRepository = new EmployeeRepository()); }
        }
        public IEnumerable<string> GetCostCenterList(){
            return this.SpinnerRepository.GetCostCenterList();
        }
        public IEnumerable<string> GetDepartmentList(){
            return this.SpinnerRepository.GetDepartmentList();
        }
        public IEnumerable<string> GetInternalLevelList(){
            return this.SpinnerRepository.GetInternalLevelList();
        }

        public IEnumerable<string> GetOfferStatusList()
        {
            return this.SpinnerRepository.GetOfferStatusList();
        }

        public IEnumerable<string> GetEmployeeList()
        {
            return this.EmployeeRepository.GetEmployees();
        }
    }
}
