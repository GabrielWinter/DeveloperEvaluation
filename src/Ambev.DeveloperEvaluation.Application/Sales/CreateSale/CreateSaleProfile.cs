using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleProfile : Profile
{
    public CreateSaleProfile()
    {
        CreateMap<CreateSaleCommand, Sale>()
            .ForMember(s => s.Items, opt => opt.Ignore())
            .AfterMap((src, dest, ctx) =>
            {
                var items = ctx.Mapper.Map<IEnumerable<SaleItem>>(src.Items);
                dest.AddItemRange(items);
            });

        CreateMap<CreateSaleItemCommand, SaleItem>();
        CreateMap<Sale, CreateSaleResult>();
        CreateMap<SaleItem, CreateSaleItemResult>();
    }
}
