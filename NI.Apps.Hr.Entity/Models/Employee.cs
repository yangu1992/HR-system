using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NI.Apps.Hr.Entity.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string EngName { get; set; }
        public string Dep { get; set; }
        public string JobTitle { get; set; }
        public string Ext { get; set; }
        public string Cube { get; set; }
        public string OfficeEmail { get; set; }
        public string Address { get; set; }
        public string HomeTel { get; set; }
        public string Mobile { get; set; }
        public string Status { get; set; }
    }
}