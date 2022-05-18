using FluentAssertions;
using NUnit.Framework;
using Schlechtums.Core.Common.Extensions;
using System.Text;

namespace Schlechtums.Core.UnitTest
{
    public class ByteExtensionsTests
    {
        private readonly byte[] testBytes = "HelloWorld".GetBytes();
        private readonly byte[] nullBytes = null;
        private readonly byte[] testbytesWithEncoding = "HelloWorld".GetBytes(Encoding.UTF32);

        [Test]
        public void GetString_Should_ReturnString()
        {
            this.testBytes.GetString().Should().Be("HelloWorld");
        }

        [Test]
        public void GetStringSafe_Should_ReturnString_WhenValued()
        {
            this.testBytes.GetStringSafe().Should().Be("HelloWorld");
        }

        [Test]
        public void GetStringSafe_Should_ReturnNull_WhenNull()
        {
            this.nullBytes.GetStringSafe().Should().BeNull();
        }

        [Test]
        public void GetStringWithEncoding_Should_ReturnString_WhenEncodingCorrect()
        {
            this.testbytesWithEncoding.GetStringSafe(Encoding.UTF32).Should().Be("HelloWorld");
        }

        [Test]
        public void GetStringWithEncoding_Should_ReturnGibberish_WhenEncodingIncorrect()
        {
            this.testbytesWithEncoding.GetStringSafe(Encoding.UTF8).Should().NotBe("HelloWorld");
        }
    }
}
