using Csla6ModelTemplates.Contracts.Simple.Set;
using Csla6ModelTemplates.Models.Simple.Edit;
using Csla6ModelTemplates.Models.Simple.Set;
using Csla6ModelTemplates.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Csla6ModelTemplates.WebApiTests.Simple
{
    public class SimpleTeamSet_Tests
    {
        #region Read

        [Fact]
        public async Task ReadTeamSet_ReturnsCurrentModels()
        {
            // Arrange
            TestSetup setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<SimpleController>();
            var sut = new SimpleController(logger);

            // Act
            SimpleTeamSetCriteria criteria = new SimpleTeamSetCriteria { TeamName = "8" };
            ActionResult<List<SimpleTeamSetItemDto>> actionResult = await sut.GetTeamSet(
                criteria,
                setup.GetPortal<SimpleTeamSet>()
                );

            // Assert
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            List<SimpleTeamSetItemDto> pristineList = okObjectResult.Value as List<SimpleTeamSetItemDto>;
            Assert.NotNull(pristineList);

            // List must contain 5 items.
            Assert.True(pristineList.Count > 3);
        }

        #endregion

        #region Update

        [Fact]
        public async Task UpdateTeamSet_ReturnsUpdatedModels()
        {
            // Arrange
            TestSetup setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<SimpleController>();
            var sutR = new SimpleController(logger);
            var sutU = new SimpleController(logger);

            // Act
            SimpleTeamSetItemDto pristine = null;
            SimpleTeamSetItemDto pristineNew = null;
            string deletedId = null;
            ActionResult<List<SimpleTeamSetItemDto>> actionResult =
                await Call<List<SimpleTeamSetItemDto>>.RetryOnDeadlock(async () =>
                {
                    SimpleTeamSetCriteria criteria = new SimpleTeamSetCriteria { TeamName = "8" };
                    ActionResult<List<SimpleTeamSetItemDto>> actionResult = await sutR.GetTeamSet(
                        criteria,
                        setup.GetPortal<SimpleTeamSet>()
                        );
                    OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
                    List<SimpleTeamSetItemDto> pristineList = okObjectResult.Value as List<SimpleTeamSetItemDto>;

                    // Modify an item.
                    pristine = pristineList[0];
                    pristine.TeamCode = "T-9101";
                    pristine.TeamName = "Test team number 9101";

                    // Create new item.
                    pristineNew = new SimpleTeamSetItemDto
                    {
                        TeamId = null,
                        TeamCode = "T-9102",
                        TeamName = "Test team number 9102",
                        Timestamp = null
                    };
                    pristineList.Add(pristineNew);

                    // Delete an item.
                    SimpleTeamSetItemDto pristine3 = pristineList[3];
                    deletedId = pristine3.TeamId;
                    pristineList.Remove(pristine3);

                    return await sutU.UpdateTeamSet(
                        criteria,
                        pristineList,
                        setup.GetPortal<SimpleTeamSet>(),
                        setup.GetPortal<SimpleTeamSetItem>()
                        );
                });

            // Assert
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            List<SimpleTeamSetItemDto> updatedList = okObjectResult.Value as List<SimpleTeamSetItemDto>;
            Assert.NotNull(updatedList);

            // The updated team must have new values.
            SimpleTeamSetItemDto updated = updatedList[0];

            Assert.Equal(pristine.TeamId, updated.TeamId);
            Assert.Equal(pristine.TeamCode, updated.TeamCode);
            Assert.Equal(pristine.TeamName, updated.TeamName);
            Assert.NotEqual(pristine.Timestamp, updated.Timestamp);

            // The created team must have new values.
            SimpleTeamSetItemDto created = updatedList
                .FirstOrDefault(o => o.TeamCode == pristineNew.TeamCode);
            Assert.NotNull(created);

            Assert.NotNull(created.TeamId);
            Assert.Equal(pristineNew.TeamCode, created.TeamCode);
            Assert.Equal(pristineNew.TeamName, created.TeamName);
            Assert.NotNull(created.Timestamp);

            // The deleted team must have gone.
            SimpleTeamSetItemDto deleted = updatedList
                .FirstOrDefault(o => o.TeamId == deletedId);
            Assert.Null(deleted);
        }

        #endregion
    }
}
