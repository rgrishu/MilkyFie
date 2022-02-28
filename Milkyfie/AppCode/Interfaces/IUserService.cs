using GenricFrame.Models;
using System.Collections.Generic;

namespace GenricFrame.AppCode.Interfaces
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(LoginRequest model);
        IEnumerable<AppicationUser> GetAll();
        AppicationUser GetById(int id);
    }
}
