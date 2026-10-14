using System.Net;

namespace WCNApi.Applications.Common
{
    public class ServiceResult<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public HttpStatusCode StatusCode { get; set; } // Optional, for specifying HTTP status

        public static ServiceResult<T> SuccessResult(T data) => new ServiceResult<T> { Success = true, Data = data };
        public static ServiceResult<T> ErrorResult(string errorMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest) =>
            new ServiceResult<T> { Success = false, ErrorMessage = errorMessage, StatusCode = statusCode };
    }
}
