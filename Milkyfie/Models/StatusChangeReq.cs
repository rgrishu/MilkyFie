using System.ComponentModel.DataAnnotations;

namespace Milkyfie.Models
{
    public class StatusChangeReq
    {
        [Required]
        public int ID { get; set; }
        [Required(ErrorMessage ="Remark Required") ]
       // [StringLength(8, ErrorMessage = "Remark Requied")]
        public string Remark { get; set; }
        [Required]
        public Status Status { get; set; }

    }
}
