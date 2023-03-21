using Csla6ModelTemplates.Contracts.Arrangement.Full;
using Csla6ModelTemplates.Dal.Contracts;
using Csla6ModelTemplates.Endpoints.Arrangement;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Xunit;

namespace Csla6ModelTemplates.EndpointTests.Arrangement
{
    public class ArrangedTeamList_Tests
    {
        [Fact]
        public async Task ArrangedTeamList_ReturnsAList()
        {
            // Arrange
            var setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<Full>();
            var sut = new Full(logger, setup.PortalFactory);

            // Act
            ActionResult<IPaginatedList<ArrangedTeamListItemDto>> actionResult = await sut.HandleAsync(
                new ArrangedTeamListCriteria
                {
                    TeamName = "1",
                    PageIndex = 1,
                    PageSize = 10,
                    SortBy = ArrangedTeamListSortBy.TeamCode,
                    SortDirection = SortDirection.Descending
                });

            // Assert
            var okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var list = okObjectResult.Value as PaginatedList<ArrangedTeamListItemDto>;
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
