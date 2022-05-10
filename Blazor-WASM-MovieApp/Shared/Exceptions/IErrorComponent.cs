namespace Blazor_WASM_MovieApp.Exceptions
{
    public interface IErrorComponent
    {
        void ShowError(List<ErrorItem> ErrorList);
        void HideError();
    }
}
