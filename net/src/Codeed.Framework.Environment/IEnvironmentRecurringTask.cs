using System;

namespace Codeed.Framework.Environment
{
    public interface IEnvironmentRecurringTask : IEnvironmentTask
    {
        TimeSpan Interval { get; }
    }
}
