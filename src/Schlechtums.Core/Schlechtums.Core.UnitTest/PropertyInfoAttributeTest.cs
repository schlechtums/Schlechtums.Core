using Schlechtums.Core.Common.Extensions;
using Schlechtums.Core.UnitTest.TestTypes;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;

namespace Schlechtums.Core.UnitTest
{
    public class PropertyInfoAttributeTest
    {
        [Test]
        public void GetAttributeTest()
        {
            var ignoredTypeProperties = typeof(Ingredient).GetProperties()
                                                          .Where(p => p.GetAttribute<DALIgnoreAttribute>() != null)
                                                          .Select(p => p.Name)
                                                          .ToHashSet();

            ignoredTypeProperties.Should().HaveCount(1);
            ignoredTypeProperties.Single().Should().Be(nameof(Ingredient.IgnoreThis));
            

            var propertyNamesToDALName = typeof(Ingredient).GetProperties()
                                                           .Where(p => p.GetAttribute<DALIgnoreAttribute>() == null)
                                                           .ToDictionary(p => p.Name, p => p.GetAttribute<DALSQLParameterNameAttribute>()?.Name ?? p.Name);

            propertyNamesToDALName.Count.Should().Be(31);
            propertyNamesToDALName[nameof(Ingredient.Id)].Should().Be("Id");
            propertyNamesToDALName[nameof(Ingredient.FiberPerEach)].Should().Be("FiberPerEach");
            propertyNamesToDALName[nameof(Ingredient.MonoUnsaturatedFatPerGram)].Should().Be("MonoUnsatFatPerGram");
            propertyNamesToDALName[nameof(Ingredient.PolyunsaturatedFatPerGram)].Should().Be("PolyUnsatFatPerGram");
        }
    }
}