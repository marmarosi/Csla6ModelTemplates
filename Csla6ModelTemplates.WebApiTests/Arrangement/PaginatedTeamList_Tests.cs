using Csla6ModelTemplates.Contracts.Arrangement.Pagination;
using Csla6ModelTemplates.Dal.Contracts;
using Csla6ModelTemplates.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Xunit;

namespace Csla6ModelTemplates.WebApiTests.Arrangement
{
    public class PaginatedTeamList_Tests
    {
        [Fact]
        public async Task GetPaginatedTeamList_ReturnsAList()
        {
            // Arrange
            TestSetup setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<ArrangementController>();
            var sut = new ArrangementController(logger, setup.Csla);

            // Act
            PaginatedTeamListCriteria criteria = new PaginatedTeamListCriteria
            {
                TeamName = "1",
                PageIndex = 1,
                PageSize = 10
            };
            ActionResult<PaginatedList<PaginatedTeamListItemDto>> actionResult = await sut.GetPaginatedTeamList(criteria);

            // Assert
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            IPaginatedList<PaginatedTeamListItemDto> list = okObjectResult.Value as IPaginatedList<PaginatedTeamListItemDto>;
            Assert.NotNull(list);

            // The list must have 4 items and 14 total items.
            Assert.Equal(4, list.Data.Count);
            Assert.Equal(14, list.TotalCount);

            // The code and names must contain 1.
            foreach (var item in list.Data)
            {
                Assert.Contains("1", item.TeamCode);
                Assert.Contains("1", item.TeamName);
            }
        }
    }
}
