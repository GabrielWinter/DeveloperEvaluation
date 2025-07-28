using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Items.AddSaleItem;

public class AddSaleItemCommandHandler : IRequestHandler<AddSaleItemCommand, AddSaleItemResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly ISaleService _saleService;
    private readonly IMapper _mapper;

    public AddSaleItemCommandHandler(ISaleRepository saleRepository, ISaleService saleService, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _saleService = saleService;
        _mapper = mapper;
    }

    public async Task<AddSaleItemResult> Handle(AddSaleItemCommand command, CancellationToken cancellationToken)
    {
        var validator = new AddSaleItemCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var sale = await _saleRepository.GetByIdAsync(command.SaleId, cancellationToken);
        if (sale is null)
            throw new KeyNotFoundException($"Sale with ID {command.SaleId} not found");

        var saleItem = _mapper.Map<SaleItem>(command);
        sale.AddItem(saleItem);
        _saleService.CalculateAndApplyItemDiscounts(sale);
        await _saleRepository.UpdateAsync(sale, cancellationToken);

        return _mapper.Map<AddSaleItemResult>(saleItem);
    }
}
