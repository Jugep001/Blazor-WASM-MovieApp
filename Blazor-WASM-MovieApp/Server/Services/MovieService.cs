using Blazor_WASM_MovieApp.Exceptions;
using Blazor_WASM_MovieApp.Models;
using Blazor_WASM_MovieApp.Repositories;
using Blazor_WASM_MovieApp.Validators;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;

namespace Blazor_WASM_MovieApp.Services
{
    public class MovieService
    {
        private readonly MovieRepository _movieRepository;
        private readonly GenreRepository _genreRepository;
        private readonly MovieValidator _movieValidator;
        private List<ErrorItem> errors = new List<ErrorItem>();

        public MovieService(MovieRepository movieRepository, GenreRepository genreRepository, MovieValidator movieValidator)
        {
            _movieRepository = movieRepository;
            _genreRepository = genreRepository;
            _movieValidator = movieValidator;
        }

        public void AddMovie(Movie movie, Image image, List<int> genreIds, string currentUser)
        {
            errors = new List<ErrorItem>();

            _movieValidator.ValidateAndThrowBusinessException(movie);

            if (_movieRepository.MovieExist(movie.Title, movie.ReleaseDate.Value, movie.Id))
            {
                errors.Add(new ErrorItem("Title", "Dieser Film existiert bereits!"));
                throw new BusinessException(errors);
            }

            _movieRepository.AddMovie(movie, image, genreIds, currentUser);



        }

        public Task<Image> AddImage(IEnumerable<IFormFile> files)
        {
            return _movieRepository.AddImage(files);
        }

        public void UpdateMovie(Movie movie, Image image, List<int> genreIds, List<Credit> deleteCreditList, bool shouldDelete, string currentUser)
        {

            errors = new List<ErrorItem>();

            _movieValidator.ValidateAndThrowBusinessException(movie);

            if (_movieRepository.MovieExist(movie.Title, movie.ReleaseDate.Value, movie.Id))
            {
                errors.Add(new ErrorItem("Title", "Dieser Film existiert bereits!"));
                throw new BusinessException(errors);
            }

            foreach (int genreId in genreIds)
            {
                if (!_genreRepository.GenreExist(genreId))
                {
                    errors.Add(new ErrorItem("GenreId", "Das Genre existiert nicht"));
                    throw new BusinessException(errors);
                }
            }

            if (!_movieRepository.MovieExistsById(movie.Id))
            {
                errors.Add(new ErrorItem("Id", "Der Film existiert nicht!"));
                throw new BusinessException(errors);
            }

            _movieRepository.UpdateMovie(movie, image, genreIds, deleteCreditList, shouldDelete, currentUser);


        }



        public void DeleteMovie(int movieId, string currentUser)
        {
            errors = new List<ErrorItem>();

            if (_movieRepository.MovieExistsById(movieId) == false)
            {
                errors.Add(new ErrorItem("Title", "Dieser Film existiert nicht"));
                throw new BusinessException(errors);
            }

            if (movieId == 0)
            {
                errors.Add(new ErrorItem("Title", "Dieser Film existiert nicht"));
                throw new BusinessException(errors);
            }



            _movieRepository.DeleteMovie(movieId, currentUser);
        }

        public Movie GetMovie(int movieId)
        {
            errors = new List<ErrorItem>();

            if (_movieRepository.MovieExistsById(movieId) == false)
            {
                errors.Add(new ErrorItem("Title", "Dieser Film existiert nicht"));
                throw new BusinessException(errors);
            }

            if (movieId == 0)
            {
                errors.Add(new ErrorItem("Title", "Dieser Film existiert nicht"));
                throw new BusinessException(errors);
            }

            return _movieRepository.GetMovie(movieId);
        }

        public List<Movie> LuceneGetMovies(string? searchString, bool isAdmin)
        {
            errors = new List<ErrorItem>();



            if (searchString == null || searchString == "")
            {
                searchString = "EMPTYVALUE";
            }

            if(searchString != "EMPTYVALUE")
            {
                if (searchString.Split(" ").Count() > 1)
                {
                    var movieList = new List<IEnumerable<Movie>>();
                    var SearchStringList = searchString.Split(" ").ToList();
                    foreach (var search in SearchStringList)
                    {
                        movieList.Add(_movieRepository.LuceneGetMovies(search, isAdmin));
                    }
                    var finalMovieList = movieList.SelectMany(x => x).Distinct().ToList();
                    if (finalMovieList.Count == 0)
                    {
                        errors.Add(new ErrorItem("Title", "Es wurden keine Filme gefunden"));
                        throw new BusinessException(errors);
                    }
                    return finalMovieList;
                }
            }
            if(searchString == "EMPTYVALUE")
            {
                return GetMovies(isAdmin);
            }
            
            if(_movieRepository.LuceneGetMovies(searchString, isAdmin).Count == 0)
            {
                errors.Add(new ErrorItem("Title", "Dieser Film existiert nicht"));
                throw new BusinessException(errors);
            }
            return _movieRepository.LuceneGetMovies(searchString, isAdmin).ToList();
        }

        public List<MarkupString> HighlightDescription(string description,string? searchString)
        {
            return _movieRepository.HighlightDescription(description, searchString);
        }

        public List<Movie> GetMovies(bool isAdmin)
        {
           
            return _movieRepository.GetMovies(isAdmin);
        }    
        
        public List<Genre> GetMovieGenres(int movieId)
        {
            return _movieRepository.GetMovieGenres(movieId);
        }

        public string GetImagePath(string imageName)
        {

            return _movieRepository.GetImagePath(imageName);

        }

        public string GetThumbnailImagePath(string imageName)
        {

            return _movieRepository.GetThumbnailImagePath(imageName);

        }

        public string GetFirstTenWords(string description)
        {
            return _movieRepository.GetFirstTenWords(description);
        }

        public async Task RestoreMovie(int movieId, string currentUser)
        {
            //errors = new List<ErrorItem>();

            //if (_movieRepository.MovieExist(movie.Title, movie.ReleaseDate.Value, movie.Id))
            //{
            //    errors.Add(new ErrorItem("Title", "Dieser Film existiert bereits!"));
            //    throw new BusinessException(errors);
            //}

            _movieRepository.RestoreMovie(movieId, currentUser);
        }


    }
}
