using Schlechtums.Core.Common.Extensions;
using Xunit;

namespace Schlechtums.Core.UnitTest
{
    public class CapitalizeTests
    {
        [Fact]
        public void NoSeparator()
        {
            var str = "helloWorld";
            Assert.Equal("Helloworld", str.SnakeCaseToPascalCase());
        }

        [Fact]
        public void JsonAttributeCapitalized()
        {
            var str = "hEllo_world";
            Assert.Equal("Hello_World", str.Capitalize(true, '_'));
        }

        [Fact]
        public void JsonAttributeCapitalizedDoNotKeepDelimiter()
        {
            var str = "hEllo_world";
            Assert.Equal("HelloWorld", str.Capitalize(false, '_'));
        }
    }
}
