using Ambev.DeveloperEvaluation.Application.Sales.Items.AddSaleItem;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.AddSaleItem;

public class AddSaleItemProfile : Profile
{
    public AddSaleItemProfile()
    {
        CreateMap<AddSaleItemRequest, AddSaleItemCommand>();
        CreateMap<AddSaleItemResult, AddSaleItemResponse>();
    }
}
