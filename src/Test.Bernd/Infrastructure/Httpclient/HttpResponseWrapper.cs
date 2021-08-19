using System.Net.Http;

namespace Eurocontrol.Vmp.Api.Infrastructure.HttpClient
{
    public class HttpResponseWrapper<T, Y>
    {
        public HttpResponseMessage ResponseMessage { get; }
        
        public T? ParsedResponse { get; }
        
        public Y? ErrorResponse { get; }

        public HttpResponseWrapper(HttpResponseMessage responseMessage, T? parsedResponse, Y? errorResponse)
        {
            ResponseMessage = responseMessage;
            ParsedResponse = parsedResponse;
            ErrorResponse = errorResponse;
        }
    }
}