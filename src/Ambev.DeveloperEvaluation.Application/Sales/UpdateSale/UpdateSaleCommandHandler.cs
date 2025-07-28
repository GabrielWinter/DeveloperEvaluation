using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

public class UpdateSaleCommandHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    public UpdateSaleCommandHandler(ISaleRepository saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    public async Task<UpdateSaleResult> Handle(UpdateSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new UpdateSaleCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var dbSale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);
        if(dbSale is null)
            throw new KeyNotFoundException($"Sale with ID {command.Id} not found");

        dbSale.Update(
            command.CustomerId, 
            command.CustomerName, 
            command.BranchId, 
            command.BranchName, 
            command.Number,
            command.Date);

        var updatedSale = await _saleRepository.UpdateAsync(dbSale, cancellationToken);
        return _mapper.Map<UpdateSaleResult>(updatedSale);
    }
}
