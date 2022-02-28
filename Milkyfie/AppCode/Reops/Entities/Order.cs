using System;
using System.ComponentModel.DataAnnotations;

namespace Milkyfie.AppCode.Reops.Entities
{
    public class OrderSchedule
    {
        public int ScheduleID { get; set; }
        public int UserID { get; set; }
        public int LoginID { get; set; }
        public Product Product { get; set; }
        public Category Category { get; set; }
        public string Frequency { get; set; }
        public string FrequencyValue { get; set; }
        public decimal Quantity { get; set; }
        public Unit Unit { get; set; }
        public DateTime StartFromDate { get; set; }
        public DateTime EndToDate { get; set; }
        public string Remark { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string CreatedOn { get; set; }
        public string UpdatedOn { get; set; }
    }
}
