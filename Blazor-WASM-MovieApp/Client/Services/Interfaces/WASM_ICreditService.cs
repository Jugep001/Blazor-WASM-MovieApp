using Blazor_WASM_MovieApp.Exceptions;
using Blazor_WASM_MovieApp.Models;

namespace Blazor_WASM_MovieApp.Client.Services
{
    public interface WASM_ICreditService
    {
        public Task<List<Credit>> GetCreditsFromMovie(int movieId);
        public Task<List<Credit>> GetCreditsByName(string searchString);
        public Task AddCredit(Credit credit);
        public Task<List<Function>> GetFunctions();
        public Task<Function> GetFunction(int id);
        public Task CreditExist(ICollection<Credit> credits, int personId, int functionId, string role, Credit? oldCredit);
    }
}
