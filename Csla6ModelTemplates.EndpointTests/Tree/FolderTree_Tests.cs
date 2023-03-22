using Csla6ModelTemplates.Contracts.Tree.View;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using M = Csla6ModelTemplates.Endpoints.Tree;

namespace Csla6ModelTemplates.EndpointTests.Tree
{
    public class FolderTree_Tests
    {
        [Fact]
        public async Task GetFolderTree_ReturnsATree()
        {
            // Arrange
            var setup = TestSetup.GetInstance();
            var logger = setup.GetLogger<M.Tree>();
            var sut = new M.Tree(logger, setup.Csla);

            // Act
            ActionResult<FolderNodeDto> actionResult = await sut.HandleAsync("7x95p9vYaZz");

            // Assert
            var okObjectResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var tree = okObjectResult.Value as List<FolderNodeDto>;
            Assert.NotNull(tree);

            // The tree must have one root node.
            Assert.Single(tree);

            // Level 1 - root node
            var nodeLevel1 = tree[0];
            Assert.Equal(1, nodeLevel1.Level);
            Assert.True(nodeLevel1.Children.Count > 0);

            // Level 2
            var nodeLevel2 = nodeLevel1.Children[0];
            Assert.Equal(2, nodeLevel2.Level);
            Assert.True(nodeLevel2.Children.Count > 0);

            // Level 3
            var nodeLevel3 = nodeLevel2.Children[0];
            Assert.Equal(3, nodeLevel3.Level);
            Assert.True(nodeLevel3.Children.Count > 0);

            // Level 4
            var nodeLevel4 = nodeLevel3.Children[0];
            Assert.Equal(4, nodeLevel4.Level);
            Assert.Empty(nodeLevel4.Children);
        }
    }
}
