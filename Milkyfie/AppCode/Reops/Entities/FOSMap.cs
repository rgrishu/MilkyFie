
using Milkyfie.Models;

namespace Milkyfie.AppCode.Reops.Entities
{
    public class FOSMap
    {
        public int FOSMapID { get; set; }
        public ApplicationUser Users { get; set; }
        public Pincode pincode { get; set; }
        public string CreatedOn { get; set; }
    }
    public class FOSMapByUser
    {
        public int FOSMapID { get; set; }
        public ApplicationUser FOSUsers { get; set; }
        public int   UserID { get; set; }
        public string    MUserID { get; set; }
        public string Name { get; set; }
        public string CreatedOn { get; set; }
    }
}
