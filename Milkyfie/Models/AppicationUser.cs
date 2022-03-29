using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Milkyfie.Models
{
    public class ApplicationUser:IdentityUser<int>
    {
        public string UserId { get; set; }
        public string Role { get; set; }
        public string MUserId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Pincode { get; set; }
        public decimal Balance { get; set; }
    }


    public class UserInfoApi
    {
        public string UserId { get; set; }
        public string MUserId { get; set; }
        public string Role { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Pincode { get; set; }
        public decimal Balance { get; set; }
    }
}
