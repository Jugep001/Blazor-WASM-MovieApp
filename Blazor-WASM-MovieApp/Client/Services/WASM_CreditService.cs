using Blazor_WASM_MovieApp.Exceptions;
using Blazor_WASM_MovieApp.Models;
using Blazor_WASM_MovieApp.Shared.Models;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace Blazor_WASM_MovieApp.Client.Services
{
    public class WASM_CreditService : WASM_ICreditService
    {
        private readonly HttpClient _httpClient;

        public WASM_CreditService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task AddCredit(Credit credit)
        {
            string json = JsonConvert.SerializeObject(credit, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            await _httpClient.PostAsJsonAsync("/AddCredit",json);
        }

        public async Task<List<Credit>> GetCreditsFromMovie(int movieId)
        {
            var json = await _httpClient.GetStringAsync($"/GetCreditsFromMovie/{movieId}");
            List<Credit> credits = JsonConvert.DeserializeObject<List<Credit>>(json);
            return credits;
        }

        public async Task<List<Function>> GetFunctions()
        {
            var json = await _httpClient.GetStringAsync($"/GetFunctions");
            List<Function> functions = JsonConvert.DeserializeObject<List<Function>>(json);
            return functions;
        }

        public async Task<Function> GetFunction(int id)
        {
            var json = await _httpClient.GetStringAsync($"/GetFunction/{id}");
            Function function = JsonConvert.DeserializeObject<Function>(json);
            return function;
        }

        public async Task CreditExist(ICollection<Credit> credits, int personId, int functionId, string role, Credit? oldCredit)
        {
            CreditInput creditInput = new CreditInput
            {
                Credits = credits,
                PersonId = personId,
                FunctionId = functionId,
                Role = role,
                OldCredit = oldCredit
            };
            var json = await _httpClient.GetStringAsync($"/CreditExist/{creditInput}");
            List<ErrorItem> errors = JsonConvert.DeserializeObject<List<ErrorItem>>(json);
            if(errors.Count > 0)
            {
                throw new BusinessException(errors);
            }
        }

        public async Task<List<Credit>> GetCreditsByName(string searchString)
        {
            var json = await _httpClient.GetStringAsync($"/GetCreditsByName/{searchString}");
            List<Credit> credits = JsonConvert.DeserializeObject<List<Credit>>(json);
            return credits;
        }
    }
}
