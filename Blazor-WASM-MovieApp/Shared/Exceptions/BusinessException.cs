namespace Blazor_WASM_MovieApp.Exceptions
{
    public class BusinessException : Exception
    {
        public List<ErrorItem> ExceptionMessageList { get; set; }

        public BusinessException(List<ErrorItem> exceptionMessageList)
        {
            this.ExceptionMessageList = exceptionMessageList;
        }

        public BusinessException(String errorMessage, String PropertyName)
        {
            this.ExceptionMessageList = new List<ErrorItem>();
            this.ExceptionMessageList.Add
                (

                new ErrorItem(errorMessage, PropertyName)

                );
        }

        public override string Message
        {
            get
            {
                String messages = String.Empty;
                foreach (ErrorItem? err in this.ExceptionMessageList)
                {
                    messages = messages + err + Environment.NewLine;
                }
                return messages;
            }
        }
    }
    public class ErrorItem
    {
        public String PropertyName { get; set; }
        public String ErrorMessage { get; set; }

        public ErrorItem(string propertyName, string errorMessage)
        {
            this.PropertyName = propertyName;
            this.ErrorMessage = errorMessage;
        }
    }

}




