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

    public class EmployeeService:IEmployeeService
    {
        private IEmployeeRepository _employeeRepository;

        public IEmployeeRepository EmployeeRepository
        {
            get { return _employeeRepository ?? (_employeeRepository = new EmployeeRepository()); }
        }
        public Table_Employee FindEmployeeByID(int? id)
        {
            return this.EmployeeRepository.FindEmployeeByID(id);
        }
    }
}
