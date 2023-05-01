namespace OrderSystemOCS.WebApi.Exceptions
{
    public sealed class WebApiException : Exception
    {
        public WebApiException(string message)
            : base(message) { }
        public WebApiException(string message, Exception inner)
            : base(message, inner) { }
    }
}
