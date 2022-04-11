using FluentAssertions;
using Schlechtums.Core.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;

namespace Schlechtums.Core.UnitTest
{
    public class EnumerationJoinsTest
    {
        [Fact]
        public void EnumerationJoinsTest_ShouldJoinCorrectly()
        {
            var left = new List<JoinTestClass>
            {
                new JoinTestClass("p1", 37, "baking"),
                new JoinTestClass("p2", 34, "reading"),
                new JoinTestClass("p3", 30, "music"),
            };

            var right = new List<JoinTestClass>
            {
                new JoinTestClass("p1", 37, "baking"),
                new JoinTestClass("p2", 34, "writing"),
                new JoinTestClass("p4", 27, "cars"),
            };

            var joins = left.GetAddedRemovedModifiedJoins(right, p => p.Name);

            joins.AddedItems.Should().HaveCount(1);
            joins.RemovedItems.Should().HaveCount(1);
            joins.ModifiedItems.Should().HaveCount(1);
            joins.IdenticalItems.Should().HaveCount(1);

            joins.AddedItems.Single().Should().BeEquivalentTo(new JoinTestClass("p4", 27, "cars"));
            joins.RemovedItems.Single().Should().BeEquivalentTo(new JoinTestClass("p3", 30, "music"));
            joins.ModifiedItems.Single().Left.Should().BeEquivalentTo(new JoinTestClass("p2", 34, "reading"));
            joins.ModifiedItems.Single().Right.Should().BeEquivalentTo(new JoinTestClass("p2", 34, "writing"));
            joins.IdenticalItems.Single().Should().BeEquivalentTo(new JoinTestClass("p1", 37, "baking"));
        }

        public class JoinTestClass : IEquatable<JoinTestClass>
        {
            public JoinTestClass(string name, int age, string favoriteHobby)
            {
                this.Name = name;
                this.Age = age;
                this.FavoriteHobby = favoriteHobby;
            }

            public string Name { get; set; }
            public int Age { get; set; }
            public string FavoriteHobby { get; set; }

            public bool Equals([AllowNull] JoinTestClass other)
            {
                if (other != null && this.Name == other.Name && this.Age == other.Age)
                {
                    return this.FavoriteHobby == other.FavoriteHobby;
                }

                return false;
            }
        }
    }
}
