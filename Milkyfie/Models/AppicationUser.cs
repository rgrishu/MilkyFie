using Microsoft.AspNetCore.Identity;

namespace GenricFrame.Models
{
    public class AppicationUser:IdentityUser<int>
    {
        public string UserId { get; set; }
        public string Role { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Pincode { get; set; }
    }
}
