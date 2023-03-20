using Csla6ModelTemplates.Contracts.Junction.Edit;
using Csla6ModelTemplates.Endpoints.Junction;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Csla6ModelTemplates.EndpointTests.Junction
{
    public class Group_Tests
    {
        #region New

        [Fact]
        public async Task NewGroup_ReturnsNewModel()
        {
            // Arrange
            var setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<New>();
            var sut = new New(logger, setup.PortalFactory);

            // Act
            ActionResult<GroupDto> actionResult = await sut.HandleAsync();

            // Assert
            var okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var group = okObjectResult.Value as GroupDto;
            Assert.NotNull(group);

            // The code and name must miss.
            Assert.Empty(group.GroupCode);
            Assert.Empty(group.GroupName);
            Assert.Null(group.Timestamp);
            Assert.Empty(group.Persons);
        }

        #endregion

        #region Create

        [Fact]
        public async Task CreateGroup_ReturnsCreatedModel()
        {
            // Arrange
            var setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<Create>();
            var sut = new Create(logger, setup.PortalFactory, setup.ChildPortalFactory);

            // Act
            var pristineGroup = new GroupDto
            {
                GroupId = null,
                GroupCode = "T-9201",
                GroupName = "Test group number 9201",
                Timestamp = null
            };
            var pristineMember1 = new GroupPersonDto
            {
                PersonId = "7adBbqg8nxV",
                PersonName = "Person #11"
            };
            pristineGroup.Persons.Add(pristineMember1);
            var pristineMember2 = new GroupPersonDto
            {
                PersonId = "4aoj5R40G1e",
                PersonName = "Person #17"
            };
            pristineGroup.Persons.Add(pristineMember2);

            var actionResult = await sut.HandleAsync(pristineGroup, new CancellationToken());

            // Assert
            var createdResult = actionResult.Result as CreatedResult;
            Assert.NotNull(createdResult);

            var createdGroup = createdResult.Value as GroupDto;
            Assert.NotNull(createdGroup);

            // The group must have new values.
            Assert.NotNull(createdGroup.GroupId);
            Assert.Equal(pristineGroup.GroupCode, createdGroup.GroupCode);
            Assert.Equal(pristineGroup.GroupName, createdGroup.GroupName);
            Assert.NotNull(createdGroup.Timestamp);

            // The persons must have new values.
            Assert.Equal(2, createdGroup.Persons.Count);

            var createdMember1 = createdGroup.Persons[0];
            Assert.Equal(pristineMember1.PersonId, createdMember1.PersonId);
            Assert.Equal(pristineMember1.PersonName, createdMember1.PersonName);

            var createdMember2 = createdGroup.Persons[1];
            Assert.Equal(pristineMember2.PersonId, createdMember2.PersonId);
            Assert.Equal(pristineMember2.PersonName, createdMember2.PersonName);
        }

        #endregion

        #region Read

        [Fact]
        public async Task ReadGroup_ReturnsCurrentModel()
        {
            // Arrange
            var setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<Read>();
            var sut = new Read(logger, setup.PortalFactory);

            // Act
            ActionResult<GroupDto> actionResult = await sut.HandleAsync("6KANyA658o9");

            // Assert
            var okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var pristineGroup = okObjectResult.Value as GroupDto;
            Assert.NotNull(pristineGroup);

            // The group code and name must end with 10.
            Assert.Equal("6KANyA658o9", pristineGroup.GroupId);
            Assert.Equal("G-10", pristineGroup.GroupCode);
            Assert.EndsWith("10", pristineGroup.GroupName);
            Assert.NotNull(pristineGroup.Timestamp);

            // The person name must start with Person.
            Assert.True(pristineGroup.Persons.Count > 0);
            foreach (var groupPerson in pristineGroup.Persons)
            {
                Assert.StartsWith("Person", groupPerson.PersonName);
            }
        }

        #endregion

        #region Update

        [Fact]
        public async Task UpdateGroup_ReturnsUpdatedModel()
        {
            // Arrange
            var setup = TestSetup.GetInstance();
            var loggerR = setup.GetLogger<Read>();
            var loggerU = setup.GetLogger<Update>();
            var sutR = new Read(loggerR, setup.PortalFactory);
            var sutU = new Update(loggerU, setup.PortalFactory, setup.ChildPortalFactory);

            // Act
            GroupDto pristineGroup = null;
            GroupPersonDto pristineMember1 = null;
            GroupPersonDto pristineMemberNew = null;

            ActionResult<GroupDto> actionResultR = await sutR.HandleAsync("aqL3y3P5dGm");
            var okObjectResultR = actionResultR.Result as OkObjectResult;
            pristineGroup = okObjectResultR.Value as GroupDto;
            pristineMember1 = pristineGroup.Persons[0];

            pristineGroup.GroupCode = "G-1212";
            pristineGroup.GroupName = "Group No. 1212";

            pristineMemberNew = new GroupPersonDto
            {
                PersonId = "a4P18mr5M62",
                PersonName = "New member",
            };
            pristineGroup.Persons.Add(pristineMemberNew);

            var actionResultU = await sutU.HandleAsync(pristineGroup, new CancellationToken());

            // Assert
            var okObjectResultU = actionResultU.Result as OkObjectResult;
            Assert.NotNull(okObjectResultU);

            var updatedGroup = okObjectResultU.Value as GroupDto;
            Assert.NotNull(updatedGroup);

            // The group must have new values.
            Assert.Equal(pristineGroup.GroupId, updatedGroup.GroupId);
            Assert.Equal(pristineGroup.GroupCode, updatedGroup.GroupCode);
            Assert.Equal(pristineGroup.GroupName, updatedGroup.GroupName);
            Assert.NotEqual(pristineGroup.Timestamp, updatedGroup.Timestamp);

            Assert.Equal(pristineGroup.Persons.Count, updatedGroup.Persons.Count);

            // Persons must reflect the changes.
            var updatedMember1 = updatedGroup.Persons[0];
            Assert.Equal(pristineMember1.PersonId, updatedMember1.PersonId);
            Assert.Equal(pristineMember1.PersonName, updatedMember1.PersonName);

            var createdMemberNew = updatedGroup.Persons[pristineGroup.Persons.Count - 1];
            Assert.Equal(pristineMemberNew.PersonId, createdMemberNew.PersonId);
            Assert.StartsWith("New member", createdMemberNew.PersonName);
        }

        #endregion

        #region Delete

        [Fact]
        public async Task DeleteGroup_ReturnsNothing()
        {
            // Arrange
            var setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<Delete>();
            var sut = new Delete(logger, setup.PortalFactory);

            // Act
            ActionResult actionResult = await sut.HandleAsync("3Nr8nQenQjA");

            // Assert
            var noContentResult = actionResult as NoContentResult;
            Assert.NotNull(noContentResult);
            Assert.Equal(204, noContentResult.StatusCode);
        }

        #endregion
    }
}
