using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.DeleteSale;

public class DeleteSaleCommandHandlerTests
{
    private readonly ISaleRepository _saleRepository = Substitute.For<ISaleRepository>();

    private readonly DeleteSaleCommandHandler _handler;

    public DeleteSaleCommandHandlerTests()
    {
        _handler = new DeleteSaleCommandHandler(_saleRepository);
    }

    [Fact(DisplayName = "Given valid DeleteSaleCommand and existing ID should delete sale and return success")]
    public async Task Handle_ValidCommand_ExistingId_ReturnsSuccess()
    {
        // Arrange
        var command = new DeleteSaleCommand(Guid.NewGuid());

        _saleRepository.DeleteAsync(command.Id, Arg.Any<CancellationToken>()).Returns(true);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();

        await _saleRepository.Received(1).DeleteAsync(command.Id, Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given invalid DeleteSaleCommand should throw ValidationException")]
    public async Task Handle_InvalidCommand_ThrowsValidationException()
    {
        // Arrange
        var command = new DeleteSaleCommand(Guid.Empty);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, default);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();

        await _saleRepository.DidNotReceive().DeleteAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given valid DeleteSaleCommand but non-existing ID should throw KeyNotFoundException")]
    public async Task Handle_ValidCommand_NonExistingId_ThrowsKeyNotFoundException()
    {
        // Arrange
        var command = new DeleteSaleCommand(Guid.NewGuid());

        _saleRepository.DeleteAsync(command.Id, Arg.Any<CancellationToken>()).Returns(false);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, default);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Sale with ID {command.Id} not found");
    }
}
