using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.CreateSale;

public static class CreateSaleCommandTestData
{

    private static readonly Faker<CreateSaleItemCommand> createSaleItemCommandFaker = new Faker<CreateSaleItemCommand>()
    .RuleFor(u => u.ProductId, f => f.Random.Uuid())
    .RuleFor(u => u.ProductName, f => f.Commerce.ProductName())
    .RuleFor(u => u.Quantity, f => f.Random.Number(1, 20))
    .RuleFor(u => u.UnitPrice, f => f.Random.Decimal(1, 10));

    private static readonly Faker<CreateSaleCommand> createSaleCommandFaker = new Faker<CreateSaleCommand>()
        .RuleFor(u => u.CustomerId, f => f.Random.Uuid())
        .RuleFor(u => u.CustomerName, f => f.Person.FullName)
        .RuleFor(u => u.BranchId, f => f.Random.Uuid())
        .RuleFor(u => u.BranchName, f => $"Store {f.Name.LastName}")
        .RuleFor(u => u.Number, f => f.Random.Number(1, 100))
        .RuleFor(u => u.Date, f => f.Date.Past())
        .RuleFor(u => u.Items, f => createSaleItemCommandFaker.GenerateBetween(1, 4));

    public static CreateSaleCommand GenerateValidCommand()
    {
        return createSaleCommandFaker.Generate();
    }

    public static Sale GenerateSaleFromCommand(CreateSaleCommand command)
    {
        return new Sale
        {
            Id = Guid.NewGuid(),
            CustomerId = command.CustomerId,
            BranchId = command.BranchId,
            Date = command.Date,
            Items = command.Items.Select(i => new SaleItem
            {
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList()
        };
    }

    public static CreateSaleResult GenerateExpectedResultFromSale(Sale sale)
    {
        return new CreateSaleResult
        {
            Id = sale.Id,
            CustomerId = sale.CustomerId,
            BranchId = sale.BranchId,
            Date = sale.Date
        };
    }
}
