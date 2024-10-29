using Marketplace.Common.Outbox.Exceptions;
using Marketplace.Common.Outbox.Queue;

namespace Marketplace.Common.UnitTests.Outbox;

public class OutboxQueueTests : IDisposable
{
    private readonly string dbName;

    public OutboxQueueTests()
    {
        var guid = Guid.NewGuid()
            .ToString()
            .Replace("-", "");

        dbName = $"{guid}.db";
    }

    [Fact]
    public async Task Item_pushed()
    {
        // Arrange
        string item = "some text";
        OutboxQueue<string> sut = new(dbName, false);

        // Act
        await sut.PushAsync(item);

        // Assert
        var outboxItem = await sut.PeekAsync();
        Assert.Equal(item, outboxItem);

        var count = await sut.CountAsync();
        Assert.Equal(1, count);
    }

    [Fact]
    public async Task Items_pushed()
    {
        // Arrange
        string[] items =
        [
            "some text 1",
            "some text 2",
            "some text 3"
        ];
        OutboxQueue<string> sut = new(dbName, false);

        // Act
        await sut.PushAsync(items);

        // Assert
        var outboxItems = await sut.PeekAsync(items.Length);
        Assert.Equal(items.Length, outboxItems.Count);
        Assert.Equal(items, outboxItems);

        var count = await sut.CountAsync();
        Assert.Equal(items.Length, count);
    }

    [Fact]
    public async Task Item_peeked()
    {
        // Arrange
        string item = "some text";

        OutboxQueue<string> sut = new(dbName, false);
        await sut.PushAsync(item);

        // Act
        var peekedItem = await sut.PeekAsync();

        // Assert
        var outboxItem = await sut.PeekAsync();
        Assert.Equal(item, peekedItem);
        Assert.Equal(peekedItem, outboxItem);

        var count = await sut.CountAsync();
        Assert.Equal(1, count);
    }

    [Fact]
    public async Task Items_peeked()
    {
        // Arrange
        string[] items =
        [
            "some text 1",
            "some text 2",
            "some text 3"
        ];
        OutboxQueue<string> sut = new(dbName, false);
        await sut.PushAsync(items);

        // Act
        var peekedItems = await sut.PeekAsync(items.Length);

        // Assert
        var outboxItems = await sut.PeekAsync(items.Length);
        Assert.Equal(items, peekedItems);
        Assert.Equal(peekedItems, outboxItems);

        var count = await sut.CountAsync();
        Assert.Equal(items.Length, count);
    }

    [Fact]
    public async Task Item_poped()
    {
        // Arrange
        string item = "some text";

        OutboxQueue<string> sut = new(dbName, false);
        await sut.PushAsync(item);

        // Act
        var popedItem = await sut.PopAsync();

        // Assert
        await Assert.ThrowsAsync<OutboxEmptyException>(async () => await sut.PopAsync());
        Assert.Equal(item, popedItem);

        var count = await sut.CountAsync();
        Assert.Equal(0, count);
    }

    [Fact]
    public async Task Items_poped()
    {
        // Arrange
        string[] items =
        [
            "some text 1",
            "some text 2",
            "some text 3"
        ];
        OutboxQueue<string> sut = new(dbName, false);
        await sut.PushAsync(items);

        // Act
        var popedItems = await sut.PopAsync(items.Length);

        // Assert
        await Assert.ThrowsAsync<OutboxEmptyException>(async () => await sut.PopAsync());
        Assert.Equal(items, popedItems);

        var count = await sut.CountAsync();
        Assert.Equal(0, count);
    }

    [Fact]
    public async Task Items_counted()
    {
        // Arrange
        string[] items =
        [
            "some text 1",
            "some text 2",
            "some text 3"
        ];
        OutboxQueue<string> sut = new(dbName, false);
        await sut.PushAsync(items);

        // Act
        var count = await sut.CountAsync();

        // Assert
        Assert.Equal(items.Length, count);
    }

    public void Dispose()
    {
        File.Delete(dbName);
    }
}
