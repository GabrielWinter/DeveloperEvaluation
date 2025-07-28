using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.CreateSale;

public class CreateSaleCommandHandlerTests
{
    private readonly ISaleRepository _saleRepository = Substitute.For<ISaleRepository>();
    private readonly ISaleService _saleService = Substitute.For<ISaleService>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();

    private readonly CreateSaleCommandHandler _handler;

    public CreateSaleCommandHandlerTests()
    {
        _handler = new CreateSaleCommandHandler(_saleRepository, _saleService, _mapper);
    }

    [Fact(DisplayName = "Given valid CreateSaleCommand should return success response")]
    public async Task Handle_ValidCommand_ReturnsSuccessResponse()
    {
        // Arrange
        var command = CreateSaleCommandTestData.GenerateValidCommand();
        var sale = CreateSaleCommandTestData.GenerateSaleFromCommand(command);
        var createdSale = sale;
        var expectedResult = CreateSaleCommandTestData.GenerateExpectedResultFromSale(createdSale);

        _mapper.Map<Sale>(command).Returns(sale);
        _saleRepository.CreateAsync(sale, Arg.Any<CancellationToken>()).Returns(createdSale);
        _mapper.Map<CreateSaleResult>(createdSale).Returns(expectedResult);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(expectedResult.Id);

        await _saleRepository.Received(1).CreateAsync(sale, Arg.Any<CancellationToken>());
        _saleService.Received(1).CalculateAndApplyItemDiscounts(sale);
    }

    [Fact(DisplayName = "Given invalid CreateSaleCommand should throw ValidationException")]
    public async Task Handle_InvalidCommand_ThrowsValidationException()
    {
        // Arrange
        var command = new CreateSaleCommand();

        // Act
        Func<Task> act = async () => await _handler.Handle(command, default);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }
}
