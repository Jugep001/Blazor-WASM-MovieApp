using Blazor_WASM_MovieApp.Data;
using Blazor_WASM_MovieApp.Models;
using Microsoft.EntityFrameworkCore;

namespace Blazor_WASM_MovieApp.Repositories
{
    public class CreditRepository
    {
        private readonly BlazorMovieContext _dbContext;
        public CreditRepository(BlazorMovieContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddCredit(Credit credit)
        {
            if (_dbContext.Movies.Any(movie => movie.Credits.Contains(credit)))
            {
                _dbContext.Update(credit);
                _dbContext.SaveChanges();
                return;
            }
            _dbContext.Add(credit);
            _dbContext.SaveChanges();

        }

        public void AddCredit_Create(Credit credit)
        {
            if (_dbContext.Movies.Any(movie => movie.Credits.Contains(credit)))
            {
                return;
            }
            _dbContext.Add(credit);
            _dbContext.SaveChanges();

        }

        public void UpdateCredit(Credit credit)
        {
            _dbContext.Credits.Update(credit);
            _dbContext.SaveChanges();
        }

        public void DeleteCredit(int creditId)
        {
            Credit credit = _dbContext.Credits.Find(creditId);
            _dbContext.Credits.Remove(credit);
            _dbContext.SaveChanges();
        }

        public List<Credit> GetCredits()
        {
            List<Credit> creditsList = (from credits in _dbContext.Credits.Include(credit => credit.Function) select credits)
                .OrderBy(credit => credit.Function.FunctionName)
                .ToList();

            return creditsList;
        }

        public List<Function> GetFunctions()
        {
            List<Function> functionsList = (from functions in _dbContext.Functions select functions).ToList();
            return functionsList;
        }

        public Credit GetCredit(int creditId)
        {
            Credit? selectedCredit = _dbContext.Credits.Where(credits => credits.Id == creditId).First();

            return selectedCredit;
        }


        public Function GetFunction(int functionId)
        {
            Function? selectedFunction = _dbContext.Functions.Where(function => function.Id == functionId).First();

            return selectedFunction;
        }

        public List<Credit> GetCreditsByName(string searchString)
        {

            List<Credit> creditList = (from credits in _dbContext.Credits select credits).ToList();

            if (searchString != null)
            {
                creditList = (from credits in _dbContext.Credits.Include(credits => credits.Person) where credits.Person.Name.Contains(searchString) select credits).ToList();
            }
            return creditList;

        }

        public bool CreditExist(ICollection<Credit> creditQuery, int personId, int functionId, string? role, Credit? oldCredit)
        {
            if (oldCredit != null)
            {
                List<Credit> creditQueryTemp = new List<Credit>(creditQuery);
                creditQueryTemp.Remove(oldCredit);
                return creditQueryTemp
                .Where(credit => credit.PersonId == personId)
                .Where(credit => credit.FunctionId == functionId)
                .Any();
            }



            return creditQuery
           .Where(credit => credit.PersonId == personId)
           .Where(credit => credit.FunctionId == functionId)
           .Any();



        }

        public bool CreditExist(int personId, int functionId, int? id)
        {
            IQueryable<Credit>? creditQuery = from credits in _dbContext.Credits where credits.Id != id select credits;

            return creditQuery
                .Where(credit => credit.PersonId == personId)
                .Where(credit => credit.FunctionId == functionId)
                .Any();

        }

        public bool CreditExist(int creditId)
        {
            bool creditExist = (from credits in _dbContext.Credits where credits.Id == creditId select credits).Any();
            return creditExist;
        }

        public List<Credit> GetCreditsFromMovie(int movieId)
        {
            var Query = from credits in _dbContext.Credits where credits.MovieId == movieId select credits;
            Query = Query.Include(credit => credit.Person).Include(credit => credit.Function);
            Query = Query.OrderBy(x => x.Function.FunctionName).ThenBy(x => x.Order);
            return Query.ToList();
        }


    }
}
