using Blazor_WASM_MovieApp.Client.Repositories;
using Blazor_WASM_MovieApp.Models;
using Blazor_WASM_MovieApp.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Net.Http;
using Blazor_WASM_MovieApp.Exceptions;

namespace Blazor_WASM_MovieApp.Client.Services
{
    public class WASM_MovieService : WASM_IMovieService
    {
        private readonly HttpClient _httpClient;
        private readonly WASM_MovieRepository _movieRepository;

        public WASM_MovieService(HttpClient httpClient, WASM_MovieRepository movieRepository)
        {
            _httpClient = httpClient;
            _movieRepository = movieRepository;
        }

        public async Task<List<Movie>> GetMovies(bool isAdmin)
        {
            var json = await _httpClient.GetStringAsync($"/GetMovies/{isAdmin}");
            List<Movie> movies = JsonConvert.DeserializeObject<List<Movie>>(json);
            return movies;
        }

        public async Task<Movie> GetMovie(int id)
        {
            var json = await _httpClient.GetStringAsync($"/GetMovie/{id}");
            Movie movie = JsonConvert.DeserializeObject<Movie>(json);
            return movie;
        }

        public async Task<string> GetFirstTenWords(string description)
        {
            string json = await _httpClient.GetStringAsync($"GetFirstTenWords/{description}");
            string markupString = JsonConvert.DeserializeObject<string>(json);
            return markupString;
        }

        public async Task<List<Movie>> SearchMovies(string searchString, bool isAdmin)
        {
            var json = await _httpClient.GetStringAsync($"/SearchMovies/{searchString}/{true}");
            List<Movie> movies = JsonConvert.DeserializeObject<List<Movie>>(json);
            return movies;
        }

        public List<MarkupString> GetHighlightedDescription(string description, string searchString)
        {
            return _movieRepository.HighlightDescription(description, searchString);
        }

        public async Task<string> AddMovie(Movie movie, IBrowserFile? loadedImage, IBrowserFile? loadedThumbnailImage, List<int> genreIds, string currentUser)
        {
            Image image = null;
            if (loadedImage != null)
            {
                MultipartFormDataContent content = new MultipartFormDataContent();
                StreamContent? imgContent = new StreamContent(loadedImage.OpenReadStream());
                StreamContent? thumbImgContent = new StreamContent(loadedThumbnailImage.OpenReadStream());
                content.Add(content: imgContent, "\"files\"", loadedImage.Name);
                content.Add(thumbImgContent, "\"files\"", loadedThumbnailImage.Name);

                HttpResponseMessage? result = await _httpClient.PostAsync("/AddImage", content);
                image = await result.Content.ReadFromJsonAsync<Image>();

                
            }
            MovieInput movieInput = new MovieInput
            {
                Movie = movie,
                Image = image,
                GenreIds = genreIds,
                CurrentUser = currentUser
            };

            string json = JsonConvert.SerializeObject(movieInput, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = TypeNameHandling.All,

            });

            var response = await _httpClient.PostAsJsonAsync($"/AddMovie", json);
            var responseMessage = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                List<ErrorItem> errors = JsonConvert.DeserializeObject<List<ErrorItem>>(responseMessage);
                throw new BusinessException(errors);
            }
            return response.StatusCode.ToString();
        }

        public async Task DeleteMovie(int id, string currentUser)
        {
            await _httpClient.DeleteAsync($"/DeleteMovie/{id}/{currentUser}");
        }

        public async Task<string> UpdateMovie(Movie movie, IBrowserFile? loadedImage, IBrowserFile? loadedThumbnailImage, List<int> genreIds, List<Credit> deleteCreditList, bool shouldDelete, string currentUser)
        {
            Image image = null;
            if (loadedImage != null)
            {
                MultipartFormDataContent content = new MultipartFormDataContent();
                StreamContent? imgContent = new StreamContent(loadedImage.OpenReadStream());
                StreamContent? thumbImgContent = new StreamContent(loadedThumbnailImage.OpenReadStream());
                content.Add(content: imgContent, "\"files\"", loadedImage.Name);
                content.Add(thumbImgContent, "\"files\"", loadedThumbnailImage.Name);

                try
                {
                    HttpResponseMessage? result = await _httpClient.PostAsync("AddImage", content);                  
                    image = await result.Content.ReadFromJsonAsync<Image>();
                }
                catch(TaskCanceledException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch(HttpRequestException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine(ex.Message);
                }




            }
            MovieInput movieInput = new MovieInput
            {
                Movie = movie,
                Image = image,
                GenreIds = genreIds,
                DeleteCreditList = deleteCreditList,
                ShouldDelete = shouldDelete,
                CurrentUser = currentUser
            };

            string json = JsonConvert.SerializeObject(movieInput, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = TypeNameHandling.All,

            });

            var response = await _httpClient.PutAsJsonAsync($"/UpdateMovie", json);
            var responseMessage = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                List<ErrorItem> errors = JsonConvert.DeserializeObject<List<ErrorItem>>(responseMessage);
                throw new BusinessException(errors);
            }
            return response.StatusCode.ToString();
        }

        public async Task RestoreMovie(Movie movie, string currentUser)
        {

            var response = await _httpClient.PutAsJsonAsync($"/RestoreMovie", movie.Id);
        }
    }
}
