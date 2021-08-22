using Schlechtums.Core.Common.Extensions;
using System.IO;
using Xunit;

namespace Schlechtums.Core.UnitTest
{
    public class ToVSProjectRootPathTests
    {
        [Fact]
        public void ToVSProjectRoot_File_Test()
        {
            var f = new FileInfo("ToVSProjectRootPathTests.cs");
            Assert.True(File.Exists(f.ToVSProjectRootPath()));
        }

        [Fact]
        public void ToVSProjectRoot_FileInDirectory_Test()
        {
            var f = new FileInfo(Path.Combine("TestTypes", "DALAttributes.cs"));
            Assert.True(File.Exists(f.ToVSProjectRootPath()));
        }

        [Fact]
        public void ToVSProjectRoot_Directory_Test()
        {
            var d = new DirectoryInfo("TestTypes");
            Assert.True(Directory.Exists(d.ToVSProjectRootPath()));
        }
    }
}