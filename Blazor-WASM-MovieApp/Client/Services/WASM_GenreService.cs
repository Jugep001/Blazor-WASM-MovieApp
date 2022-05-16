using Blazor_WASM_MovieApp.Exceptions;
using Blazor_WASM_MovieApp.Models;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace Blazor_WASM_MovieApp.Client.Services
{
    public class WASM_GenreService : WASM_IGenreService
    {
        private readonly HttpClient _httpClient;

        public WASM_GenreService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task AddGenre(Genre genre)
        {
            string json = JsonConvert.SerializeObject(genre, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            var response = await _httpClient.PostAsJsonAsync("/AddGenre", json);
            var responseMessage = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                List<ErrorItem> errors = JsonConvert.DeserializeObject<List<ErrorItem>>(responseMessage);
                throw new BusinessException(errors);
            }
            
        }

        public async Task UpdateGenre(Genre genre)
        {
            string json = JsonConvert.SerializeObject(genre, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = TypeNameHandling.All,

            });

            var response = await _httpClient.PutAsJsonAsync($"/UpdateGenre", json);
            var responseMessage = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                List<ErrorItem> errors = JsonConvert.DeserializeObject<List<ErrorItem>>(responseMessage);
                throw new BusinessException(errors);
            }
        }

        public async Task DeleteGenre(int id)
        {
            var response = await _httpClient.DeleteAsync($"/DeleteGenre/{id}");
            var responseMessage = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                List<ErrorItem> errors = JsonConvert.DeserializeObject<List<ErrorItem>>(responseMessage);
                throw new BusinessException(errors);
            }

        }

        public async Task<Genre> GetGenre(int id)
        {
            var json = await _httpClient.GetStringAsync($"/GetGenre/{id}");
            Genre genre = JsonConvert.DeserializeObject<Genre>(json);
            return genre;
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
