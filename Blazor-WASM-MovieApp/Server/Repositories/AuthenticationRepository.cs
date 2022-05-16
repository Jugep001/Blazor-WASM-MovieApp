using Blazor_WASM_MovieApp.Data;
using Blazor_WASM_MovieApp.Exceptions;
using Blazor_WASM_MovieApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using Blazor_WASM_MovieApp.Shared.Models;
using Microsoft.AspNetCore.Identity;
using System.Text;

namespace Blazor_WASM_MovieApp.Repositories
{
    public class AuthenticationRepository
    {

        private readonly BlazorMovieContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthenticationRepository(BlazorMovieContext dbContext, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration iconfig)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = iconfig;
        }

        private async Task<List<Claim>> GetClaims(IdentityUser user)
        {
            var claims = new List<Claim>
            {
                 new Claim(ClaimTypes.Name, user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
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

        public async Task Register(AuthInput authInput)
        {
            var user = new IdentityUser
            {
                Email = authInput.Email,
                NormalizedEmail = authInput.Email,
                UserName = authInput.Username,
                NormalizedUserName = authInput.Username,
                PhoneNumber = authInput.PhoneNumber,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };
            var passwordhasher = new PasswordHasher<IdentityUser>();
            var hashed = passwordhasher.HashPassword(user, authInput.Password);
            user.PasswordHash = hashed;

            var result = await _userManager.CreateAsync(user, authInput.Password);

            if (!result.Succeeded)
            {
                throw new Exception();
            }
            var roleResult = await _userManager.AddToRoleAsync(user, authInput.Role);
            if (!roleResult.Succeeded)
            {
                throw new Exception();
            }

        }

        public async Task UpdateUser(AuthInput authInput)
        {
            if (authInput.Password != null && authInput.Password != "")
            {
                var passwordhasher = new PasswordHasher<IdentityUser>();
                var hashed = passwordhasher.HashPassword(authInput.User, authInput.Password);
                authInput.User.PasswordHash = hashed;
            }

            _dbContext.Users.Update(authInput.User);
            _dbContext.SaveChanges();
        }

        public async Task DeleteUser(string id)
        {
            var user = GetUser(id);
            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();
            
        }

        public async Task<AuthInput> Login(AuthInput authInput)
        {
            var user = await _userManager.FindByNameAsync(authInput.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, authInput.Password))
            {
                List<ErrorItem> errors = new List<ErrorItem>();
                errors.Add(new ErrorItem("Identity", "Invalid Login"));
                throw new BusinessException(errors);
            }

            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims(user);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            try
            {
                var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                authInput.Token = token;
                authInput.Email = user.Email;
                authInput.Username = user.UserName;
                return authInput;
            }
            catch (ArgumentNullException ex)
            {
                throw ex.InnerException;
            }
            catch (ArgumentException ex)
            {
                throw ex.InnerException;
            }
            catch (SecurityTokenEncryptionFailedException ex)
            {
                throw ex.InnerException;
            }
            return null;

        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes("MySuperSecretKey");
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var tokenOptions = new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("JWTSettings:validIssuer"),
                audience: _configuration.GetValue<string>("JWTSettings:validAudience"),
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration.GetValue<string>("JWTSettings:expiryInMinutes"))),
                signingCredentials: signingCredentials);

            return tokenOptions;
        }


    }
}
