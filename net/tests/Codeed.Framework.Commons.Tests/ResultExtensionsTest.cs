using Codeed.Framework.Commons.Tests.Utils;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Codeed.Framework.Commons.Tests
{
    public class ResultExtensionsTest
    {
        public class Add
        {
            [Fact]
            public void should_add_errors_from_anothers_failed_result()
            {
                // Arrange
                var failedResult1 = new Result<string>();
                failedResult1.AddError("Error 1");

                var failedResult2 = new Result<bool>();
                failedResult2.AddError("Error 2");

                var currentResult = new Result<SimpleClass>();

                // Act
                currentResult.Add(failedResult1);
                currentResult.Add(failedResult2);

                // Assert
                Assert.False(currentResult);
                Assert.Equal(2, currentResult.Errors.Count());
                Assert.Collection(currentResult.Errors,
                    (error1) => Assert.Equal("Error 1", error1),
                    (error2) => Assert.Equal("Error 2", error2));
            }

            [Fact]
            public void should_not_add_errors_from_anothers_succeeded_results()
            {
                // Arrange
                var succeededResult1 = new Result<string>();
                var succeededResult2 = new Result<bool>();

                var currentResult = new Result<SimpleClass>();

                // Act
                currentResult.Add(succeededResult1);
                currentResult.Add(succeededResult2);

                // Assert
                Assert.True(currentResult);
                Assert.Empty(currentResult.Errors);
            }

            [Fact]
            public void should_keep_result_value_after_add_succeeded_result()
            {
                // Arrange
                var succeededResult = new Result<string>();

                var currentResult = new Result<SimpleClass>();
                var resultValue = new SimpleClass { Age = 20, Name = "Roger" };
                currentResult.Ok(resultValue);

                // Act
                currentResult.Add(succeededResult);

                // Assert
                Assert.True(currentResult);
                Assert.Equal(resultValue, currentResult.Value);
            }

            [Fact]
            public void should_set_result_value_to_null_after_add_failed_result()
            {
                // Arrange
                var failedResult = new Result<string>();
                failedResult.AddError("Error 1");

                var currentResult = new Result<SimpleClass>();
                var resultValue = new SimpleClass { Age = 20, Name = "Roger" };
                currentResult.Ok(resultValue);

                // Act
                currentResult.Add(failedResult);

                // Assert
                Assert.False(currentResult);
                Assert.Null(currentResult.Value);
            }

            [Fact]
            public void should_add_string_error_message()
            {
                // Arrange
                var result = new Result<string>();
                var errorMessage = "Error";

                // Act
                result.Add(errorMessage);

                // Assert
                Assert.False(result);
                Assert.Single(result.Errors);
                Assert.Equal(errorMessage, result.Errors.First());
            }

            [Fact]
            public void should_add_string_multiple_error_messages()
            {
                // Arrange
                var result = new Result<string>();

                // Act
                result.Add("Error 1", "Error 2", "Error 3");

                // Assert
                Assert.False(result);
                Assert.Collection(result.Errors,
                    (error1) => Assert.Equal("Error 1", error1),
                    (error2) => Assert.Equal("Error 2", error2),
                    (error3) => Assert.Equal("Error 3", error3));
            }

            [Fact]
            public void should_add_exception_error()
            {
                // Arrange
                var result = new Result<string>();
                var exception = new ResultNetTestException();

                // Act
                result.Add(exception);

                // Assert
                Assert.False(result);
                Assert.Single(result.Errors);
                Assert.Equal(exception.Message, result.Errors.First());
            }

            [Fact]
            public void should_add_errors_message_from_error_message_list()
            {
                // Arrange
                var result = new Result<string>();
                var errorList = new List<string>();
                int errorCount = 5;

                for (int i = 0; i < errorCount; i++)
                {
                    errorList.Add($"Error {i}");
                }

                // Act
                result.Add(errorList);

                // Assert
                Assert.False(result);
                Assert.Equal(errorCount, result.Errors.Count());

                for (int i = 0; i < errorCount; i++)
                {
                    Assert.Equal($"Error {i}", result.Errors.ElementAt(i));
                }

            }
        }
    }
}
