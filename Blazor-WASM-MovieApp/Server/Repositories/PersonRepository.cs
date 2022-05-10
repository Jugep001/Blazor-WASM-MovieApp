using Blazor_WASM_MovieApp.Data;
using Blazor_WASM_MovieApp.Models;

namespace Blazor_WASM_MovieApp.Repositories
{
    public class PersonRepository
    {
        private readonly BlazorMovieContext _dbContext;
        public PersonRepository(BlazorMovieContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void AddPerson(Person person)
        {
            _dbContext.People.Add(person);
            _dbContext.SaveChanges();
        }

        public void UpdatePerson(Person person)
        {
            _dbContext.People.Update(person);
            _dbContext.SaveChanges();
        }

        public void DeletePerson(int personId)
        {
            Person person = _dbContext.People.Find(personId);
            _dbContext.People.Remove(person);
            _dbContext.SaveChanges();
        }

        public List<Person> GetPeople()
        {
            List<Person> peopleList = (from people in _dbContext.People select people).OrderBy(g => g.Name).ToList();
            return peopleList;
        }

        public List<Person> GetPeople(string searchString)
        {
            List<Person> peopleList = (from people in _dbContext.People where people.Name!.Contains(searchString) select people).ToList();
            return peopleList;
        }

        public Person GetPerson(int personId)
        {
            Person? selectedPerson = _dbContext.People
                .Where(person => person.Id == personId)
                
                .FirstOrDefault();

            return selectedPerson;
        }

        public List<Person> GetListOfPeople(List<int> peopleIds)
        {
            List<Person> people = new List<Person>();
            foreach (var personId in peopleIds)
            {
                var selectedPerson = (from person in _dbContext.People where personId == person.Id select person).First();
                people.Add(selectedPerson);
            }
            return people;
        }

        public Person GetPersonByName(string firstName, string lastName)
        {
            Person person = (from people in _dbContext.People where people.Vorname == firstName where people.Name == lastName select people).First();
            return person;
        }

        public bool PersonExist(string vorname, string name, int id = 0)
        {
            IQueryable<Person>? peopleQuery = from people in _dbContext.People select people;
            if (id != 0)
            {
                peopleQuery = from people in _dbContext.People where people.Id != id select people;
            }


            return peopleQuery
                .Where(person => person.Vorname == vorname)
                .Where(person => person.Name == name)
                .Any();

        }

    }
}
