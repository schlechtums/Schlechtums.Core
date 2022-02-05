using Schlechtums.Core.Common;
using Schlechtums.Core.Common.Extensions;
using System;
using System.IO;
using System.Text;
using Xunit;

namespace Schlechtums.Core.UnitTest
{

    public class TemporaryFileTest
    {
        private string _TestText = @"Hello
World!";

        private string[] _TestLines = new[] { "Hello", "World!" };

        private byte[] _TestBytes = @"Hello
World!".GetBytes();

        private Encoding _TestEncoding = Encoding.ASCII;

        [Fact]
        public void TextTest()
        {
            using (var tf = new TemporaryFile())
            {
                tf.WriteAllText(this._TestText);

                Assert.Equal(this._TestText, tf.ReadAllText());
                Assert.Equal(File.ReadAllText(tf.FullPath), tf.ReadAllText());
            }
        }

        [Fact]
        public void TextEncodingTest()
        {
            using (var tf = new TemporaryFile())
            {
                tf.WriteAllText(this._TestText, this._TestEncoding);

                Assert.Equal(this._TestText, tf.ReadAllText(this._TestEncoding));
                Assert.Equal(File.ReadAllText(tf.FullPath, this._TestEncoding), tf.ReadAllText(this._TestEncoding));
            }
        }

        [Fact]
        public void LinesTest()
        {
            using (var tf = new TemporaryFile())
            {
                tf.WriteAllLines(this._TestLines);

                Assert.Equal(this._TestLines.JoinWithNewline(), tf.ReadAllLines().JoinWithNewline());
                Assert.Equal(File.ReadAllLines(tf.FullPath).JoinWithNewline(), tf.ReadAllLines().JoinWithNewline());
            }
        }

        [Fact]
        public void LinesEncodingTest()
        {
            using (var tf = new TemporaryFile())
            {
                tf.WriteAllLines(this._TestLines, this._TestEncoding);

                Assert.Equal(this._TestLines.JoinWithNewline(), tf.ReadAllLines(this._TestEncoding).JoinWithNewline());
                Assert.Equal(File.ReadAllLines(tf.FullPath, this._TestEncoding).JoinWithNewline(), tf.ReadAllLines(this._TestEncoding).JoinWithNewline());
            }
        }

        [Fact]
        public void BytesTest()
        {
            using (var tf = new TemporaryFile())
            {
                tf.WriteAllBytes(this._TestBytes);

                Assert.True(this.ByteArraysAreSame(this._TestBytes, tf.ReadAllBytes()));
                Assert.True(this.ByteArraysAreSame(File.ReadAllBytes(tf.FullPath), tf.ReadAllBytes()));
            }
        }

        [Fact]
        public void TextStaticTest()
        {
            using (var tf = TemporaryFile.WithContent(this._TestText))
            {
                Assert.Equal(this._TestText, tf.ReadAllText());
                Assert.Equal(File.ReadAllText(tf.FullPath), tf.ReadAllText());
            }
        }

        [Fact]
        public void LinesStaticTest()
        {
            using (var tf = TemporaryFile.WithContent(this._TestLines))
            {
                Assert.Equal(this._TestLines.JoinWithNewline(), tf.ReadAllLines().JoinWithNewline());
                Assert.Equal(File.ReadAllLines(tf.FullPath).JoinWithNewline(), tf.ReadAllLines().JoinWithNewline());
            }
        }

        [Fact]
        public void BytesStaticTest()
        {
            using (var tf = TemporaryFile.WithContent(this._TestBytes))
            {
                Assert.True(this.ByteArraysAreSame(this._TestBytes, tf.ReadAllBytes()));
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