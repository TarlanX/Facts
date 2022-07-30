using System;
using Teagle.Facts.Web.Infrastructure.Mappers.Base;
using Xunit;

namespace Teagle.Facts.Web.Tests
{
    public class AutomapperTests
    {
        [Fact]
        [Trait("Automapper", "Mapper Configuration")]
        public void IsShouldCorreclyConfigured()
        {

            // arrange 
            var config = MapperRegistration.GetMapperConfiguration();


            // act

            // assert 

            config.AssertConfigurationIsValid();
        }
    }
}
