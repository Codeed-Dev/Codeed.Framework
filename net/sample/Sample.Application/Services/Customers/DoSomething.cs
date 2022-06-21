using Codeed.Framework.Services;
using Microsoft.AspNetCore.Mvc;

namespace Sample.Application.Services.Customers
{
    [Route("api/do-something")]
    public class DoSomething : HttpService
        .WithParameters<string>
        .WithResponse<string>
    {
        [HttpPost]
        public override Task<string> ExecuteAsync(string parameter, CancellationToken cancellationToken)
        {
            return Task.FromResult(parameter + " final.");
        }
    }
}
