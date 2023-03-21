using Csla6ModelTemplates.Dal.Contracts;
using Csla6ModelTemplates.Endpoints.Tree;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Csla6ModelTemplates.EndpointTests.Tree
{
    public class RootFolderChoice_Tests
    {
        [Fact]
        public async Task GetTeamChoiceWithId_ReturnsAChoice()
        {
            // Arrange
            var setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<Choice>();
            var sut = new Choice(logger, setup.PortalFactory);

            // Act
            ActionResult<IList<IdNameOptionDto>> actionResult = await sut.HandleAsync();

            // Assert
            var okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var choice = okObjectResult.Value as List<IdNameOptionDto>;
            Assert.NotNull(choice);

            // The choice must have 3 items.
            Assert.Equal(3, choice.Count);

            // The names must start with 'Folder entry'.
            foreach (var item in choice)
            {
                Assert.StartsWith("Folder entry", item.Name);
            }
        }
    }
}
