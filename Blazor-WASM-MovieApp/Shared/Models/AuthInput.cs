using Microsoft.AspNetCore.Identity;

namespace Blazor_WASM_MovieApp.Models
{
    public class AuthInput
    {
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string? Role { get; set; }
        public bool IsAuthSuccessful { get; set; }
        public string ErrorMessage { get; set; }
        public string Token { get; set; }
        public IdentityUser? User { get; set; }


    }
}
