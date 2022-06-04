using System;

namespace Influencer.Core.Environment
{
    public interface IEnvironmentRecurringTask : IEnvironmentTask
    {
        TimeSpan Interval { get; }
    }
}
