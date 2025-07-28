using Ambev.DeveloperEvaluation.Application.Sales.Items.CancelSaleItem;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.Items.CancelSaleItem;

public class CancelSaleItemCommandHandlerTests
{
    private readonly ISaleRepository _saleRepository = Substitute.For<ISaleRepository>();
    private readonly CancelSaleItemCommandHandler _handler;

    public CancelSaleItemCommandHandlerTests()
    {
        _handler = new CancelSaleItemCommandHandler(_saleRepository);
    }

    [Fact(DisplayName = "Given valid command and item exists, should cancel and return success")]
    public async Task Handle_ValidCommand_ItemExists_ReturnsSuccess()
    {
        // Arrange
        var command = new CancelSaleItemCommand(Guid.NewGuid(), Guid.NewGuid());

        var cancelledItem = new SaleItem() {  Id = command.ItemId };
        var sale = new Sale() { Id = command.SaleId, Items = new() { cancelledItem } };


        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>())
            .Returns(sale);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();

        await _saleRepository.Received(1).GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>());
        await _saleRepository.Received(1).UpdateAsync(sale, Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given invalid command should throw ValidationException")]
    public async Task Handle_InvalidCommand_ThrowsValidationException()
    {
        // Arrange
        var command = new CancelSaleItemCommand(Guid.Empty, Guid.Empty);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, default);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();

        await _saleRepository.DidNotReceive().GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
        await _saleRepository.DidNotReceive().UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given sale not found should throw KeyNotFoundException")]
    public async Task Handle_SaleNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        var command = new CancelSaleItemCommand(Guid.NewGuid(), Guid.NewGuid());

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>())
            .Returns((Sale)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, default);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Sale with ID {command.SaleId} not found");
    }

    [Fact(DisplayName = "Given sale found but item not found should throw KeyNotFoundException")]
    public async Task Handle_ItemNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        var command = new CancelSaleItemCommand(Guid.NewGuid(), Guid.NewGuid());

        var sale = new Sale() { Id = command.SaleId };

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>())
            .Returns(sale);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, default);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Sale Item with ID {command.ItemId} not found");

        _saleRepository.DidNotReceive().UpdateAsync(sale, Arg.Any<CancellationToken>());
    }
}