using System;
using Xunit;
using System.Linq;
using Schlechtums.Core.Common;

namespace Schlechtums.Core.UnitTest
{
    public class RollingWindowTests
    {
        [Fact]
        public void RollingWindow_3Values_Size10_Test()
        {
            this.RunTest(10, new[] { 5, 7, 10 });
        }

        [Fact]
        public void RollingWindow_10Values_Size10_Test()
        {
            this.RunTest(10, new[] { 5, 7, 10, 2, 6, 3, 5, 1, 2, 3 });
        }

        [Fact]
        public void RollingWindow_12Values_Size10_Test()
        {
            this.RunTest(10, new[] { 5, 7, 10, 2, 6, 3, 5, 1, 2, 3, 4, 7 });
        }

        private void RunTest(int windowSize, int[] values)
        {
            var rw = new RollingWindow<int>(windowSize);

            rw.Add(values);

            Assert.Equal(values.Skip(Math.Max(0, values.Length - windowSize)).Average(), rw.Average());
        }
    }
}
