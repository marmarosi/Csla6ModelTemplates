using Csla6ModelTemplates.Contracts.Junction.View;
using Csla6ModelTemplates.Endpoints.Junction;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Xunit;

namespace Csla6ModelTemplates.EndpointTests.Junction
{
    public class GroupView_Tests
    {
        [Fact]
        public async Task GetGroupView_ReturnsAView()
        {
            // Arrange
            var setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<View>();
            var sut = new View(logger, setup.Csla);

            // Act
            ActionResult<GroupViewDto> actionResult = await sut.HandleAsync("oQLOyK85x6g");

            // Assert
            var okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var group = okObjectResult.Value as GroupViewDto;
            Assert.NotNull(group);

            // The code and name must end with 17.
            Assert.Equal("G-08", group.GroupCode);
            Assert.EndsWith("8", group.GroupName);
            Assert.True(group.Persons.Count > 0);

            // The code and name must end with 17.
            var groupPerson = group.Persons[0];
            Assert.StartsWith("Person #", groupPerson.PersonName);
        }
    }
}
