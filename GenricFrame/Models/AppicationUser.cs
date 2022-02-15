using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenricFrame.Models
{
    public class AppicationUser:IdentityUser<int>
    {
        public string UserId { get; set; }
        //public string UserName { get; set; }
        public string Role { get; set; }
    
        public string Name { get; set; }
        public string Address { get; set; }
        public string Pincode { get; set; }
        //[JsonIgnore]
        //public string Password { get; set; }
        //public string EmailAddress { get; set; }
        //public DateTime DateOfJoing { get; set; }
    }
}
