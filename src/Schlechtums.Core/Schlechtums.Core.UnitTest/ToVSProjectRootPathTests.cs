using Schlechtums.Core.Common.Extensions;
using System.IO;
using NUnit.Framework;
using FluentAssertions;

namespace Schlechtums.Core.UnitTest
{
    public class ToVSProjectRootPathTests
    {
        [Test]
        public void ToVSProjectRoot_File_Test()
        {
            var f = new FileInfo("ToVSProjectRootPathTests.cs");
            File.Exists(f.ToVSProjectRootPath()).Should().BeTrue();
        }

        [Test]
        public void ToVSProjectRoot_FileInDirectory_Test()
        {
            var f = new FileInfo(Path.Combine("TestTypes", "DALAttributes.cs"));
            File.Exists(f.ToVSProjectRootPath()).Should().BeTrue();
        }

        [Test]
        public void ToVSProjectRoot_Directory_Test()
        {
            var dirName = "TestTypes";
            var d = new DirectoryInfo(dirName);
            Path.GetFileName(d.ToVSProjectRootPath()).Should().Be(dirName);
        }
    }
}