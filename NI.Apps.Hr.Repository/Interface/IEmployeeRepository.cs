using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NI.Apps.Hr.Entity;
using NI.Apps.Hr.Entity.Models;

namespace NI.Apps.Hr.Repository.Interface
{
    public interface IEmployeeRepository
    {
        IEnumerable<string> GetEmployees();

        Table_Employee FindEmployeeByID(int? id);

        object GetEmployeeWithEmail();

        IEnumerable<Employee> FindEmployees(string ChineseName, string Department);

        void AddNewEmployee(int offerID);

        string getEmailPassword(string emailAddress);
    }
}
