using Blazor_WASM_MovieApp.Data;
using Microsoft.AspNetCore.Components.Forms;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using System.Diagnostics;
using LuceneDirectory = Lucene.Net.Store.Directory;
using Lucene.Net.Search.Highlight;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Blazor_WASM_MovieApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components;

namespace Blazor_WASM_MovieApp.Repositories
{
    public class MovieRepository
    {
        private readonly BlazorMovieContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _env;
        private string indexName = "Movies_index";
        private Guid imageId;
        const LuceneVersion luceneVersion = LuceneVersion.LUCENE_48;
        Lucene.Net.Analysis.Analyzer standardAnalyzer = new StandardAnalyzer(luceneVersion);
        Lucene.Net.Analysis.Analyzer classicAnalyzer = new ClassicAnalyzer(luceneVersion);

        public MovieRepository(BlazorMovieContext dbContext, IWebHostEnvironment env, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _env = env;
            _userManager = userManager;


        }

        public async void AddMovie(Movie movie, Image image, List<int> genreIds, string currentUser)
        {
            if (movie.Description != null || movie.Description != "")
            {
                movie.ShortDescription = String.Empty;
                var descriptionWithoutMarkup = Regex.Replace(movie.Description, "<.*?>", String.Empty);
                var shortDescriptionList = descriptionWithoutMarkup.Split().Take(10);
                var lastItem = shortDescriptionList.Last();
                foreach (var shortDescription in shortDescriptionList)
                {
                    if (shortDescription.Equals(lastItem))
                    {
                        movie.ShortDescription += lastItem;
                        break;
                    }
                    movie.ShortDescription += shortDescription + " ";

                }
                movie.ShortDescription += "...";
            }

            if (image != null)
            {
                movie.Image = image;

            }
            else
            {
                movie.Image = null;
            }




            movie.Genres = new List<Genre>();
            foreach (var genreId in genreIds)
            {
                Genre? selectedGenre = (from genre in _dbContext.Genres where genreId == genre.Id select genre).First();
                movie.Genres.Add(selectedGenre);
            }

            foreach (var credit in movie.Credits)
            {
                credit.Id = null;
            }

            _dbContext.Movies.Add(movie);
            _dbContext.SaveChanges();


            AddChangelog(movie, currentUser);


            string indexPath = Path.Combine(Environment.CurrentDirectory, indexName);

            using LuceneDirectory indexDir = FSDirectory.Open(indexPath);

            IndexWriterConfig indexConfig = new IndexWriterConfig(luceneVersion, standardAnalyzer);
            indexConfig.OpenMode = OpenMode.APPEND;
            IndexWriter writer = new IndexWriter(indexDir, indexConfig);

            Document doc = new Document();
            doc.Add(new StringField("Id", movie.Id.ToString(), Field.Store.YES));
            doc.Add(new TextField("Title", movie.Title, Field.Store.YES));
            doc.Add(new TextField("Description", movie.Description, Field.Store.YES));
            if (movie.Genres != null)
            {
                foreach (Genre genre in movie.Genres)
                {
                    doc.Add(new TextField("Genre", genre.Name, Field.Store.YES));
                }
            }
            writer.AddDocument(doc);

            writer.Dispose();
        }





