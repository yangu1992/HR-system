using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NI.Apps.Hr.Entity;
using NI.Apps.Hr.Entity.Models;

namespace NI.Apps.Hr.Service.Interface
{
    public interface IEmployeeService
    {
        Table_Employee FindEmployeeByID(int? id);

        IEnumerable<Employee> FindEmployees(string ChineseName, string Department);

    }
}
