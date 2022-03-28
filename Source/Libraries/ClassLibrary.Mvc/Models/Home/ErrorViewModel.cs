using ClassLibrary.Mvc.Http;

namespace ClassLibrary.Mvc.Models.Home
{
    public class ErrorViewModel
    {
        public int StatusCode { get; set; } = 0;
        public string StatusMessage { get; set; } = string.Empty;
        public Exception Exception { get; set; } = new Exception("Empty Exception");
        public string RequestId { get; set; } = string.Empty;

        public ErrorViewModel(int statusCode)
        {
            KeyValuePair<int, string> kvp = StatusCodeDefinition.GetCodeDefinition(statusCode);
            StatusCode = kvp.Key;
            StatusMessage = kvp.Value;
        }
    }
}
