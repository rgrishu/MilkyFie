using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Milkyfie.Models
{
    public class Employee
    {
        //[NotMapped]
        //public int Id { get; set; }
        public Guid EmployeeId { get; set; }
        public int Age { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public Guid CompanyId { get; set; }
    }
}
