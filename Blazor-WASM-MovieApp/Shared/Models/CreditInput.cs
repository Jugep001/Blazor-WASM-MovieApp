using Blazor_WASM_MovieApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor_WASM_MovieApp.Shared.Models
{
    public class CreditInput
    {
        public ICollection<Credit> Credits { get; set; }
        public int PersonId { get; set; }
        public int FunctionId { get; set; }
        public string Role { get; set; }
        public Credit? OldCredit { get; set; }
    }
}
