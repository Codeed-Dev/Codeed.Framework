using Xunit;

namespace Codeed.Framework.Concurrency.Tests
{
    public class InMemoryLockerTest
    {
        [Fact]
        public async Task should_wait_for_lock_to_execute()
        {
            bool lockAcquired = false;
            

            Task task1 = Task.Run(async () =>
            {
                var locker = new InMemoryLocker();
                using (await locker.AcquireLockAsync("test", TimeSpan.FromSeconds(5), CancellationToken.None))
                {
                    Thread.Sleep(3000);
                    lockAcquired = true;
                }
            });

            Task task2 = Task.Run(async () =>
            {
                Thread.Sleep(100);
                var locker = new InMemoryLocker();
                using (var a = await locker.AcquireLockAsync("test", TimeSpan.FromSeconds(5), CancellationToken.None))
                {
                    Assert.True(lockAcquired);
                }
            });

            await Task.WhenAll(task1, task2);
        }
    }
}