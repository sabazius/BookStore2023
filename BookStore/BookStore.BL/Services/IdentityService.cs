using BookStore.BL.Interfaces;
using BookStore.Models.Configurations.Identity;
using BookStore.Models.Models.Users;
using BookStore.Models.Responses;
using Microsoft.AspNetCore.Identity;

namespace BookStore.BL.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly JwtSettings _jwtSettings;

        public Task<AuthenticationResult> RegisterAsync(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public Task<AuthenticationResult> LoginAsync(string userName, string password)
        {
            throw new NotImplementedException();
        }
    }
}
