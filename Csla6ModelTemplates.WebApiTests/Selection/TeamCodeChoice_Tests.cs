using Csla6ModelTemplates.Contracts.Selection.WithCode;
using Csla6ModelTemplates.Dal.Contracts;
using Csla6ModelTemplates.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Csla6ModelTemplates.WebApiTests.Selection
{
    public class TeamCodeChoice_Tests
    {
        [Fact]
        public async Task GetTeamChoiceWithCode_ReturnsAChoice()
        {
            // Arrange
            TestSetup setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<SelectionController>();
            var sut = new SelectionController(logger, setup.PortalFactory, setup.ChildPortalFactory);

            // Act
            TeamCodeChoiceCriteria criteria = new TeamCodeChoiceCriteria { TeamName = "9" };
            ActionResult<List<CodeNameOptionDto>> actionResult = await sut.GetTeamChoiceWithCode(criteria);

            // Assert
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            IList<CodeNameOptionDto> choice = okObjectResult.Value as IList<CodeNameOptionDto>;
            Assert.NotNull(choice);

            // The choice must have 5 items.
            Assert.Equal(5, choice.Count);

            // The codes and names must end with 9.
            foreach (var option in choice)
            {
                Assert.EndsWith("9", option.Code);
                Assert.EndsWith("9", option.Name);
            }
        }
    }
}