        public async void UpdateMovie(Movie movie, Image image, List<int> genreIds, List<Credit> deleteCreditList, bool shouldDelete, string currentUser)
        {
            var tempMovie = _dbContext.Movies.Include(x => x.Genres).Include(x => x.Image).Include(x => x.Changelogs).Where(x => x.Id == movie.Id).IgnoreQueryFilters().First();
            tempMovie.Title = movie.Title;
            tempMovie.Description = movie.Description;
            tempMovie.Price = movie.Price;
            tempMovie.Rating = movie.Rating;
            tempMovie.ReleaseDate = movie.ReleaseDate;

            if (tempMovie.Description != null || tempMovie.Description != "")
            {
                tempMovie.ShortDescription = String.Empty;
                var descriptionWithoutMarkup = Regex.Replace(tempMovie.Description, "<.*?>", String.Empty);
                var shortDescriptionList = descriptionWithoutMarkup.Split().Take(10);
                var lastItem = shortDescriptionList.Last();
                foreach (var shortDescription in shortDescriptionList)
                {
                    if (shortDescription.Equals(lastItem))
                    {
                        tempMovie.ShortDescription += shortDescription;
                        break;
                    }
                    else
                    {
                        tempMovie.ShortDescription += shortDescription + " ";
                    }

                }
                tempMovie.ShortDescription += "...";
            }
            if (image != null)
            {

                tempMovie.Image = image;

            }
            if (shouldDelete == true)
            {
                File.Delete(tempMovie.Image.Path);
                File.Delete(tempMovie.Image.ThumbnailPath);
                _dbContext.Images.Remove(tempMovie.Image);
                tempMovie.Image = null;
            }

            if (deleteCreditList.Count != 0)
            {
                foreach (Credit credit in deleteCreditList)
                {
                    _dbContext.Credits.Remove(credit);
                }
            }

            tempMovie.Genres?.Clear();
            _dbContext.Movies.Update(tempMovie);
            _dbContext.SaveChanges();
            foreach (var genreId in genreIds)
            {
                var Genre = (from genre in _dbContext.Genres where genreId == genre.Id select genre).First();
                tempMovie.Genres?.Add(Genre);

            }
            _dbContext.Movies.Update(tempMovie);
            _dbContext.SaveChanges();

            UpdateChangelog(tempMovie, currentUser);

            string indexPath = Path.Combine(Environment.CurrentDirectory, indexName);

            using LuceneDirectory indexDir = FSDirectory.Open(indexPath);

            IndexWriterConfig indexConfig = new IndexWriterConfig(luceneVersion, standardAnalyzer);
            indexConfig.OpenMode = OpenMode.APPEND;
            IndexWriter writer = new IndexWriter(indexDir, indexConfig);

            Document doc = new Document();
            doc.Add(new StringField("Id", tempMovie.Id.ToString(), Field.Store.YES));
            doc.Add(new TextField("Title", tempMovie.Title, Field.Store.YES));
            doc.Add(new TextField("Description", tempMovie.Description, Field.Store.YES));
            if (movie.Genres != null)
            {
                foreach (Genre genre in movie.Genres)
                {
                    doc.Add(new TextField("Genre", genre.Name, Field.Store.YES));
                }
            }
            writer.UpdateDocument(new Term("Id", tempMovie.Id.ToString()), doc);

            writer.Dispose();
        }

        public async Task<Image> AddImage(IEnumerable<IFormFile> files)
        {


            string? imgName = files.ElementAt(0).FileName;
            string path = $"{_env.WebRootPath}\\images\\{imgName}";
            string thumbnailPath = string.Empty;
            imageId = Guid.NewGuid();

            string imageName = Path.GetFileNameWithoutExtension(path);
            string imageExtension = Path.GetExtension(path);

            path = $"{_env.WebRootPath}\\images\\{imageName + imageId.ToString() + imageExtension}";
            thumbnailPath = $"{_env.WebRootPath}\\images\\thumbnails\\{imageName + imageId.ToString() + imageExtension}";

            Image imageModel = new Image
            {
                ImageName = imageName + imageId.ToString() + imageExtension,
                Path = path,
                ThumbnailPath = thumbnailPath,
                Size = files.ElementAt(0).Length
            };

            _dbContext.Add(imageModel);

            var imgStream = files.ElementAt(0).OpenReadStream();
            var thumbImgStream = files.ElementAt(1).OpenReadStream();
            await using FileStream fs = new(path, FileMode.Create);
            await imgStream.CopyToAsync(fs);
            await using FileStream fs2 = new(thumbnailPath, FileMode.Create);
            await thumbImgStream.CopyToAsync(fs2);

            return imageModel;

        }

        public void AddImageToDatabase(Movie movie, Guid imageId, long size, string imageName, string imageExtension, string path, string thumbnailPath)
        {

            using (var dbContext = _dbContext)
            {
                Image imageModel = new Image
                {
                    ImageName = imageName + imageId.ToString() + imageExtension,
                    Path = path,
                    ThumbnailPath = thumbnailPath,
                    Size = size,
                    MovieId = movie.Id,
                    Movie = movie
                };
                dbContext.Add(imageModel);
                dbContext.SaveChanges();


            }



        }

