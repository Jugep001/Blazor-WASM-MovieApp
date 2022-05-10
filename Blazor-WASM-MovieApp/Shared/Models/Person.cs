using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blazor_WASM_MovieApp.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Vorname { get; set; }

        public ICollection<Credit> Credits { get; set; }

    }
}
