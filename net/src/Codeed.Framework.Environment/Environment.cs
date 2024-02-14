using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Codeed.Framework.Environment
{
    public class Environment : IEnvironment
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<TaskRunner> _logger;
        private readonly IOptions<EnvironmentConfiguration> _environmentConfiguration;
        private readonly List<TaskRunner> _taskRunners = new List<TaskRunner>();

        public Environment(IServiceProvider serviceProvider, ILogger<TaskRunner> logger, IOptions<EnvironmentConfiguration> environmentConfiguration)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _environmentConfiguration = environmentConfiguration;
        }

        public IEnumerable<IEnvironmentTask> ScheduledTasks => _taskRunners.Select(r => r.EnvironmentTask);

        public string WebAppUrl => _environmentConfiguration.Value.WebAppUrl;

        public void ScheduleTask(IEnvironmentTask task)
        {
            var taskRunner = new TaskRunner(task, _serviceProvider, _logger);

            taskRunner.FinishRunningCallback(RemoveScheduledTask);

            taskRunner.Start();

            _taskRunners.Add(taskRunner);
        }

        public void RemoveScheduledTask(IEnvironmentTask task)
        {
            var taskRunner = _taskRunners.FirstOrDefault(tr => tr.EnvironmentTask == task);
            if (taskRunner == null)
            {
                return;
            }

            taskRunner.Stop();
            taskRunner.Dispose();
            _taskRunners.Remove(taskRunner);
        }
    }
}
