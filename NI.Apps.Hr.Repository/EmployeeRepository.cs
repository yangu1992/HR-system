using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NI.Apps.Hr.Repository.Interface;
using NI.Apps.Hr.Entity;
using NI.Apps.Hr.Entity.Models;
using System.Data.Entity.Core.Objects;

namespace NI.Apps.Hr.Repository
{
    public class EmployeeRepository:IEmployeeRepository
    {
        public IEnumerable<Employee> FindEmployees(string Name, string Dep)
        {
            using (var db = new HrDbContext())
            {
                List<Employee> result = (from e in db.Table_Employee
                                         where (Name==null || e.Employee_FullName.Contains(Name))
                                         && (Dep == null || e.Employee_Department == Dep)
                                         select new Employee()
                                         {
                                             Id = e.Employee_ID,
                                             Name = e.Employee_LocalName,
                                             EngName=e.Employee_FullName,
                                             Dep=e.Employee_Department,
                                             JobTitle=e.Employee_JobTitle,
                                             Ext="",
                                             Cube="",
                                             OfficeEmail=e.Employee_Email,
                                             Address="",
                                             HomeTel="",
                                             Mobile="",
                                             Status="Active"
                                         }).ToList();

                return result;
            }
        }

        public object GetEmployeeWithEmail()
        {
            using (var db = new HrDbContext()) {
                var result = from e in db.Table_Employee
                             select new { e.Employee_FullName, e.Employee_Email };

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

        public IEnumerable<string> GetEmployees()
        {
            throw new NotImplementedException();
        }




        public void AddNewEmployee(int offerID)
        {
            using (var db=new HrDbContext()){
                ObjectParameter p = new ObjectParameter("returnVal", typeof(int));
                db.Proc_InsertNewEmployee(p,offerID,"NIA Support");
            }

            
        }


        public string getEmailPassword(string emailAddress)
        {

            using (var db = new HrDbContext())
            {
                ObjectParameter p = new ObjectParameter("returnVal", typeof(string));
                db.Proc_GetEmailAccountPwd(p, emailAddress);
                string pwd = (p.Value == null) ? string.Empty : p.Value.ToString();

                return pwd;
            }
        }
    }
}
