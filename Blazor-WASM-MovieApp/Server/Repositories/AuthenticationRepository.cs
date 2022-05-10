using Blazor_WASM_MovieApp.Data;
using Blazor_WASM_MovieApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace Blazor_WASM_MovieApp.Repositories
{
    public class AuthenticationRepository
    {

        private readonly BlazorMovieContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthenticationRepository(BlazorMovieContext dbContext, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public List<IdentityUser> GetUsers()
        {
            return _dbContext.Users.ToList();
        }

        public IdentityUser GetUser(string id)
        {
            IdentityUser user = _dbContext.Users.Find(id);

            return user; 
        }

        public async void AddUser(IdentityUser user, string role)
        {
            _dbContext.Users.Add(user);
            

            var userStore = new UserStore<IdentityUser>(_dbContext);
            await userStore.AddToRoleAsync(user,role);

            _dbContext.SaveChanges();

        }

        public async void UpdateUser(IdentityUser user, AuthInput authInput)
        {
            if(authInput.Password != null && authInput.Password != "")
            {
                var passwordhasher = new PasswordHasher<IdentityUser>();
                var hashed = passwordhasher.HashPassword(user, authInput.Password);
                user.PasswordHash = hashed;
            }
           
            _dbContext.Users.Update(user);
            _dbContext.SaveChanges();
        }

        public void DeleteUser(IdentityUser user)
        {
            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();

            return;
        }
    }
}
