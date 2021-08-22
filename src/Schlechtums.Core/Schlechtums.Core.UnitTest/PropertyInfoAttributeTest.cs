using Schlechtums.Core.Common.Extensions;
using Schlechtums.Core.UnitTest.TestTypes;
using System.Linq;
using Xunit;

namespace Schlechtums.Core.UnitTest
{
    public class PropertyInfoAttributeTest
    {
        [Fact]
        public void GetAttributeTest()
        {
            var ignoredTypeProperties = typeof(Ingredient).GetProperties()
                                                          .Where(p => p.GetAttribute<DALIgnoreAttribute>() != null)
                                                          .Select(p => p.Name)
                                                          .ToHashSet();

            Assert.Single(ignoredTypeProperties);
            Assert.Equal(nameof(Ingredient.IgnoreThis), ignoredTypeProperties.Single());
            

            var propertyNamesToDALName = typeof(Ingredient).GetProperties()
                                                           .Where(p => p.GetAttribute<DALIgnoreAttribute>() == null)
                                                           .ToDictionary(p => p.Name, p => p.GetAttribute<DALSQLParameterNameAttribute>()?.Name ?? p.Name);

            Assert.Equal(31, propertyNamesToDALName.Count);
            Assert.Equal("Id", propertyNamesToDALName[nameof(Ingredient.Id)]);
            Assert.Equal("FiberPerEach", propertyNamesToDALName[nameof(Ingredient.FiberPerEach)]);
            Assert.Equal("MonoUnsatFatPerGram", propertyNamesToDALName[nameof(Ingredient.MonoUnsaturatedFatPerGram)]);
            Assert.Equal("PolyUnsatFatPerGram", propertyNamesToDALName[nameof(Ingredient.PolyunsaturatedFatPerGram)]);
        }
    }
}