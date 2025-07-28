using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.UpdateSale;

public class UpdateSaleCommandHandlerTests
{
    private readonly ISaleRepository _saleRepository = Substitute.For<ISaleRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();

    private readonly UpdateSaleCommandHandler _handler;

    public UpdateSaleCommandHandlerTests()
    {
        _handler = new UpdateSaleCommandHandler(_saleRepository, _mapper);
    }

    [Fact(DisplayName = "Given valid UpdateSaleCommand and existing ID should update and return result")]
    public async Task Handle_ValidCommand_ExistingId_ReturnsUpdatedResult()
    {
        // Arrange
        var command = new UpdateSaleCommand
        {
            Id = Guid.NewGuid(),
            CustomerId = Guid.NewGuid(),
            CustomerName = "John Doe",
            BranchId = Guid.NewGuid(),
            BranchName = "Branch A",
            Number = 123,
            Date = DateTime.UtcNow
        };

        var dbSale = Substitute.For<Sale>();
        dbSale.Id = command.Id;

        var updatedSale = dbSale;

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(dbSale);
        _saleRepository.UpdateAsync(dbSale, Arg.Any<CancellationToken>()).Returns(updatedSale);

        var expectedResult = new UpdateSaleResult { Id = updatedSale.Id };
        _mapper.Map<UpdateSaleResult>(updatedSale).Returns(expectedResult);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(command.Id);

        await _saleRepository.Received(1).GetByIdAsync(command.Id, Arg.Any<CancellationToken>());
        await _saleRepository.Received(1).UpdateAsync(dbSale, Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<UpdateSaleResult>(updatedSale);

        dbSale.Received(1).Update(
            command.CustomerId,
            command.CustomerName,
            command.BranchId,
            command.BranchName,
            command.Number,
            command.Date);
    }

    [Fact(DisplayName = "Given invalid UpdateSaleCommand should throw ValidationException")]
    public async Task Handle_InvalidCommand_ThrowsValidationException()
    {
        // Arrange
        var command = new UpdateSaleCommand();

        // Act
        Func<Task> act = async () => await _handler.Handle(command, default);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();

        await _saleRepository.DidNotReceive().GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
        _saleRepository.DidNotReceive().UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given valid UpdateSaleCommand but non-existing ID should throw KeyNotFoundException")]
    public async Task Handle_ValidCommand_NonExistingId_ThrowsKeyNotFoundException()
    {
        // Arrange
        var command = new UpdateSaleCommand
        {
            Id = Guid.NewGuid(),
            CustomerId = Guid.NewGuid(),
            CustomerName = "John Doe",
            BranchId = Guid.NewGuid(),
            BranchName = "Branch A",
            Number = 123,
            Date = DateTime.UtcNow
        };

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns((Sale)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, default);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Sale with ID {command.Id} not found");

        _saleRepository.DidNotReceive().UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
        _mapper.DidNotReceive().Map<UpdateSaleResult>(Arg.Any<Sale>());
    }
}