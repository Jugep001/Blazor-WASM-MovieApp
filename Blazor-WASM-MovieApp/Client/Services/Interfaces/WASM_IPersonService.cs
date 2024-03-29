﻿using Blazor_WASM_MovieApp.Models;

namespace Blazor_WASM_MovieApp.Client.Services
{
    public interface WASM_IPersonService
    {
        public Task<List<Person>> GetPeople();
        public Task AddPerson(Person person);
        public Task UpdatePerson(Person person);
        public Task DeletePerson(int id);
        public Task<Person> GetPersonByName(string name);
        public Task<List<Person>> SearchPeople(string searchString);
        public Task<Person> GetPerson(int id);
    }
}
