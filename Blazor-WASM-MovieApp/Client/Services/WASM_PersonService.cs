using Blazor_WASM_MovieApp.Exceptions;
using Blazor_WASM_MovieApp.Models;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace Blazor_WASM_MovieApp.Client.Services
{
    public class WASM_PersonService : WASM_IPersonService
    {
        private readonly HttpClient _httpClient;

        public WASM_PersonService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task AddPerson(Person person)
        {
            string json = JsonConvert.SerializeObject(person, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            var response = await _httpClient.PostAsJsonAsync("/AddPerson", json);
            var responseMessage = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                List<ErrorItem> errors = JsonConvert.DeserializeObject<List<ErrorItem>>(responseMessage);
                throw new BusinessException(errors);
            }
        }

        public async Task UpdatePerson(Person person)
        {
            string json = JsonConvert.SerializeObject(person, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = TypeNameHandling.All,

            });

            var response = await _httpClient.PutAsJsonAsync($"/UpdatePerson", json);
            var responseMessage = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                List<ErrorItem> errors = JsonConvert.DeserializeObject<List<ErrorItem>>(responseMessage);
                throw new BusinessException(errors);
            }
        }

        public async Task DeletePerson(int id)
        {
            var response = await _httpClient.DeleteAsync($"/DeletePerson/{id}");
            var responseMessage = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                List<ErrorItem> errors = JsonConvert.DeserializeObject<List<ErrorItem>>(responseMessage);
                throw new BusinessException(errors);
            }

        }

        public async Task<List<Person>> GetPeople()
        {
            var json = await _httpClient.GetStringAsync($"/GetPeople");
            List<Person> people = JsonConvert.DeserializeObject<List<Person>>(json);
            return people;
        }

        public async Task<Person> GetPerson(int id)
        {
            var json = await _httpClient.GetStringAsync($"/GetPerson/{id}");
            Person person = JsonConvert.DeserializeObject<Person>(json);
            return person;
        }

        public async Task<Person> GetPersonByName(string name)
        {
            var json = await _httpClient.GetStringAsync($"/GetPersonByName/{name}");
            Person person = JsonConvert.DeserializeObject<Person>(json);
            return person;
        }

        public async Task<List<Person>> SearchPeople(string searchString)
        {
            string json;
            List<Person> people;
            if (searchString == "" || searchString == null)
            {
                json = await _httpClient.GetStringAsync($"/GetPeople");
                people = JsonConvert.DeserializeObject<List<Person>>(json);
                return people;
            }

            json = await _httpClient.GetStringAsync($"/SearchPeople/{searchString}");
            people = JsonConvert.DeserializeObject<List<Person>>(json);
            return people;
        }
    }
}
