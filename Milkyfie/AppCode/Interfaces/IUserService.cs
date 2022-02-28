using Milkyfie.Models;
using System.Collections.Generic;

namespace Milkyfie.AppCode.Interfaces
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(LoginRequest model);
        IEnumerable<ApplicationUser> GetAll();
        ApplicationUser GetById(int id);
    }
}
