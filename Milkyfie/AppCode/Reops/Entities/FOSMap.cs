
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
}
