namespace Blazor_WASM_MovieApp.Models
{
    public class Genre
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public ICollection<Movie>? Movies { get; set; }
    }
}
