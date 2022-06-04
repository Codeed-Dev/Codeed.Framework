using System;
using System.Collections.Generic;

namespace Codeed.Framework.Commons
{
    public static class ResultExtensions
    {
        public static Result<T1> Add<T1, T2>(this Result<T1> result, Result<T2> resultWithErrors)
        {
            foreach (var error in resultWithErrors.Errors)
            {
                result.AddError(error);
            }

            return result;
        }

        public static Result<T> Add<T>(this Result<T> result, params string[] errorMessages)
        {
            result.Add(errorMessages as IEnumerable<string>);
            return result;
        }

        public static Result<T> Add<T>(this Result<T> result, Exception exception)
        {
            result.AddError(exception);

            return result;
        }
        public static Result<T> Add<T>(this Result<T> result, IEnumerable<string> errorMessages)
        {
            foreach (var errorMessage in errorMessages)
            {
                result.AddError(errorMessage);
            }

            return result;
        }
    }
}
