using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Items.CancelSaleItem;

public class CancelSaleItemCommandHandler : IRequestHandler<CancelSaleItemCommand, CancelSaleItemResult>
{
    private readonly ISaleRepository _saleRepository;

    public CancelSaleItemCommandHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public async Task<CancelSaleItemResult> Handle(CancelSaleItemCommand command, CancellationToken cancellationToken)
    {
        var validator = new CancelSaleItemCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var sale = await _saleRepository.GetByIdAsync(command.SaleId, cancellationToken);
        if (sale is null)
            throw new KeyNotFoundException($"Sale with ID {command.SaleId} not found");

        var cancelledItem = sale.CancelItem(command.ItemId);
        if (cancelledItem is null)
            throw new KeyNotFoundException($"Sale Item with ID {command.ItemId} not found");

        
        await _saleRepository.UpdateAsync(sale, cancellationToken);
        return new CancelSaleItemResult { Success = true };
    }
}
