using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Codeed.Framework.Commons
{
    public class Result<T> : IResult
    {
        public static implicit operator bool(Result<T> result)
        {
            return result.Succeeded;
        }

        public static implicit operator Result(Result<T> result)
        {
            return result.Cast();
        }

        public static Result<T> Try(Func<T> action)
        {
            var result = new Result<T>();
            try
            {
                result.Ok(action());
            }
            catch (Exception e)
            {
                result.Add(e);
            }

            return result;
        }

        public static async Task<Result<T>> TryAsync(Func<Task<T>> action)
        {
            var result = new Result<T>();
            try
            {
                result.Ok(await action().ConfigureAwait(false));
            }
            catch (Exception e)
            {
                result.Add(e);
            }

            return result;
        }

        private List<ResultError> _resultErrors = new List<ResultError>();

        public Result()
        {

        }

        public Result(string errorMessage)
        {
            AddError(errorMessage);
        }

        public virtual T Value { get; private set; }

        public IEnumerable<string> Errors => ResultErrors.Select(error => error.ToString());

        public bool Succeeded => ResultErrors.Count() == 0;

        public bool Failed => !Succeeded;

        protected IEnumerable<ResultError> ResultErrors => _resultErrors;

        public Result<T> Ok(T value)
        {
            Value = value;
            _resultErrors.Clear();
            return this;
        }

        public void AddError(string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(errorMessage))
            {
                throw new ArgumentNullException(nameof(errorMessage));
            }

            _resultErrors.Add(new ResultError(errorMessage));
            Value = default;
        }

        public void AddError(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            _resultErrors.Add(new ResultError(exception));
            Value = default;
        }

        public Result<T2> Cast<T2>()
        {
            var newResult = new Result<T2>();
            newResult.SetResultErrorsList(_resultErrors);
            return newResult;
        }

        public Result Cast()
        {
            var newResult = new Result();
            newResult.SetResultErrorsList(_resultErrors);
            return newResult;
        }

        private void SetResultErrorsList(IEnumerable<ResultError> resultErrors)
        {
            _resultErrors = resultErrors.ToList();
        }
    }
}
