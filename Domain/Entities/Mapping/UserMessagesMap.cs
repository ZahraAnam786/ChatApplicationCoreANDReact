using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Domain.Entities.Mapping
{
    public class UserMessagesMap : IEntityTypeConfiguration<UserMessages>
    {
        public void Configure(EntityTypeBuilder<UserMessages> builder)
        {
            builder.ToTable("UserMessages");

            // Primary Key
            builder.HasKey(um => um.Id);

            // Property configurations
            builder.Property(um => um.Id)
                .HasColumnName("Id");

            builder.Property(um => um.SenderId)
                .HasColumnName("SenderId")
                .IsRequired();

            builder.Property(um => um.ReceiverId)
                .HasColumnName("ReceiverId")
                .IsRequired();

            builder.Property(um => um.Content)
                .HasColumnName("Content")
                .HasColumnType("nvarchar(max)")
                .IsRequired(false);

            builder.Property(um => um.IsSender)
                .HasColumnName("IsSender")
                .IsRequired();

            builder.Property(um => um.Timestamp)
                .HasColumnName("Timestamp")
                .IsRequired();
        }
    }

}
