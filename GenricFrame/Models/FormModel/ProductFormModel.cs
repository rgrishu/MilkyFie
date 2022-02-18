namespace GenricFrame.Models.FormModel
{
    public class ProductFormModel
    {
      
        public int CatgoryID  { get; set; }  
        public int UnitID  { get; set; } 
        public string ProductName  { get; set; } 
        public string Quantity { get; set; }
        public decimal MRP { get; set; }
        public decimal SellingPrice { get; set; }
        public string DiscountType { get; set; }
        public decimal Discount { get; set; }
        public bool IsDiscount { get; set; }
        public string ProductImage { get; set; }
        public string ProductIcon { get; set; }
        public string Description { get; set; }
        public string Remark { get; set; }
    }
}