        public List<Movie> GetMoviesByTitleAndYear(string title, int year)
        {

            return _dbContext.Movies.Where(x => x.Title == title).Where(x => ((DateTime)x.ReleaseDate).Year == year).ToList();


        }



        public void DeleteMovie(int movieId, string currentUser)
        {
            Movie? movie = _dbContext.Movies.Include(movie => movie.Changelogs).Where(movie => movie.Id == movieId).FirstOrDefault();
            movie.IsDeleted = true;
            _dbContext.Movies.Update(movie);
            DeleteChangelog(movie, currentUser);

            return;
        }

        public void RestoreMovie(int movieId, string currentUser)
        {
            Movie movie = GetMovie(movieId);
            movie.IsDeleted = false;

            _dbContext.Movies.Update(movie);


            UpdateChangelog(movie, currentUser);
            _dbContext.SaveChanges();
        }

        public Movie? GetMovie(int movieId)
        {
            Movie? movie = _dbContext.Movies
                .Include(movie => movie.Genres)
                .Include(movie => movie.Image)
                .Include(movie => movie.Changelogs)
                .Include(movie => movie.Credits)
                .ThenInclude(credits => credits.Person)
                .Include(movie => movie.Credits)
                .ThenInclude(credits => credits.Function)
                .Where(x => x.Id == movieId)
                .IgnoreQueryFilters()
                .FirstOrDefault();
            return movie;
        }

        public List<Movie> GetMovies(List<int> movieIds, bool isAdmin)
        {
            if (isAdmin)
            {
                List<Movie> movieList = _dbContext.Movies
                                .Include(movie => movie.Genres)
                                .Include(movie => movie.Image)
                                .Include(movie => movie.Credits)
                                .ThenInclude(credits => credits.Person)
                                .Include(movie => movie.Credits)
                                .ThenInclude(credits => credits.Function)
                                .Where(x => movieIds.Contains(x.Id))
                                .IgnoreQueryFilters()
                                .AsEnumerable()
                                .OrderBy(x => movieIds.IndexOf(x.Id))
                                .Distinct()
                                .ToList();
                return movieList;
            }
            else
            {
                List<Movie> movieList = _dbContext.Movies
                                .Include(movie => movie.Genres)
                                .Include(movie => movie.Image)
                                .Include(movie => movie.Credits)
                                .ThenInclude(credits => credits.Person)
                                .Include(movie => movie.Credits)
                                .ThenInclude(credits => credits.Function)
                                .Where(x => movieIds.Contains(x.Id))
                                .AsEnumerable()
                                .OrderBy(x => movieIds.IndexOf(x.Id))
                                .Distinct()
                                .ToList();
                return movieList;
            }

        }

        public List<Movie> GetMovies(bool isAdmin)
        {
            if (isAdmin)
            {
                List<Movie> moviesList = _dbContext.Movies
                    .OrderBy(movie => movie.Title)
                    .Include(movie => movie.Genres)
                    .Include(movie => movie.Image)
                    .IgnoreQueryFilters()
                    .ToList();
                return moviesList;
            }
            else
            {
                List<Movie> moviesList = (from movies in _dbContext.Movies
                                          .Include(movie => movie.Genres)
                                          .Include(movie => movie.Image)
                                          select movies).OrderBy(movie => movie.Title).ToList();
                return moviesList;
            }


        }

        public List<Genre> GetMovieGenres(int movieId)
        {
            List<Genre> genresList = _dbContext.Genres.Where(x => x.Movies.Any(y => y.Id == movieId)).ToList();
            return genresList;
        }

        public List<Movie> GetMoviesByTitle(string title)
        {
            return _dbContext.Movies.Where(movie => movie.Title == title).ToList();
        }

        public bool MovieExistsById(int id)
        {
            return _dbContext.Movies
                .IgnoreQueryFilters()
                .Any(movie => movie.Id == id);
        }

        public bool MovieExistsByTitleAndDescription(string searchString)
        {
            bool movieExist = (from movies in _dbContext.Movies where movies.Title!.Contains(searchString) || movies.Description!.Contains(searchString) select movies).Any();

            return movieExist;
        }

