using Catalog.Api.Api.Controllers;
using Catalog.Api.Dtos;
using Catalog.Api.Api.Entities;
using Catalog.Api.Api.Repositories.Contracts;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
namespace Catalog.UnitTests;

public class ItemsControllerTests
{

    private readonly Mock<IItemsRepository> _repositoryStub = new();
    private readonly Mock<ILogger<ItemsController>> _loggerStub = new();
    private readonly Random random = new();

    [Fact]
    public async Task GetItemAsync_WithUnexistingItem_ReturnsNotFound ()
    {
        _repositoryStub.Setup(repo => repo
        .GetItemAsync(It.IsAny<Guid>()))
        .ReturnsAsync((Item)null);
        var controller = new ItemsController(_repositoryStub.Object, _loggerStub.Object);
        var result = await controller.GetItemAsync(Guid.NewGuid());
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetItemAsync_WithExistingItem_ReturnsExpectedItem()
    {
        var expectedItem = CreateRandomItem();
        _repositoryStub.Setup(repo => repo
            .GetItemAsync(It.IsAny<Guid>()))
            .ReturnsAsync(expectedItem);
        var controller = new ItemsController(_repositoryStub.Object, _loggerStub.Object);
        var response = await controller.GetItemAsync(Guid.NewGuid());
        var result = ((OkObjectResult)response.Result).Value;
        result.Should().BeEquivalentTo(
            expectedItem,
            options => options.ComparingByMembers<Item>()
            );
    }

    
    [Fact]
    public async Task GetItemsAsync_WithExistingItems_ReturnAllItems()
    {
        var expectedItems = new[]{ CreateRandomItem(), CreateRandomItem(), CreateRandomItem() };
        _repositoryStub.Setup(repo => repo
        .GetItemsAsync())
        .ReturnsAsync(expectedItems);
        var controller = new ItemsController(_repositoryStub.Object, _loggerStub.Object);
        var actualItems = await controller.GetItemsAsync();
        actualItems.Should().BeEquivalentTo(
            expectedItems,
            options => options.ComparingByMembers<Item>()
        );
    }

    [Fact]
    public async Task CreateItemAsync_WithItemToCreate_ReturnsCreatedItem()
    {
        var itemToCreate = new CreateItemDTO(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), random.Next(1000));
        var controller = new ItemsController(_repositoryStub.Object, _loggerStub.Object);
        var result = await controller.CreateItemAsync(itemToCreate);
        var createdItem = (result.Result as CreatedAtActionResult).Value as ItemDTO;
        itemToCreate.Should().BeEquivalentTo(
            createdItem,
            options => options.ComparingByMembers<ItemDTO>().ExcludingMissingMembers()
        );
        createdItem.Id.Should().NotBeEmpty();
        createdItem.CreatedDate
        .Should()
        .BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public async Task UpdateItemAsync_WithExistingItem_ReturnsNoContent()
    {
        var existingItem = CreateRandomItem();
        _repositoryStub.Setup(repo => repo
            .GetItemAsync(It.IsAny<Guid>()))
            .ReturnsAsync(existingItem);

        var itemId = existingItem.Id;
        var itemToUpdate = new UpdateItemDTO(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), existingItem.Price + 3);
        var controller = new ItemsController(_repositoryStub.Object, _loggerStub.Object);
        var result = await controller.UpdateItemAsync(itemId, itemToUpdate);
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task DeleteItemAsync_WithExistingItem_ReturnsNoContent()
    {
        var existingItem = CreateRandomItem();
        _repositoryStub.Setup(repo => repo
            .GetItemAsync(It.IsAny<Guid>()))
            .ReturnsAsync(existingItem);

        var controller = new ItemsController(_repositoryStub.Object, _loggerStub.Object);
        var result = await controller.DeleteItemAsync(existingItem.Id);
        result.Should().BeOfType<NoContentResult>();
    }

    private Item CreateRandomItem()
    {
        return new() 
        {
            Id = Guid.NewGuid(),
            Name = Guid.NewGuid().ToString(),
            Price = random.Next(1000),
            CreatedDate = DateTimeOffset.UtcNow
        };
    }
}