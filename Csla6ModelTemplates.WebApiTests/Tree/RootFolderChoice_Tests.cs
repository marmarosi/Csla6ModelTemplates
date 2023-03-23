using Csla6ModelTemplates.Dal.Contracts;
using Csla6ModelTemplates.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Csla6ModelTemplates.WebApiTests.Tree
{
    public class RootFolderChoice_Tests
    {
        [Fact]
        public async Task GetTeamChoiceWithId_ReturnsAChoice()
        {
            // Arrange
            var setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<TreeController>();
            var sut = new TreeController(logger, setup.Csla);

            // Act
            var actionResult = await sut.GetRootFolderChoice();

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult);
            var choice = Assert.IsAssignableFrom<IList<IdNameOptionDto>>(okObjectResult.Value);

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
