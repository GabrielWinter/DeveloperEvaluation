using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
{
    public void Configure(EntityTypeBuilder<SaleItem> builder)
    {
        builder.ToTable("SaleItems");

        builder.HasKey(si => si.Id);
        builder.Property(si => si.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(si => si.SaleId).HasColumnType("uuid");
        builder.Property(si => si.ProductId).IsRequired().HasColumnType("uuid");
        builder.Property(si => si.ProductName).IsRequired().HasMaxLength(135);
        builder.Property(si => si.Quantity);
        builder.Property(si => si.UnitPrice);
        builder.Property(si => si.UnitDiscount);
        builder.Property(s => s.IsCancelled);

        builder.Ignore(si => si.TotalDiscount);
        builder.Ignore(si => si.Subtotal);
        builder.Ignore(si => si.Total);

        builder.HasOne(si => si.Sale)
            .WithMany(s => s.Items)
            .HasForeignKey(si => si.SaleId);

    }
}
