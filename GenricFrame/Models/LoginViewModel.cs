using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GenricFrame.Models
{
    public class LoginViewModel
    {
        [Required]

        [Phone]
        public string MobileNo { get; set; }
        //[EmailAddress]
        //public string EmailId { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
