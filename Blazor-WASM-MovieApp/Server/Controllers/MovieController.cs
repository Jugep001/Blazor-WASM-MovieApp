using Blazor_WASM_MovieApp.Exceptions;
using Blazor_WASM_MovieApp.Models;
using Blazor_WASM_MovieApp.Services;
using Blazor_WASM_MovieApp.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Blazor_WASM_MovieApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieController : ControllerBase
    {

        private readonly MovieService _movieService;
        private readonly GenreService _genreService;
        private readonly CreditService _creditService;
        private readonly PersonService _personService;



        public MovieController(MovieService movieService, GenreService genreService, PersonService personService, CreditService creditService)
        {
            _movieService = movieService;
            _genreService = genreService;
            _creditService = creditService;
            _personService = personService;
        }

        


        [HttpGet]
        [Route("/GetGenres")]
        public async Task<IActionResult> GetGenres()
        {
            var genres = _genreService.GetGenres(null);
            string json = JsonConvert.SerializeObject(genres, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Ok(json);
        }

        [HttpGet]
        [Route("/GetGenre/{id}")]
        public async Task<IActionResult> GetGenre(int id)
        {
            var genre = _genreService.GetGenre(id);
            string json = JsonConvert.SerializeObject(genre, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Ok(json);
        }

        [HttpGet]
        [Route("/SearchGenres/{searchString}")]
        public async Task<IActionResult> SearchGenres(string searchString)
        {
            var genres = _genreService.GetGenres(searchString);
            string json = JsonConvert.SerializeObject(genres, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Ok(json);
        }

        [HttpGet]
        [Route("/GetPeople")]
        public async Task<IActionResult> GetPeople()
        {
            var people = _personService.GetPeople();
            string json = JsonConvert.SerializeObject(people, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Ok(json);
        }

        [HttpGet]
        [Route("/GetFunctions")]
        public async Task<IActionResult> GetFunctions()
        {
            var functions = _creditService.GetFunctions();
            string json = JsonConvert.SerializeObject(functions, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Ok(json);
        }

        [HttpGet]
        [Route("/GetFunction/{id}")]
        public async Task<IActionResult> GetFunction(int id)
        {
            var function = _creditService.GetFunction(id);
            string json = JsonConvert.SerializeObject(function, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Ok(json);
        }

        [HttpGet]
        [Route("/GetPerson/{id}")]
        public async Task<IActionResult> GetPersonByName(int id)
        {
            var person = _personService.GetPerson(id);
            string json = JsonConvert.SerializeObject(person, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Ok(json);
        }

        [HttpGet]
        [Route("/GetPersonByName/{name}")]
        public async Task<IActionResult> GetPersonByName(string name)
        {
            var person = _personService.GetPersonByName(name);
            string json = JsonConvert.SerializeObject(person, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Ok(json);
        }


        [HttpGet]
        [Route("/SearchPeople/{searchString}")]
        public async Task<IActionResult> SearchPeople(string searchString)
        {
            var people = _personService.GetPeople(searchString);
            string json = JsonConvert.SerializeObject(people, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Ok(json);
        }



        [HttpGet]
        [Route("/GetMovies/{isAdmin}")]
        public async Task<IActionResult> GetMovies(bool isAdmin)
        {
            var movies = _movieService.GetMovies(isAdmin);
            string json = JsonConvert.SerializeObject(movies, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return Ok(json);
        }

        [HttpGet]
        [Route("/GetMovie/{id}")]
        public async Task<IActionResult> GetMovie(int id)
        {
            var movie = _movieService.GetMovie(id);
            string json = JsonConvert.SerializeObject(movie, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return Ok(json);
        }

        [HttpGet]
        [Route("/GetMovieById/{id}")]
        public async Task<IActionResult> GetMovieById(int id)
        {
            var movie = _movieService.GetMovie(id);
            string json = JsonConvert.SerializeObject(movie, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return Ok(json);
        }

        [HttpGet]
        [Route("/GetCreditsFromMovie/{movieId}")]
        public async Task<IActionResult> GetCreditsFromMovie(int movieId)
        {
            var credits = _creditService.GetCreditsFromMovie(movieId);
            string json = JsonConvert.SerializeObject(credits, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return Ok(json);
        }

        [HttpGet]
        [Route("/GetCreditsByName/{searchString}")]
        public async Task<IActionResult> GetCreditsFromMovie(string searchString)
        {
            var credits = _creditService.GetCreditsByName(searchString);
            string json = JsonConvert.SerializeObject(credits, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return Ok(json);
        }

        [HttpGet]
        [Route("/GetMovieGenres/{movieId}")]
        public List<Genre> GetMovieGenres(int movieId)
        {
            return _movieService.GetMovieGenres(movieId);
        }

        [HttpGet]
        [Route("/GetFirstTenWords/{description}")]
        public async Task<IActionResult> GetFirstTenWords(string description)
        {
            Microsoft.AspNetCore.Components.MarkupString short_description = (Microsoft.AspNetCore.Components.MarkupString)_movieService.GetFirstTenWords(description);
            string json = JsonConvert.SerializeObject(short_description, Formatting.None, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Ok(json);
        }

        [HttpGet("/SearchMovies/{searchString}/{isAdmin}")]
        public async Task<IActionResult> SearchMovies(string searchString, bool isAdmin)
        {
            var movies = _movieService.LuceneGetMovies(searchString, isAdmin);
            string json = JsonConvert.SerializeObject(movies, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Ok(json);
        }

        [HttpPost]
        [Route("/AddMovie")]
        public async Task<IActionResult> AddMovie([FromBody] string json)
        {
            try
            {
                MovieInput movieInput = JsonConvert.DeserializeObject<MovieInput>(json);
                _movieService.AddMovie(movieInput.Movie, movieInput.Image, movieInput.GenreIds, movieInput.CurrentUser);
                return Ok();
            }
            catch (BusinessException ex)
            {
                string errorString = JsonConvert.SerializeObject(ex.ExceptionMessageList, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                return BadRequest(errorString);
            }

        }

        [HttpPut]
        [Route("/UpdateMovie")]
        public async Task<IActionResult> UpdateMovie([FromBody] string json)
        {
            try
            {
                MovieInput movieInput = JsonConvert.DeserializeObject<MovieInput>(json);
                _movieService.UpdateMovie(movieInput.Movie, movieInput.Image, movieInput.GenreIds, movieInput.DeleteCreditList, movieInput.ShouldDelete, movieInput.CurrentUser);
                return Ok();
            }
            catch (BusinessException ex)
            {
                string errorString = JsonConvert.SerializeObject(ex.ExceptionMessageList, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                return BadRequest(errorString);
            }

        }

        [HttpPost]
        [Route("/AddImage")]
        public async Task<IActionResult> AddImage([FromForm] IEnumerable<IFormFile> files)
        {

            Image image = await _movieService.AddImage(files);

            return Ok(image);
        }


        [HttpPost("/AddCredit")]
        public async Task<IActionResult> AddCredit([FromBody] string json)
        {
            Credit credit = JsonConvert.DeserializeObject<Credit>(json);
            _creditService.AddCredit(credit);
            return Ok();
        }

        [HttpPost("/CreditExist")]
        public async Task<IActionResult> CreditExist([FromBody] string json)
        {
            try
            {
                CreditInput creditInput = JsonConvert.DeserializeObject<CreditInput>(json);
                _creditService.CreditExist(creditInput.Credits, creditInput.PersonId, creditInput.FunctionId, creditInput.Role, creditInput.OldCredit);
                
            }          
            catch (BusinessException ex)
            {
                string errorString = JsonConvert.SerializeObject(ex.ExceptionMessageList, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                return BadRequest(errorString);
            }
            return Ok();
        }

        [HttpDelete("/DeleteMovie/{id}/{currentUser}")]
        public async Task<IActionResult> DeleteMovie(int id, string currentUser)
        {
            _movieService.DeleteMovie(id, currentUser);
            return Ok();
        }

        [HttpPost("/AddGenre")]
        public async Task<IActionResult> AddGenre([FromBody] string json)
        {
            try
            {

                Genre genre = JsonConvert.DeserializeObject<Genre>(json);
                _genreService.AddGenre(genre);
                return Ok();

            }
            catch (BusinessException ex)
            {
                string errorString = JsonConvert.SerializeObject(ex.ExceptionMessageList, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                return BadRequest(errorString);
            }
        }

        [HttpPut("/UpdateGenre")]
        public async Task<IActionResult> UpdateGenre([FromBody] string json)
        {
            try
            {

                Genre genre = JsonConvert.DeserializeObject<Genre>(json);
                _genreService.UpdateGenre(genre);
                return Ok();

            }
            catch (BusinessException ex)
            {
                string errorString = JsonConvert.SerializeObject(ex.ExceptionMessageList, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                return BadRequest(errorString);
            }

        }

        [HttpDelete("/DeleteGenre/{id}")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            try
            {
                _genreService.DeleteGenre(id);
                return Ok();
            }
            catch (BusinessException ex)
            {
                string errorString = JsonConvert.SerializeObject(ex.ExceptionMessageList, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                return BadRequest(errorString);
            }

        }

        [HttpPost("/AddPerson")]
        public async Task<IActionResult> AddPerson([FromBody] string json)
        {
            try
            {
                Person person = JsonConvert.DeserializeObject<Person>(json);
                _personService.AddPerson(person);
                return Ok();
            }
            catch (BusinessException ex)
            {
                string errorString = JsonConvert.SerializeObject(ex.ExceptionMessageList, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                return BadRequest(errorString);
            }

        }

        [HttpPut("/UpdatePerson")]
        public async Task<IActionResult> UpdatePerson([FromBody] string json)
        {
            try
            {
                Person person = JsonConvert.DeserializeObject<Person>(json);
                _personService.UpdatePerson(person);
                return Ok();
            }
            catch (BusinessException ex)
            {
                string errorString = JsonConvert.SerializeObject(ex.ExceptionMessageList, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                return BadRequest(errorString);
            }

        }

        [HttpDelete("/DeletePerson/{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            try
            {
                _personService.DeletePerson(id);
                return Ok();
            }
            catch (BusinessException ex)
            {
                string errorString = JsonConvert.SerializeObject(ex.ExceptionMessageList, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                return BadRequest(errorString);
            }
        }

        [HttpPut("/RestoreMovie")]
        public async Task<IActionResult> RestoreMovie([FromBody] int movieId)
        {
            await _movieService.RestoreMovie(movieId, "admin");
            return Ok();
        }

    }
}