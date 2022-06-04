using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Influencer.Core.Environment
{
    public class TaskRunner
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<TaskRunner> _logger;
        private CancellationTokenSource _cancellationTokenSource;
        private object _lock = new object();
        private TimeSpan _lastDurationTime;
        private DateTimeOffset? _lastExecution;
        private DateTimeOffset? _nextExecution;
        private Action<IEnvironmentTask> _onFinishRunning;

        public TaskRunner(IEnvironmentTask task, IServiceProvider serviceProvider, ILogger<TaskRunner> logger)
        {
            EnvironmentTask = task;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public IEnvironmentTask EnvironmentTask { get; }

        public void FinishRunningCallback(Action<IEnvironmentTask> onFinishRunning)
        {
            _onFinishRunning = onFinishRunning;
        }

        public void Start()
        {
            if (_cancellationTokenSource == null)
            {
                lock (_lock)
                {
                    if (_cancellationTokenSource == null)
                    {
                        _cancellationTokenSource = new CancellationTokenSource();
                        using (System.Threading.ExecutionContext.SuppressFlow())
                        {
                            Task.Run(Run);
                        }
                    }
                }
            }
        }

        public void Stop()
        {
            if (_cancellationTokenSource != null)
            {
                lock (_lock)
                {
                    if (_cancellationTokenSource != null)
                    {
                        _cancellationTokenSource.Cancel();
                        _cancellationTokenSource = null;
                    }
                }
            }
        }

        private async Task Run()
        {
            while (_cancellationTokenSource != null && !_cancellationTokenSource.Token.IsCancellationRequested)
            {
                if (_cancellationTokenSource == null || _cancellationTokenSource.Token.IsCancellationRequested)
                    break;

                _lastExecution = DateTimeOffset.Now;
                _nextExecution = null;

                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        await EnvironmentTask.ExecuteAsync(scope, _cancellationTokenSource.Token);
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Falha inesperada ao executar a task.");
                }

                _lastDurationTime = DateTimeOffset.Now - _lastExecution.Value;

                if (EnvironmentTask is IEnvironmentRecurringTask environmentTask)
                {
                    _nextExecution = DateTimeOffset.Now.Add(environmentTask.Interval);

                    try
                    {
                        var waitInterval = environmentTask.Interval - _lastDurationTime;
                        if (waitInterval.TotalMilliseconds > 0)
                        {
                            _logger.LogInformation($"Waiting {waitInterval} until next execution.");
                            await Task.Delay(waitInterval, _cancellationTokenSource != null ? _cancellationTokenSource.Token : CancellationToken.None);
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        _logger.LogInformation($"Task {EnvironmentTask.Name} was cancelled.");
                    }
                }
                else
                {
                    Stop();
                }
            }

            if (_onFinishRunning != null)
                _onFinishRunning(EnvironmentTask);
        }
    }
}
