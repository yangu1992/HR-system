using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NI.Apps.Hr.Entity;

namespace NI.Apps.Hr.Repository.Interface
{
    public interface IEmployeeRepository
    {
        IEnumerable<string> GetEmployees();

        Table_Employee FindEmployeeByID(int? id);
    }
}
