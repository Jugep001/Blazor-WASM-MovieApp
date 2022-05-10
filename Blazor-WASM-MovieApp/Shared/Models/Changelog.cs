namespace Blazor_WASM_MovieApp.Models
{
    public class Changelog
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
        public int? MovieId { get; set; }
        public virtual Movie? Movie { get; set; }
        
    }
}
