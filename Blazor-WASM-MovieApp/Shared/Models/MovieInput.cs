using Blazor_WASM_MovieApp.Models;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor_WASM_MovieApp.Shared.Models
{
    public class MovieInput
    {

        public Movie Movie { get; set; }
        public Image? Image { get; set; }
        public List<int> GenreIds { get; set; }
        public List<Credit> DeleteCreditList { get; set; }
        public string? CurrentUser { get; set; }
        public bool ShouldDelete { get; set; }

    }
}
