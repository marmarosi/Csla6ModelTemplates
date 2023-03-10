﻿using Csla6ModelTemplates.Contracts.Simple.Command;
using Csla6ModelTemplates.Models.Simple.Command;
using Csla6ModelTemplates.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Transactions;
using Xunit;

namespace Csla6ModelTemplates.WebApiTests.Simple
{
    public class RenameTeam_Tests
    {
        [Fact]
        public async Task RenameTeam_ReturnsTrue()
        {
            // Arrange
            TestSetup setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<SimpleController>();
            var sut = new SimpleController(logger);

            // Act
            ActionResult<bool> actionResult;
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                actionResult = await sut.RenameTeamCommand(
                    new RenameTeamDto
                    {
                        TeamId = "oZkzGJ6G794",
                        TeamName = "Team Thirty Seven"
                    },
                    setup.GetPortal<RenameTeam>()
                    );
            }

            // Assert
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            bool success = (bool)okObjectResult.Value;
            Assert.True(success);
        }
    }
}