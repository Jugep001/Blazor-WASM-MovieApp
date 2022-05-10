using Blazor_WASM_MovieApp.Data;
using Blazor_WASM_MovieApp.Models;

namespace Blazor_WASM_MovieApp.Repositories
{
    public class GenreRepository
    {
        private readonly BlazorMovieContext _dbContext;
        public GenreRepository(BlazorMovieContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddGenre(Genre genre)
        {

            _dbContext.Genres.Add(genre);
            _dbContext.SaveChanges();

        }

        public void UpdateGenre(Genre genre)
        {
            _dbContext.Genres.Update(genre);
            _dbContext.SaveChanges();
        }


        public List<Genre> GetGenresByName(string name)
        {

            return _dbContext.Genres.Where(genre => genre.Name == name).ToList();


        }

        public List<Genre> GetGenresByName(string name, int id)
        {
            IQueryable<Genre>? genres = from genre in _dbContext.Genres where genre.Id != id select genre;
            if (genres.FirstOrDefault(genre => genre.Name == name) != null)
            {
                return _dbContext.Genres.Where(genre => genre.Name == name).ToList();
            }
            return new List<Genre>();


        }

        public Genre GetGenre(int genreId)
        {
            Genre? genre = _dbContext.Genres.Where(genre => genre.Id == genreId).First();

            return genre;
        }

        

        public List<Genre> GetGenres(string searchString)
        {
            List<Genre> Genres = (from genres in _dbContext.Genres select genres).OrderBy(g => g.Name).ToList(); ;

            if (searchString != null)
            {
                Genres = (from genres in _dbContext.Genres where genres.Name!.Contains(searchString) select genres).ToList();
            }

            return Genres;

        }

        public void DeleteGenre(int genreId)
        {
            Genre? genre = _dbContext.Genres.Find(genreId);
            _dbContext.Genres.Remove(genre);
            _dbContext.SaveChanges();
        }

        public bool MoviesExist(string title)
        {
            return _dbContext.Movies.Where(movie => movie.Title!.Contains(title)).Any();
        }

        public bool GenreExist(int genreId)
        {
            return _dbContext.Genres.Any(genre => genre.Id == genreId);
        }

        public bool GenreExist(string name,int genreId)
        {

            IQueryable<Genre>? genres = from genre in _dbContext.Genres where genre.Id != genreId select genre;
            return genres.Any(genre => genre.Name == name);

        }

        public bool GenreExistByName(string name)
        {
            IQueryable<Genre>? genreQuery = (from genres in _dbContext.Genres where genres.Name!.Contains(name) select genres);
            if (genreQuery != null)
                return true;

            return false;
        }
    }
}
