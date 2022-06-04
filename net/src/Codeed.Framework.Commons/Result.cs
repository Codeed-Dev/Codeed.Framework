using System;
using System.Threading.Tasks;

namespace Codeed.Framework.Commons
{
    public class Result : Result<bool>
    {

        public static Result Try(Action action)
        {
            return Try(() =>
            {
                action();
                return true;
            });
        }

        public static async Task<Result> TryAsync(Func<Task> action)
        {
            return await TryAsync(async () =>
            {
                await action().ConfigureAwait(false);
                return true;
            }).ConfigureAwait(false);
        }

        public static Result Ok()
        {
            return new Result();
        }

        public Result()
        {

        }

        public Result(string errorMessage) : base(errorMessage)
        {

        }

        public override bool Value => this;

    }
}