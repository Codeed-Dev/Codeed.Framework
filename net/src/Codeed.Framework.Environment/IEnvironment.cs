using System;
using System.Collections.Generic;

namespace Codeed.Framework.Environment
{
    public interface IEnvironment
    {
        IEnumerable<IEnvironmentTask> ScheduledTasks { get; }

        void ScheduleTask(IEnvironmentTask task);

        void RemoveScheduledTask(IEnvironmentTask task);
    }
}
