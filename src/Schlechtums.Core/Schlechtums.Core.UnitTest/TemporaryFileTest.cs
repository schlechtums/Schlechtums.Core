using Schlechtums.Core.Common;
using Schlechtums.Core.Common.Extensions;
using System;
using System.IO;
using System.Text;
using NUnit.Framework;
using FluentAssertions;

namespace Schlechtums.Core.UnitTest
{

    public class TemporaryFileTest
    {
        private string testText = @"Hello
World!";

        private string[] testLines = new[] { "Hello", "World!" };

        private byte[] testBytes = @"Hello
World!".GetBytes();

        private Encoding _TestEncoding = Encoding.ASCII;

        [Test]
        public void TextTest()
        {
            using (var tf = new TemporaryFile())
            {
                tf.WriteAllText(this.testText);

                tf.ReadAllText().Should().Be(this.testText);
                tf.ReadAllText().Should().Be(File.ReadAllText(tf.FullPath));
            }
        }

        [Test]
        public void TextEncodingTest()
        {
            using (var tf = new TemporaryFile())
            {
                tf.WriteAllText(this.testText, this._TestEncoding);

                tf.ReadAllText(this._TestEncoding).Should().Be(this.testText);
                tf.ReadAllText(this._TestEncoding).Should().Be(File.ReadAllText(tf.FullPath, this._TestEncoding));
            }
        }

        [Test]
        public void LinesTest()
        {
            using (var tf = new TemporaryFile())
            {
                tf.WriteAllLines(this.testLines);

                tf.ReadAllLines().JoinWithNewline().Should().Be(this.testLines.JoinWithNewline());
                tf.ReadAllLines().JoinWithNewline().Should().Be(File.ReadAllLines(tf.FullPath).JoinWithNewline());
            }
        }

        [Test]
        public void LinesEncodingTest()
        {
            using (var tf = new TemporaryFile())
            {
                tf.WriteAllLines(this.testLines, this._TestEncoding);

                tf.ReadAllLines(this._TestEncoding).JoinWithNewline().Should().Be(this.testLines.JoinWithNewline());
                tf.ReadAllLines(this._TestEncoding).JoinWithNewline().Should().Be(File.ReadAllLines(tf.FullPath, this._TestEncoding).JoinWithNewline());
            }
        }

        [Test]
        public void BytesTest()
        {
            using (var tf = new TemporaryFile())
            {
                tf.WriteAllBytes(this.testBytes);

                this.ByteArraysAreSame(this.testBytes, tf.ReadAllBytes()).Should().BeTrue();
                this.ByteArraysAreSame(File.ReadAllBytes(tf.FullPath), tf.ReadAllBytes()).Should().BeTrue();
            }
        }

        [Test]
        public void TextStaticTest()
        {
            using (var tf = TemporaryFile.WithContent(this.testText))
            {
                tf.ReadAllText().Should().Be(this.testText);
                tf.ReadAllText().Should().Be(File.ReadAllText(tf.FullPath));
            }
        }

        [Test]
        public void LinesStaticTest()
        {
            using (var tf = TemporaryFile.WithContent(this.testLines))
            {
                tf.ReadAllLines().JoinWithNewline().Should().Be(this.testLines.JoinWithNewline());
                tf.ReadAllLines().JoinWithNewline().Should().Be(File.ReadAllLines(tf.FullPath).JoinWithNewline());
            }
        }

        [Test]
        public void BytesStaticTest()
        {
            using (var tf = TemporaryFile.WithContent(this.testBytes))
            {
                Assert.True(this.ByteArraysAreSame(this.testBytes, tf.ReadAllBytes()));
                Assert.True(this.ByteArraysAreSame(File.ReadAllBytes(tf.FullPath), tf.ReadAllBytes()));
            }
        }

        private bool ByteArraysAreSame(byte[] left, byte[] right)
        {
            if (left.Length != right.Length)
                return false;

            for (int i = 0; i < left.Length; i++)
            {
                if (left[i] != right[i])
                    return false;
            }

            return true;
        }
    }
}