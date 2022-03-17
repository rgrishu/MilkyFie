namespace Milkyfie.Models
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }        
        public string Username { get; set; }
        public string Token { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Pincode { get; set; }
        public string Role { get; set; }


        public AuthenticateResponse(ApplicationUser user, string token)
        {
            Id = user.Id;
            Role = user.Role;
            Username = user.UserName;
            Token = token;
            PhoneNumber = user.PhoneNumber;
            Email = user.Email;
            Name = user.Name;
            Address = user.Address;
            Pincode = user.Pincode;
        }
    }
}
