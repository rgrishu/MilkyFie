using System.ComponentModel.DataAnnotations;

namespace Milkyfie.Models
{
    public class StatusChangeReq
    {
      
        public int ID { get; set; }
        public int Quantity { get; set; }
        public string Remark { get; set; }
        [Required]
        public Status Status { get; set; }

    }
}
