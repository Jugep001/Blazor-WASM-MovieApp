using Blazor_WASM_MovieApp.Client.AuthProviders;
using Blazor_WASM_MovieApp.Client.Services.Interfaces;
using Blazor_WASM_MovieApp.Exceptions;
using Blazor_WASM_MovieApp.Models;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Blazor_WASM_MovieApp.Client.Services
{
    public class WASM_AuthenticationService : WASM_IAuthenticationService
    {

        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorage;

        public WASM_AuthenticationService(HttpClient httpClient, AuthenticationStateProvider authStateProvider, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _authStateProvider = authStateProvider;
        }

        public async Task Register(AuthInput authInput)
        {
            string json = JsonConvert.SerializeObject(authInput, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            var response = await _httpClient.PostAsJsonAsync("/Register", json);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }
        }

        public async Task<AuthInput> Login(AuthInput authInput)
        {
            string json = JsonConvert.SerializeObject(authInput, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            var response = await _httpClient.PostAsJsonAsync("/Login", json);
            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<AuthInput>(responseContent);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }
            await _localStorage.SetItemAsync("authToken", result.Token);
            ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(result.Token);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token);
            return new AuthInput { IsAuthSuccessful = true };

        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((AuthStateProvider)_authStateProvider).NotifyUserLogout();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<List<IdentityUser>> GetUsers()
        {
            var json = await _httpClient.GetStringAsync($"/GetUsers");
            List<IdentityUser> users = JsonConvert.DeserializeObject<List<IdentityUser>>(json);
            return users;
        }

        public async Task<IdentityUser> GetUser(string id)
        {
            var json = await _httpClient.GetStringAsync($"/GetUser/{id}");
            IdentityUser user = JsonConvert.DeserializeObject<IdentityUser>(json);
            return user;
        }

        public async Task UpdateUser(AuthInput authInput)
        {
            string json = JsonConvert.SerializeObject(authInput, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = TypeNameHandling.All,

            });

            var response = await _httpClient.PutAsJsonAsync($"/UpdateUser", json);
            var responseMessage = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                List<ErrorItem> errors = JsonConvert.DeserializeObject<List<ErrorItem>>(responseMessage);
                throw new BusinessException(errors);
            }
        }

        public async Task DeleteUser(string id)
        {
            var response = await _httpClient.DeleteAsync($"/DeleteUser/{id}");
            var responseMessage = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                List<ErrorItem> errors = JsonConvert.DeserializeObject<List<ErrorItem>>(responseMessage);
                throw new BusinessException(errors);
            }
        }
    }
}
