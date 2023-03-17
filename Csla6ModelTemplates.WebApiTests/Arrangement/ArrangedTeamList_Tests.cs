using Csla6ModelTemplates.Contracts.Arrangement.Full;
using Csla6ModelTemplates.Dal.Contracts;
using Csla6ModelTemplates.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Xunit;

namespace Csla6ModelTemplates.WebApiTests.Arrangement
{
    public class ArrangedTeamList_Tests
    {
        [Fact]
        public async Task GetArrangedTeamList_ReturnsAList()
        {
            // Arrange
            TestSetup setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<ArrangementController>();
            var sut = new ArrangementController(logger, setup.PortalFactory, setup.ChildPortalFactory);

            // Act
            ArrangedTeamListCriteria criteria = new ArrangedTeamListCriteria
            {
                TeamName = "1",
                PageIndex = 1,
                PageSize = 10,
                SortBy = ArrangedTeamListSortBy.TeamCode,
                SortDirection = SortDirection.Descending
            };
            ActionResult<PaginatedList<ArrangedTeamListItemDto>> actionResult = await sut.GetArrangedTeamList(criteria);

            // Assert
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            IPaginatedList<ArrangedTeamListItemDto> list = okObjectResult.Value as IPaginatedList<ArrangedTeamListItemDto>;
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
