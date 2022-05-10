using Blazor_WASM_MovieApp.Models;
using Newtonsoft.Json;

namespace Blazor_WASM_MovieApp.Client.Services
{
    public class WASM_GenreService : WASM_IGenreService
    {
        private readonly HttpClient _httpClient;

        public WASM_GenreService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Genre>> GetGenres()
        {
            var json = await _httpClient.GetStringAsync($"/GetGenres");
            List<Genre> genres = JsonConvert.DeserializeObject<List<Genre>>(json);
            return genres;
        }

        public async Task<List<Genre>> SearchGenres(string searchString)
        {
            string json;
            List<Genre> genres;
            if (searchString == "" || searchString == null)
            {
                json = await _httpClient.GetStringAsync($"/GetGenres");
                genres = JsonConvert.DeserializeObject<List<Genre>>(json);
                return genres;
            }

            json = await _httpClient.GetStringAsync($"/SearchGenres/{searchString}");
            genres = JsonConvert.DeserializeObject<List<Genre>>(json);
            return genres;
        }
    }
}
