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
            // ********** Arrange
            var setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<Arranged>();
            var sut = new Arranged(logger, setup.Csla);

            // ********** Act
            var actionResult = await sut.HandleAsync(
                new ArrangedTeamListCriteria
                {
                    TeamName = "1",
                    PageIndex = 1,
                    PageSize = 10,
                    SortBy = ArrangedTeamListSortBy.TeamCode,
                    SortDirection = SortDirection.Descending
                });

            // ********** Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult);
            var list = Assert.IsAssignableFrom<IPaginatedList<ArrangedTeamListItemDto>>(okObjectResult.Value);

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
