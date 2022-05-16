using Blazor_WASM_MovieApp.Models;

namespace Blazor_WASM_MovieApp.Client.Services
{
    public interface WASM_IGenreService
    {
        public Task<List<Genre>> GetGenres();
        public Task<Genre> GetGenre(int id);
        public Task AddGenre(Genre genre);
        public Task UpdateGenre(Genre genre);
        public Task DeleteGenre(int id);
        public Task<List<Genre>> SearchGenres(string searchString);
    }
}
