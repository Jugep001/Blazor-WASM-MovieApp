using Blazor_WASM_MovieApp.Exceptions;
using Blazor_WASM_MovieApp.Models;
using Blazor_WASM_MovieApp.Repositories;
using Blazor_WASM_MovieApp.Validators;

namespace Blazor_WASM_MovieApp.Services
{
    public class GenreService
    {
        private List<ErrorItem> errors = new List<ErrorItem>();
        private readonly GenreRepository _genreRepository;
        private readonly MovieRepository _movieRepository;
        private readonly GenreValidator _genreValidator;

        public GenreService(GenreRepository genreRepository, MovieRepository movieRepository, GenreValidator genreValidator)
        {
            _genreRepository = genreRepository;
            _movieRepository = movieRepository; 
            _genreValidator = genreValidator;
        }

        public void AddGenre(Genre genre)
        {
            errors = new List<ErrorItem>();
            _genreValidator.ValidateAndThrowBusinessException(genre);

            if(_genreRepository.GenreExist(genre.Name, genre.Id))
            {
                errors.Add(new ErrorItem("Name", "Dieses Genre existiert bereits!"));
                throw new BusinessException(errors);
            }

            _genreRepository.AddGenre(genre);

        }

        public void UpdateGenre(Genre genre)
        {

            _genreValidator.ValidateAndThrowBusinessException(genre);
            errors = new List<ErrorItem>();

            if (!_genreRepository.GenreExist(genre.Id))
            {
                errors.Add(new ErrorItem("Id", "Dieses Genre existiert nicht!"));
                throw new BusinessException(errors);
            }
            if (_genreRepository.GenreExist(genre.Name, genre.Id))
            {
                errors.Add(new ErrorItem("Name", "Dieses Genre existiert bereits!"));
                throw new BusinessException(errors);
            }


            _genreRepository.UpdateGenre(genre);

        }

        public Genre GetGenre(int genreId)
        {
            errors = new List<ErrorItem>();

            if (genreId == 0)
            {
                errors.Add(new ErrorItem("Name", "Dieses Genre existiert nicht"));
                throw new BusinessException(errors);

            }

            return _genreRepository.GetGenre(genreId);
        }



        public List<Genre> GetGenres(string? searchString)
        {
            errors = new List<ErrorItem>();
            if (!String.IsNullOrEmpty(searchString))
            {
                if (searchString.Length < 4 && searchString.Length > 0)
                {
                    errors.Add(new ErrorItem("Name", "Der Genrename ist zu kurz"));
                    throw new BusinessException(errors);
                }
                if (!_genreRepository.GenreExistByName(searchString))
                {
                    errors.Add(new ErrorItem("Name", "Das Genre existiert nicht"));
                    throw new BusinessException(errors);
                }
            }
            return _genreRepository.GetGenres(searchString);

        }

        public void DeleteGenre(int genreId)
        {
            errors = new List<ErrorItem>();
            if (_genreRepository.GenreExist(genreId) == false)
            {

                errors.Add(new ErrorItem("Name", "Dieses Genre existiert nicht"));
                throw new BusinessException(errors);

            }

            if (genreId == 0)
            {
                errors.Add(new ErrorItem("Name", "Dieses Genre existiert nicht"));
                throw new BusinessException(errors);
            }
            
            if (_movieRepository.MovieHasGenre(genreId))
            {

                errors.Add(new ErrorItem("Name", "Dieses Genre ist an Filmen gebunden"));
                throw new BusinessException(errors);

            }

            _genreRepository.DeleteGenre(genreId);
        }
    }
}
