using Blazor_WASM_MovieApp.Models;
using Newtonsoft.Json;

namespace Blazor_WASM_MovieApp.Client.Services
{
    public class WASM_PersonService : WASM_IPersonService
    {
        private readonly HttpClient _httpClient;

        public WASM_PersonService(HttpClient httpClient)
        {
            _httpClient = httpClient;
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
