
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WCNApi.Infrastructure.Errors
{

    public class ValidationResultModel
    {
        public string Message { get; }
        public ErrorCode ErrorCode { get; }
        public List<ValidationError> Errors { get; }

        public ValidationResultModel(ModelStateDictionary modelState)
        {
            Message = "Validation Failed";
            ErrorCode = ErrorCode.ValidationModelError;
            Errors = modelState.Keys
                    .SelectMany(key => modelState[key]!.Errors.Select(x => new ValidationError(key, x.ErrorMessage)))
                    .ToList();
        }
    }
}
