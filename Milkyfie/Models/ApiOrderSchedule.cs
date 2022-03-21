namespace Milkyfie.Models
{

    public class ApiOrderReq
    {
        public string UserID { get; set; }
        public string CategoryID { get; set; }
        public string ProductID { get; set; }
    }




    public class ApiOrderSchedule
    {
        public string ScheduleID { get; set; }
        public string UserID { get; set; }
        public string Name { get; set; }
        public string CategoryID { get; set; }
        public string CategoryName { get; set; }

        public string FrequencyID { get; set; }
        public string FrequencyName { get; set; }

        public string ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public string SellingPrice { get; set; }
        public string MRP { get; set; }
        public string Quantity { get; set; }
        public string StartFromDate { get; set; }
        public string EndToDate { get; set; }
        public string Remark { get; set; }
        public string ScheduleShift { get; set; }
        public string Description { get; set; }
        public string CreatedOn { get; set; }
        public string Status { get; set; }
        public string StatusValue { get; set; }


        public string Sunday { get; set; }
        public string Monday { get; set; }
        public string Tuesday { get; set; }
        public string Wednesday { get; set; }
        public string Thursday { get; set; }
        public string Friday { get; set; }
        public string Saturday { get; set; }
        public string Weight { get; set; }

    }


    public class ApiOrderSummary
    {
        public int OrderID { get; set; }
        public string UserID { get; set; }
        public string Name { get; set; }
        public string OrderDate { get; set; }
        public int TotalItem { get; set; }
        public decimal TotalCost { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalGst { get; set; }
        public bool IsProcess { get; set; }
        public string Status { get; set; }
        public string StatusValue { get; set; }
    }
    public class APIOrderDetail
    {
        public int OrderID { get; set; }
        public int OrderDetailID { get; set; }
        public string ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public string SellingPrice { get; set; }
        public string MRP { get; set; }
        // public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Cost { get; set; }
        public decimal GST { get; set; }
        public decimal Discount { get; set; }
        public string CreatedOn { get; set; }
        public string OrderShift { get; set; }
        public string Remark { get; set; }
        public string CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string Status { get; set; }
        public string StatusValue { get; set; }
        public string Weight { get; set; }
    }
}
