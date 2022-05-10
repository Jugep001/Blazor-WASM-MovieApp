namespace Blazor_WASM_MovieApp.Models
{
    public class Function
    {
        public int Id { get; set; } 
        public string FunctionName { get; set; }     
        public ICollection<Credit> Credits { get; set; }

        public bool IsRoleRequired { get; set; }

    }
}
