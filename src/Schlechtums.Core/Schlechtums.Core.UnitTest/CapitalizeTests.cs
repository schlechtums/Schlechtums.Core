using Schlechtums.Core.Common.Extensions;
using NUnit.Framework;
using FluentAssertions;

namespace Schlechtums.Core.UnitTest
{
    public class CapitalizeTests
    {
        [Test]
        public void NoSeparator()
        {
            var str = "helloWorld";
            str.SnakeCaseToPascalCase().Should().Be("Helloworld");
        }

        [Test]
        public void JsonAttributeCapitalized()
        {
            var str = "hEllo_world";
            str.Capitalize(true, '_').Should().Be("Hello_World");
        }

        [Test]
        public void JsonAttributeCapitalizedDoNotKeepDelimiter()
        {
            var str = "hEllo_world";
            str.Capitalize(false, '_').Should().Be("HelloWorld");
        }
    }
}
