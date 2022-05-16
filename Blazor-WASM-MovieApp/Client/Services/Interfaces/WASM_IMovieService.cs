using Blazor_WASM_MovieApp.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Blazor_WASM_MovieApp.Client.Services
{
    public interface WASM_IMovieService
    {
        public Task<List<Movie>> GetMovies(bool isAdmin);
        public Task<Movie> GetMovie(int id);
        public Task<List<Movie>> SearchMovies(string description, bool isAdmin);
        public Task<string> GetFirstTenWords(string description);
        public List<MarkupString> GetHighlightedDescription(string description, string searchString);
        public Task<string> AddMovie(Movie movie, IBrowserFile? loadedImage, IBrowserFile? loadedThumbnailImage, List<int> GenreIds);
        public Task<string> UpdateMovie(Movie movie, IBrowserFile? loadedImage, IBrowserFile? loadedThumbnailImage, List<int> GenreIds, List<Credit> DeleteCreditList, bool shouldDelete, string currentUser);
        public Task DeleteMovie(int id, string currentUser);
        public Task RestoreMovie(Movie movie, string currentUser);
    }
}
