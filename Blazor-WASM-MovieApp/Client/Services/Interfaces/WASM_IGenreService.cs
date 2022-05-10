using Blazor_WASM_MovieApp.Models;

namespace Blazor_WASM_MovieApp.Client.Services
{
    public interface WASM_IGenreService
    {
        public Task<List<Genre>> GetGenres();
        public Task<List<Genre>> SearchGenres(string searchString);
    }
}