        public bool MovieExist(string title, DateTime releaseDate, int movieId)
        {
            IQueryable<Movie>? movies = (from movie in _dbContext.Movies where movie.Id != movieId select movie);
            return movies.Where(movie => movie.Title == title).Where(movie => movie.ReleaseDate.Value.Year == releaseDate.Year).Any();
        }

        public bool MovieHasGenre(int genreId)
        {

            IQueryable<Genre> genres = from genre in _dbContext.Genres.Include(genre => genre.Movies).ThenInclude(movie => movie.Genres) where genre.Id == genreId select genre;
            bool genreExist = (from genre in genres where genre.Movies.Any() != false select genre).Any();

            return genreExist;
        }

        public bool MovieHasPerson(int personId)
        {

            IQueryable<Movie>? movieQuery = (from movie in _dbContext.Movies.Include(movie => movie.Credits).ThenInclude(credits => credits.Person) select movie);
            if (movieQuery.Where(movie => movie.Credits.Where(credit => credit.Person.Id == personId).Any() == true).Any())
            {
                return true;
            }
            return false;
        }

        public string GetImagePath(string imageName)
        {
            string path = "";

            if (imageName != null)
            {
                path = $"images\\{imageName}";
            }

            return path;
        }

        public string GetThumbnailImagePath(string imageName)
        {
            string path = "";

            if (imageName != null)
            {
                path = $"images\\thumbnails\\{imageName}";
            }

            return path;
        }

        public bool IsImage(IBrowserFile? Image)
        {
            if (!string.Equals(Image.ContentType, "image/jpg") &&
               !string.Equals(Image.ContentType, "image/jpeg") &&
               !string.Equals(Image.ContentType, "image/pjpeg") &&
               !string.Equals(Image.ContentType, "image/gif") &&
               !string.Equals(Image.ContentType, "image/x-png") &&
               !string.Equals(Image.ContentType, "image/png"))
            {
                return false;
            }
            return true;
        }

        public List<Movie> LuceneGetMovies(string? searchString, bool isAdmin)
        {
            const LuceneVersion luceneVersion = LuceneVersion.LUCENE_48;
            Lucene.Net.Analysis.Analyzer standardAnalyzer = new StandardAnalyzer(luceneVersion);


            //Open the Directory using a Lucene Directory class
            string indexName = "Movies_index";
            string indexPath = Path.Combine(Environment.CurrentDirectory, indexName);

            using LuceneDirectory indexDir = FSDirectory.Open(indexPath);


            using IndexReader reader = DirectoryReader.Open(indexDir);
            IndexSearcher searcher = new IndexSearcher(reader);

            TopDocs? topDocs = null;


            MultiFieldQueryParser parser = new MultiFieldQueryParser(luceneVersion, new string[] { "Title", "Description", "Genre" }, standardAnalyzer);
            Query query = parser.Parse(searchString);


            topDocs = searcher.Search(query, n: 10);

            List<int> movieIds = new List<int>();

            if (topDocs != null)
            {
                for (int i = 0; i < topDocs.ScoreDocs.Length; i++)
                {
                    Document resultDoc = searcher.Doc(topDocs.ScoreDocs[i].Doc);

                    int id = Int32.Parse(resultDoc.Get("Id"));
                    movieIds.Add(id);
                }

            }

            reader.Dispose();
            if (movieIds.Count > 0)
            {
                return GetMovies(movieIds, isAdmin);
            }
            else
            {
                return new List<Movie>();
            }

        }

