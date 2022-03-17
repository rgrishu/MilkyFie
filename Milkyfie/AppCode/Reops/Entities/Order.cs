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
        public int Quantity { get; set; }
        public string StartFromDate { get; set; }
        public string EndToDate { get; set; }
        public string Remark { get; set; }
        public string ScheduleShift { get; set; }
        public string Description { get; set; }
        public string CreatedOn { get; set; }
        public string UpdatedOn { get; set; }
        public bool IsActive { get; set; }
        public ApplicationUser User { get; set; }
        public Frequency Frequency { get; set; }
        public Product Product { get; set; }
        public Unit Unit { get; set; }
        public Category Category { get; set; }
        public Status Status { get; set; }
    }
    public class OrderSummary
    {
        public int OrderID { get; set; }
        public string OrderDate { get; set; }
        public int TotalItem { get; set; }
        public decimal TotalCost { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalGst { get; set; }
        public bool IsProcess { get; set; }
        public ApplicationUser User { get; set; }
        public Status Status { get; set; }
    }
    public class OrderDetail
    {
        public int OrderDetailID { get; set;}
        public int Quantity { get; set; }
        public decimal Cost { get; set; }
        public decimal GST { get; set; }
        public decimal Discount { get; set; }
        public string CreatedOn { get; set; }
        public string OrderShift { get; set; }
        public string Remark { get; set; }
        public OrderSummary OrderSummary { get; set; }
        public OrderSchedule Schedule { get; set; }
        public Product Product { get; set; }
        public Category Category { get; set; }
        public Status Status { get; set; }
    }
}
