using Milkyfie.Models;
using Microsoft.AspNetCore.Http;

namespace Milkyfie.AppCode.Reops.Entities
{
    public class Product 
    {
        public int ProductID { get; set; }
       // public int CategoryID { get; set; }
        
     //   public int UnitID { get; set; }
        
        public string ProductName { get; set; }
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
        public Category Category { get; set; }
        public Unit Unit { get; set; }

    }
    //public class ProductCategory
    //{
    //    public int CategoryID { get; set; }
    //    public int CategoryName { get; set; }
    //    public Parent Parent { get; set; }
    //    public ProductDetails ProductDetails { get; set; }
    //}
    //public class ProductDetails
    //{
    //    public string ProductName { get; set; }

    //}

}
