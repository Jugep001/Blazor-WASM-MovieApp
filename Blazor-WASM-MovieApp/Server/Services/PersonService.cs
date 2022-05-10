using Blazor_WASM_MovieApp.Exceptions;
using Blazor_WASM_MovieApp.Models;
using Blazor_WASM_MovieApp.Repositories;
using Blazor_WASM_MovieApp.Validators;

namespace Blazor_WASM_MovieApp.Services
{
    public class PersonService
    {
        private List<ErrorItem> errors = new List<ErrorItem>();
        private readonly PersonRepository _personRepository;
        private readonly MovieRepository _movieRepository;
        private readonly PersonValidator _personValidator;

        public PersonService(PersonRepository personRepository, MovieRepository movieRepository, PersonValidator personValidator)
        {

            _personRepository = personRepository;
            _movieRepository = movieRepository;
            _personValidator = personValidator;

        }

        public void AddPerson(Person person)
        {
            _personValidator.ValidateAndThrowBusinessException(person);
            _personRepository.AddPerson(person);
        }

        public void UpdatePerson(Person person)
        {
            _personValidator.ValidateAndThrowBusinessException(person);
            _personRepository.UpdatePerson(person);
        }

        public void DeletePerson(int personId)
        {
            if (personId == 0)
            {
                errors.Add(new ErrorItem("personId", "Diese Person gibt es nicht!"));
                throw new BusinessException(errors);
            }

            if (_movieRepository.MovieHasPerson(personId))
            {

                errors.Add(new ErrorItem("Name", "Diese Person ist an Filmen gebunden"));
                throw new BusinessException(errors);

            }

            _personRepository.DeletePerson(personId);
        }

        public List<Person> GetListOfPeople(List<int> peopleIds)
        {
            return _personRepository.GetListOfPeople(peopleIds);
        }

        public List<Person> GetPeople()
        {
            return _personRepository.GetPeople();
        }

        public List<Person> GetPeople(string searchString)
        {
            return _personRepository.GetPeople(searchString);
        }

        public Person GetPerson(int id)
        {
            if (id == 0)
            {
                errors.Add(new ErrorItem("personId", "Diese Person gibt es nicht!"));
                throw new BusinessException(errors);
            }

            return _personRepository.GetPerson(id);
        }

        public Person GetPersonByName(string Name)
        {
            string[] FullName = Name.Split(' ');

            if (FullName.Length < 2)
            {
                errors.Add(new ErrorItem("Vorname", "Kein richtiger Name!"));
                throw new BusinessException(errors);
            }

            string firstName = FullName[0];
            string lastName = FullName[1];

            if (!_personRepository.PersonExist(firstName, lastName))
            {
                errors.Add(new ErrorItem("Vorname", "Diese Person gibt es nicht"));
                throw new BusinessException(errors);
            }

            return _personRepository.GetPersonByName(firstName, lastName);
        }
    }
}
