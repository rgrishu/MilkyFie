using System.ComponentModel.DataAnnotations;

namespace Milkyfie.AppCode.Reops.Entities
{
    public class Category
    {
        public int CategoryID { get; set; }
        //public int ParentID { get; set; }
       
       
        public string CategoryName { get; set; }
        //public string ParentName { get; set; }
        public string Icon { get; set; }
        public bool IsActive { get; set; }
        public Parent Parent { get; set; }
    }
    public class Parent
    {
        public int ParentID { get; set; }
        public string ParentCategory { get; set; }
    }
}
