using Schlechtums.Core.Common.Extensions;
using NUnit.Framework;
using FluentAssertions;

namespace Schlechtums.Core.UnitTest
{
    public class PascalCaseTests
    {
        [Test]
        public void SnakeCaseToPascalCase()
        {
            var str = "helloWorld";
            str.SnakeCaseToPascalCase().Should().Be("Helloworld");
        }

        [Test]
        public void JsonAttributeToPascalCase()
        {
            var str = "hello_world";
            str.SnakeCaseToPascalCase().Should().Be("HelloWorld");
        }
    }
}
