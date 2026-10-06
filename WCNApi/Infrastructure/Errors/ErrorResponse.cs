
namespace WCNApi.Infrastructure.Errors
{
    public class ErrorResponse
    {
        public string Message { get; }
        public ErrorCode ErrorCode { get; }

        public ErrorResponse(ErrorCode errorCode, string message)
        {
            ErrorCode = errorCode;
            Message = message;
        }
    }
}
