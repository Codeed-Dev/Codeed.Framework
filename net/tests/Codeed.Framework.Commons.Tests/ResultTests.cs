using Codeed.Framework.Commons.Tests.Utils;
using System;
using Xunit;

namespace Codeed.Framework.Commons.Tests
{
    public class ResultTests
    {
        public class Try
        {
            [Fact]
            public void should_return_succeeded_result_when_action_does_not_throw()
            {
                // Arrange & Act
                var result = Result.Try(() => { });

                // Assert
                Assert.True(result);
                Assert.True(result.Value);
            }

            [Fact]
            public void should_return_failed_result_when_action_throws()
            {
                // Arrange
                Action action = () =>
                {
                    throw new ResultNetTestException();
                };

                // Act
                var result = Result.Try(action);

                // Assert
                Assert.False(result);
                Assert.False(result.Value);
            }
        }

        public class Ok
        {
            [Fact]
            public void should_return_succeeded_result()
            {
                // Arrange & Act
                var result = Result.Ok();

                // Assert
                Assert.True(result);
            }
        }

        public class Constructor
        {

            [Fact]
            public void succeeded_result_should_return_true_value()
            {
                // Arrange & Act
                var result = new Result();

                // Assert
                Assert.True(result.Value);
            }

            [Fact]
            public void failed_result_should_return_false_value()
            {
                // Arrange & Act
                var result = new Result("Error");

                // Assert
                Assert.False(result.Value);
            }
        }
    }
}
