using GenricFrame.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace GenricFrame.AppCode.Data
{
    public class UserStore : IUserStore<AppicationUser>, IUserPasswordStore<AppicationUser>, IUserEmailStore<AppicationUser>, IUserRoleStore<AppicationUser>, IQueryableUserStore<AppicationUser>
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<ApplicationRole> _roleManager;
        public UserStore(ApplicationDbContext context, RoleManager<ApplicationRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }

        public IQueryable<AppicationUser> Users => _context.Users();

        public Task AddToRoleAsync(AppicationUser user, string roleName, CancellationToken cancellationToken)
        {
            int roleid = _roleManager.FindByNameAsync(roleName).Result.Id;
            _context.AddToRoleAsync(user.Id, roleid);
            return Task.FromResult(0);
        }

        public Task<IdentityResult> CreateAsync(AppicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(_context.AddUser(user));
        }

        public Task<IdentityResult> DeleteAsync(AppicationUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public Task<AppicationUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            var user = _context.FindByEmailAsync(normalizedEmail);
            return user;
        }

        public Task<AppicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var user = _context.FindByIdAsync(userId);
            return user;
        }

        public Task<AppicationUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var user = _context.FindByNameAsync(normalizedUserName);
            return user;
        }

        public Task<string> GetEmailAsync(AppicationUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(AppicationUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task<string> GetNormalizedEmailAsync(AppicationUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.NormalizedEmail);
        }

        public Task<string> GetNormalizedUserNameAsync(AppicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(_context.GetNormalizedUserName(user));
        }

        public Task<string> GetPasswordHashAsync(AppicationUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.PasswordHash);
        }

        public Task<IList<string>> GetRolesAsync(AppicationUser user, CancellationToken cancellationToken)
        {
            var userRoles = (IList<string>)_context.GetRolesAsync(user).Result;
            return Task.FromResult(userRoles);
        }

        public Task<string> GetUserIdAsync(AppicationUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(AppicationUser user, CancellationToken cancellationToken)
        {

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.UserName);
        }

        public Task<IList<AppicationUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasPasswordAsync(AppicationUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsInRoleAsync(AppicationUser user, string roleName, CancellationToken cancellationToken)
        {
            int roleid = _roleManager.FindByNameAsync(roleName).Result.Id;
            return _context.IsInRoleAsync(user, roleid);
        }

        public Task RemoveFromRoleAsync(AppicationUser user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetEmailAsync(AppicationUser user, string email, CancellationToken cancellationToken)
        {
            user.Email = email;
            return Task.FromResult(0);

        }

        public Task SetEmailConfirmedAsync(AppicationUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public Task SetNormalizedEmailAsync(AppicationUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.NormalizedEmail = normalizedEmail;
            return Task.FromResult(0);
        }

        public Task SetNormalizedUserNameAsync(AppicationUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.FromResult(0);

        }

        public Task SetPasswordHashAsync(AppicationUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public Task SetUserNameAsync(AppicationUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.FromResult(0);

        }

        public Task<IdentityResult> UpdateAsync(AppicationUser user, CancellationToken cancellationToken)
        {
            return _context.UpdateUser(user);
        }
    }
}
