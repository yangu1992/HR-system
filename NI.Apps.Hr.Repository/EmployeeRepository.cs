using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NI.Apps.Hr.Repository.Interface;
using NI.Apps.Hr.Entity;

namespace NI.Apps.Hr.Repository
{
    public class EmployeeRepository:IEmployeeRepository
    {
        public IEnumerable<string> GetEmployees()
        {
            using (var db = new HrDbContext())
            {
                List<string> result = (from e in db.Table_Employee
                                       select e.Employee_FullName).ToList();

                return result;
            }
        }

        public int GetID(string fullName)
        {
            using (var db = new HrDbContext())
            {
                var id = (from e in db.Table_Employee
                          where e.Employee_FullName == fullName
                          select e.Employee_ID).First();

                return id;
            } 
        }

        public Table_Employee FindEmployeeByID(int? id) {
            using (var db = new HrDbContext())
            {
                var entity = (from e in db.Table_Employee
                          where e.Employee_ID == id
                          select e).First();

                return entity;
            } 
        }
    }
}
