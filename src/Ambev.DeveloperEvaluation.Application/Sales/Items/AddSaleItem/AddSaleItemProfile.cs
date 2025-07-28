using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.Items.AddSaleItem;

public class AddSaleItemProfile : Profile
{
    public AddSaleItemProfile()
    {
        CreateMap<AddSaleItemCommand, SaleItem>();
        CreateMap<SaleItem, AddSaleItemResult>();
    }
}
