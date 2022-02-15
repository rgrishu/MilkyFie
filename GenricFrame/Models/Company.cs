using LinqToDB.Mapping;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenricFrame.Models
{
    public class Company
    {
        //[NotMapped]
        //public int Id { get; set; }
        public Guid CompanyId { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string Name { get; set; }
    }
}
