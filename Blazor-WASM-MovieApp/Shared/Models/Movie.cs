using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blazor_WASM_MovieApp.Models
{
    public class Movie
    {
        public int Id { get; set; }



        [StringLength(60, MinimumLength = 3)]
        public string? Title { get; set; }

        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        public DateTime? ReleaseDate { get; set; }

        [Range(1, 100)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Price { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s-]*$")]
        [StringLength(10)]
        public string? Rating { get; set; }

        public string Description { get; set; } = string.Empty;

        public string ShortDescription { get; set; } = string.Empty;

        public bool IsDeleted { get; set; } = false;
        
        public ICollection<Genre>? Genres { get; set; }

        public virtual ICollection<Credit> Credits { get; set; }

        public virtual Image? Image { get; set; } = null;

        public virtual ICollection<Changelog> Changelogs { get; set; } = new List<Changelog>();
    }
}
