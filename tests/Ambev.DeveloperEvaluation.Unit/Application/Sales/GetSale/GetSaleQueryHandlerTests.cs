using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.GetSale;

public class GetSaleQueryHandlerTests
{
    private readonly ISaleRepository _saleRepository = Substitute.For<ISaleRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();

    private readonly GetSaleQueryHandler _handler;

    public GetSaleQueryHandlerTests()
    {
        _handler = new GetSaleQueryHandler(_saleRepository, _mapper);
    }

    [Fact(DisplayName = "Given valid GetSaleQuery and existing ID should return sale result")]
    public async Task Handle_ValidQuery_ExistingId_ReturnsSaleResult()
    {
        // Arrange
        var query = new GetSaleQuery(Guid.NewGuid());

        var sale = new Sale
        {
            Id = query.Id,
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            Date = DateTime.UtcNow
        };

        var expectedResult = new GetSaleResult
        {
            Id = sale.Id,
            CustomerId = sale.CustomerId,
            BranchId = sale.BranchId,
            Date = sale.Date
        };

        _saleRepository.GetByIdAsNoTrackingAsync(query.Id, Arg.Any<CancellationToken>()).Returns(sale);
        _mapper.Map<GetSaleResult>(sale).Returns(expectedResult);

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(expectedResult.Id);
        result.CustomerId.Should().Be(expectedResult.CustomerId);

        await _saleRepository.Received(1).GetByIdAsNoTrackingAsync(query.Id, Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<GetSaleResult>(sale);
    }

    [Fact(DisplayName = "Given invalid GetSaleQuery should throw ValidationException")]
    public async Task Handle_InvalidQuery_ThrowsValidationException()
    {
        // Arrange
        var query = new GetSaleQuery(Guid.Empty);

        // Act
        Func<Task> act = async () => await _handler.Handle(query, default);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();

        await _saleRepository.DidNotReceive().GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
        _mapper.DidNotReceive().Map<GetSaleResult>(Arg.Any<Sale>());
    }

    [Fact(DisplayName = "Given valid GetSaleQuery but non-existing ID should throw KeyNotFoundException")]
    public async Task Handle_ValidQuery_NonExistingId_ThrowsKeyNotFoundException()
    {
        // Arrange
        var query = new GetSaleQuery(Guid.NewGuid());

        _saleRepository.GetByIdAsync(query.Id, Arg.Any<CancellationToken>()).Returns((Sale)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(query, default);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Sale with ID {query.Id} not found");

        _mapper.DidNotReceive().Map<GetSaleResult>(Arg.Any<Sale>());
    }
}