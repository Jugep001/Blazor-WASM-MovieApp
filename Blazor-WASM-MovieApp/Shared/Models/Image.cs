namespace Blazor_WASM_MovieApp.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string? ImageName { get; set; }
        public string? Path { get; set; }

        public string? ThumbnailPath { get; set; }
        public long Size { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}
