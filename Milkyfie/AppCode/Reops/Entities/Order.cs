using Milkyfie.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Milkyfie.AppCode.Reops.Entities
{
    public class OrderSchedule
    {
        public int ScheduleID { get; set; }
      
        public int LoginID { get; set; }
      
     
        public string OtherFrequency { get; set; }
        public decimal Quantity { get; set; }
      
        public string StartFromDate { get; set; }
        public string EndToDate { get; set; }
        public string Remark { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string CreatedOn { get; set; }
        public string UpdatedOn { get; set; }
        public ApplicationUser User { get; set; }
        public Frequency Frequency { get; set; }
        public Product Product { get; set; }
        public Unit Unit { get; set; }
        public Category Category { get; set; }
    }
}
