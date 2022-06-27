using Catalog.Api.Api.Controllers;
using Catalog.Api.Api.Entities;
using Catalog.Api.Api.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
namespace Catalog.UnitTests;

public class ItemsControllerTests
{
    [Fact]
    public async Task GetItemAsync_WithUnexistingItem_ReturnsNotFound ()
    {
        var repositorySub = new Mock<IItemsRepository>();
        repositorySub.Setup(repo => repo
        .GetItemAsync(It.IsAny<Guid>()))
        .ReturnsAsync((Item)null);
        var loggerStub = new Mock<ILogger<ItemsController>>();
        var controller = new ItemsController(repositorySub.Object, loggerStub.Object);

        var result = await controller.GetItemAsync(Guid.NewGuid());

        Assert.IsType<NotFoundResult>(result.Result);

    }
}