using Schlechtums.Core.Common.Extensions;
using Xunit;

namespace Schlechtums.Core.UnitTest
{
    public class PascalCaseTests
    {
        [Fact]
        public void SnakeCaseToPascalCase()
        {
            var str = "helloWorld";
            Assert.Equal("Helloworld", str.SnakeCaseToPascalCase());
        }

        [Fact]
        public void JsonAttributeToPascalCase()
        {
            var str = "hello_world";
            Assert.Equal("HelloWorld", str.SnakeCaseToPascalCase());
        }
    }
}
