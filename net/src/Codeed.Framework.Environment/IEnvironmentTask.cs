using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Influencer.Core.Environment
{
    public interface IEnvironmentTask
    {
        string Name { get; }

        Task ExecuteAsync(IServiceScope scope, CancellationToken cancellationToken);
    }
}