        public List<MarkupString> HighlightDescription(string description, string searchString)
        {
            const LuceneVersion luceneVersion = LuceneVersion.LUCENE_48;
            Lucene.Net.Analysis.Analyzer standardAnalyzer = new StandardAnalyzer(luceneVersion);


            //Open the Directory using a Lucene Directory class
            string indexName = "Description_index";
            string indexPath = Path.Combine(Environment.CurrentDirectory, indexName);

            using LuceneDirectory indexDir = FSDirectory.Open(indexPath);

            IndexWriterConfig indexConfig = new IndexWriterConfig(luceneVersion, standardAnalyzer);
            indexConfig.OpenMode = OpenMode.CREATE;                             // create/overwrite index
            IndexWriter writer = new IndexWriter(indexDir, indexConfig);

            Document doc = new Document();
            doc.Add(new TextField("Description", description, Field.Store.YES));
            writer.AddDocument(doc);

            writer.Commit();

            using IndexReader reader = DirectoryReader.Open(indexDir);
            IndexSearcher searcher = new IndexSearcher(reader);


            QueryParser parser = new QueryParser(luceneVersion, "Description", classicAnalyzer);
            Query query = parser.Parse(searchString);
            TopDocs topDocs = searcher.Search(query, n: 10);

            List<Movie> movies = new List<Movie>();

            QueryScorer scorer = new QueryScorer(query);

            SimpleHTMLFormatter formatter = new SimpleHTMLFormatter("<span style='color:maroon; font-weight:bold;'>", "</span>");
            Highlighter highlighter = new Highlighter(formatter, scorer);
            List<MarkupString> descriptionList = new List<MarkupString>();
            string[] fragment = null;
            for (int i = 0; i < topDocs.TotalHits; i++)
            {
                int docId = topDocs.ScoreDocs[i].Doc;
                Document resultDoc = searcher.Doc(topDocs.ScoreDocs[i].Doc);

                string text = resultDoc.Get("Description");

                TokenStream stream = TokenSources.GetAnyTokenStream(reader, docId, "Description", resultDoc, standardAnalyzer);
                IFragmenter fragmenter = new SimpleSpanFragmenter(scorer);
                highlighter.TextFragmenter = fragmenter;
                fragment = highlighter.GetBestFragments(stream, text, 2);


            }
            if (fragment != null)
            {
                foreach (var highlight in fragment)
                {
                    descriptionList.Add((MarkupString)highlight);
                }
            }

            writer.Dispose();
            reader.Dispose();

            return descriptionList;
        }

        public string GetFirstTenWords(string description)
        {
            var descriptionWithoutMarkup = Regex.Replace(description, "<.*?>", String.Empty);
            var movieDescription = descriptionWithoutMarkup.Split(" ");

            if (movieDescription.Length < 10)
            {

                movieDescription = movieDescription.Take(movieDescription.Length).ToArray();

                var finalMovieDescription = string.Join(" ", movieDescription);
                return finalMovieDescription;
            }
            else
            {
                string[] movieDescriptionTemp = new string[11];
                for (int i = 0; i < movieDescriptionTemp.Length; i++)
                {
                    movieDescriptionTemp[i] = movieDescription[i];
                    if (i == 5)
                    {
                        movieDescriptionTemp[i] = "<br />";
                    }
                }
                var finalMovieDescription = string.Join(" ", movieDescriptionTemp);
                return finalMovieDescription;
            }

        }

        public void AddChangelog(Movie movie, string currentUser)
        {
            Changelog changelog = new Changelog
            {
                CreatedAt = DateTime.Now,
                CreatedBy = currentUser,
                UpdatedAt = DateTime.Now,
                UpdatedBy = currentUser,
                Movie = movie

            };
            _dbContext.Logs.Add(changelog);
            _dbContext.SaveChanges();
        }

        public void UpdateChangelog(Movie movie, string currentUser)
        {
            Changelog changelog = new Changelog
            {
                CreatedAt = movie.Changelogs.ElementAt(0).CreatedAt,
                CreatedBy = movie.Changelogs.ElementAt(0).CreatedBy,
                UpdatedAt = DateTime.Now,
                UpdatedBy = currentUser,
                Movie = movie

            };

            _dbContext.Logs.Add(changelog);
            _dbContext.SaveChanges();
        }

        public void DeleteChangelog(Movie movie, string currentUser)
        {
            Changelog changelog = new Changelog
            {
                CreatedAt = movie.Changelogs.ElementAt(0).CreatedAt,
                CreatedBy = movie.Changelogs.ElementAt(0).CreatedBy,
                DeletedAt = DateTime.Now,
                DeletedBy = currentUser,
                Movie = movie

            };

            _dbContext.Logs.Add(changelog);
            _dbContext.SaveChanges();
        }


    }
}
