using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Mapping
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");

            builder.HasKey(p => p.UserID);

            builder.Property(u => u.UserID)
                   .ValueGeneratedOnAdd()
                   .HasColumnName("UserID");

            builder.Property(u => u.UserName)
                   .IsRequired()
                   .HasMaxLength(50)
                   .HasColumnName("UserName");

            builder.Property(u => u.Email)
                   .IsRequired()
                   .HasMaxLength(50)
                   .HasColumnName("Email");

            builder.Property(u => u.CreatedDate)
                   .IsRequired()
                   .HasColumnName("CreatedDate");

            builder.Property(u => u.Password)
                   .HasMaxLength(250)
                   .IsRequired(false)
                   .HasColumnName("Password");


            builder.Property(u => u.Image)
                .IsRequired(false)
                   .HasColumnName("Image");

            builder.Property(u => u.SignalID)
                .IsRequired(false)
                  .HasColumnName("SignalID");

        }
    }
}
