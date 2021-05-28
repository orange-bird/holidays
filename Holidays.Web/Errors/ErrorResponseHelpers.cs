using Holidays.Contracts;
using Holidays.Core;
using Microsoft.AspNetCore.Http;

namespace Holidays.Web.Errors
{
    public static class ErrorResponseHelpers
    {
        public static ErrorResponse BadRequest(BadArgumentException exception)
        {
            return new ErrorResponse { Code = StatusCodes.Status400BadRequest, Message = exception.Message, Context = exception.GetAsDictionary() };
        }
    }
}
