using Ambev.DeveloperEvaluation.Application.Sales.Items.AddSaleItem;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.Items.AddSaleItem;

public class AddSaleItemCommandHandlerTests
{
    private readonly ISaleRepository _saleRepository = Substitute.For<ISaleRepository>();
    private readonly ISaleService _saleService = Substitute.For<ISaleService>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();

    private readonly AddSaleItemCommandHandler _handler;

    public AddSaleItemCommandHandlerTests()
    {
        _handler = new AddSaleItemCommandHandler(_saleRepository, _saleService, _mapper);
    }

    [Fact(DisplayName = "Given valid AddSaleItemCommand and existing sale, should add item and return result")]
    public async Task Handle_ValidCommand_ExistingSale_ReturnsItemResult()
    {
        // Arrange
        var command = new AddSaleItemCommand
        {
            SaleId = Guid.NewGuid(),
            ProductId = Guid.NewGuid(),
            ProductName = "Beer",
            Quantity = 10,
            UnitPrice = 5.5m
        };

        var sale = Substitute.For<Sale>();
        var saleItem = new SaleItem();
        var expectedResult = new AddSaleItemResult { ProductId = command.ProductId };

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>()).Returns(sale);
        _mapper.Map<SaleItem>(command).Returns(saleItem);
        _mapper.Map<AddSaleItemResult>(saleItem).Returns(expectedResult);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.Should().NotBeNull();
        result.ProductId.Should().Be(command.ProductId);

        await _saleRepository.Received(1).GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>());
        sale.Received(1).AddItem(saleItem);
        _saleService.Received(1).CalculateAndApplyItemDiscounts(sale);
        await _saleRepository.Received(1).UpdateAsync(sale, Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given invalid AddSaleItemCommand should throw ValidationException")]
    public async Task Handle_InvalidCommand_ThrowsValidationException()
    {
        // Arrange
        var command = new AddSaleItemCommand(); // campos vazios/invalidos

        // Act
        Func<Task> act = async () => await _handler.Handle(command, default);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();

        await _saleRepository.DidNotReceive().GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
        _saleRepository.DidNotReceive().UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given valid command but sale not found should throw KeyNotFoundException")]
    public async Task Handle_ValidCommand_SaleNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        var command = new AddSaleItemCommand
        {
            SaleId = Guid.NewGuid(),
            ProductId = Guid.NewGuid(),
            ProductName = "Beer",
            Quantity = 2,
            UnitPrice = 4.5m
        };

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>())
            .Returns((Sale)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, default);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Sale with ID {command.SaleId} not found");

        _saleRepository.DidNotReceive().UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
        _mapper.DidNotReceive().Map<AddSaleItemResult>(Arg.Any<SaleItem>());
    }
}