using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blazor_WASM_MovieApp.Models
{
    public class Credit
    {
        public int? Id { get; set; }

        public int? MovieId { get; set; }
        public virtual Movie? Movie { get; set; }

        public int PersonId { get; set; }
        public virtual Person? Person { get; set; }

        public int FunctionId { get; set; }
        public virtual Function? Function { get; set; }

        public string? Role { get; set; }

        public int? Position { get; set; }

        public bool IsDragOver { get; set; }


    }
}
