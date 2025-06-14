using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ProductMap : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products"); 

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Price).HasColumnType("decimal(18,2)");



        // Column mappings
        builder.Property(t => t.Id).HasColumnName("Id");
        builder.Property(t => t.Name).HasColumnName("Name");
        builder.Property(t => t.Price).HasColumnName("Price");
    }
}
