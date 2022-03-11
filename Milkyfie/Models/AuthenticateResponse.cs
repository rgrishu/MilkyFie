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


        public AuthenticateResponse(ApplicationUser user, string token)
        {
            Id = user.Id;
            Username = user.UserName;
            Token = token;
            PhoneNumber = user.PhoneNumber;
            Email = user.Email;
            Name = user.Name;
        }
    }
}
