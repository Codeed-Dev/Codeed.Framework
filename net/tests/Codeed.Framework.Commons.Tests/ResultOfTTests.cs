using Codeed.Framework.Commons.Tests.Utils;
using System;
using System.Linq;
using Xunit;

namespace Codeed.Framework.Commons.Tests
{
    public class ResultOfTTests
    {
        public class Operators
        {
            [Fact]
            public void should_return_true_when_result_succeeded()
            {
                // Arrange & Act
                var result = new Result<bool>();

                // Assert
                Assert.True(result.Succeeded);
                Assert.True(result);
            }

            [Fact]
            public void should_return_false_when_result_failed()
            {
                // Arrange & Act
                var result = new Result<bool>("Error");

                // Assert
                Assert.True(result.Failed);
                Assert.False(result);
            }

            [Fact]
            public void should_cast_succeeded_result_of_t_to_result()
            {
                // Arrange
                var resultOfT = new Result<string>();

                // Act
                var result = (Result)resultOfT;

                // Assert
                Assert.True(result);
            }
        }

        public class Constructor
        {
            [Fact]
            public void should_initialize_with_succeeded_result()
            {
                // Arrange & Act
                var result = new Result<bool>();

                // Assert
                Assert.True(result.Succeeded);
                Assert.False(result.Failed);
            }

            [Fact]
            public void should_initialize_with_empty_error_list()
            {
                // Arrange & Act
                var result = new Result<bool>();

                // Assert
                Assert.Empty(result.Errors);
            }

            [Fact]
            public void should_initialize_with_failed_result_when_error_message_argument_exists()
            {
                // Arrange & Act
                var result = new Result<bool>("Error");

                // Assert
                Assert.True(result.Failed);
                Assert.Single(result.Errors);
                Assert.Equal("Error", result.Errors.First());
            }
        }

        public class Try
        {
            [Fact]
            public void should_return_succeeded_result_when_action_does_not_throw()
            {
                // Arrange 
                var resultValue = "value";
                Func<string> action = () => resultValue;

                // Act
                var result = Result<string>.Try(action);

                // Assert
                Assert.True(result);
                Assert.Equal(resultValue, result.Value);
            }

            [Fact]
            public void should_return_failed_result_when_action_throws()
            {
                // Arrange 
                Func<string> action = () =>
                {
                    throw new ResultNetTestException();
                };

                // Act
                var result = Result<string>.Try(action);

                // Assert
                Assert.False(result);
                Assert.Null(result.Value);
            }
        }

        public class Ok
        {
            [Fact]
            public void should_set_primitive_type_value()
            {
                // Arrange
                var result = new Result<string>();
                var value = "Test";

                // Act
                result.Ok(value);

                // Assert
                Assert.True(result);
                Assert.Equal(value, result.Value);
            }

            [Fact]
            public void should_set_complex_type_value()
            {
                // Arrange
                var result = new Result<SimpleClass>();
                var value = new SimpleClass
                {
                    Age = 20,
                    Name = "Roger"
                };

                // Act
                result.Ok(value);

                // Assert
                Assert.True(result);
                Assert.Equal(value, result.Value);
            }

            [Fact]
            public void should_clean_error_list()
            {
                // Arrange
                var result = new Result<string>();
                result.AddError("Error 1");
                result.AddError("Error 2");
                var value = "Test";

                // Act
                result.Ok(value);

                // Assert
                Assert.True(result);
                Assert.Equal(value, result.Value);
                Assert.Empty(result.Errors);
            }
        }

        public class AddError
        {
            [Fact]
            public void should_add_error_string_on_result_error_list()
            {
                // Arrange
                var result = new Result<string>();
                var error = "Error";

                // Act
                result.AddError(error);

                // Assert
                Assert.Single(result.Errors);
                Assert.Equal(error, result.Errors.First());
            }

            [Fact]
            public void should_add_error_exception_on_result_error_list()
            {
                // Arrange
                var result = new Result<string>();
                var error = new ResultNetTestException();

                // Act
                result.AddError(error);

                // Assert
                Assert.Single(result.Errors);
                Assert.Equal(error.Message, result.Errors.First());
            }

            [Fact]
            public void should_throw_when_error_message_is_null()
            {
                // Arrange
                var result = new Result<string>();
                string error = null;

                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => result.AddError(error));
            }

            [Fact]
            public void should_throw_when_error_message_is_empty()
            {
                // Arrange
                var result = new Result<string>();
                string error = string.Empty;

                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => result.AddError(error));
            }

            [Fact]
            public void should_throw_when_error_exception_is_null()
            {
                // Arrange
                var result = new Result<string>();
                Exception error = null;

                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => result.AddError(error));
            }


            [Fact]
            public void should_add_error_with_code_and_parameters()
            {
                // Arrange
                var result = new Result<string>();

                // Act
                result.AddError("Error message", "error/code", new { param1 = "parameter 1" });

                // Assert
                Assert.Collection(result.ErrorCodes,
                    (first) =>
                    {
                        Assert.Equal("error/code", first.ErrorCode);
                        Assert.Collection(first.Parameters,
                            (firstParameters) =>
                            {
                                Assert.Equal("param1", firstParameters.Key);
                                Assert.Equal("parameter 1", firstParameters.Value);
                            });
                    });
            }

            [Fact]
            public void should_add_error_with_code()
            {
                // Arrange
                var result = new Result<string>();

                // Act
                result.AddError("Error message", "error/code");

                // Assert
                Assert.Collection(result.ErrorCodes,
                    (first) =>
                    {
                        Assert.Equal("error/code", first.ErrorCode);
                        Assert.Empty(first.Parameters);
                    });
            }
        }

        public class Cast
        {
            [Fact]
            public void should_cast_failed_result_errors()
            {
                // Arrange
                var result = new Result<bool>();
                result.Add("Error 1");
                result.Add("Error 2");

                // Act
                var castResult = result.Cast<SimpleClass>();

                // Assert
                Assert.NotEmpty(castResult.Errors);
                Assert.False(castResult);
                Assert.Collection(castResult.Errors,
                    (error1) => Assert.Equal("Error 1", error1),
                    (error2) => Assert.Equal("Error 2", error2));
            }

            [Fact]
            public void should_cast_succeeded_result()
            {
                // Arrange
                var result = new Result<bool>();

                // Act
                var castResult = result.Cast<SimpleClass>();

                // Assert
                Assert.Empty(castResult.Errors);
                Assert.True(castResult);
            }
        }
    }
}
