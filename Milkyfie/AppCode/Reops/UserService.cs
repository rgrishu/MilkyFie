using Milkyfie.AppCode.Interfaces;
using Milkyfie.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Milkyfie.AppCode.Reops
{
    public class UserService : IUserService
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private List<ApplicationUser> _users = new List<ApplicationUser>
        {
            new ApplicationUser { Id = 1, UserName = "test", PasswordHash = "test" }
        };

        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public AuthenticateResponse Authenticate(LoginRequest model)
        {
            var user = _users.SingleOrDefault(x => x.UserName == model.UserName && x.PasswordHash == model.Password);
            var userinfo = new ApplicationUser()
            {
                Id = user.Id,
                UserName = user.UserName,
                Role = user.Role
            };
            // return null if user not found
            if (userinfo == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(userinfo);

            return new AuthenticateResponse(userinfo, token);
        }

        public IEnumerable<ApplicationUser> GetAll()
        {
            return _users;
        }

        public ApplicationUser GetById(int id)
        {
            return _users.FirstOrDefault(x => x.Id == id);
        }

        // helper methods

        private string generateJwtToken(ApplicationUser user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            /* Claims */
            var claims = new[] {
                new Claim("id", user.UserId.ToString()),
                new Claim("role", user.Role),
                new Claim("userName", user.UserName),
               // new Claim(JwtRegisteredClaimNames.Sub, userInfo.Username),
               // new Claim(JwtRegisteredClaimNames.Email, userInfo.EmailAddress),
               // new Claim("DateOfJoing", userInfo.DateOfJoing.ToString("yyyy-MM-dd")),
               // new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            /* End Claims */
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                //Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}