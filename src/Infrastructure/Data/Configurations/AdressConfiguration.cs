using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AML.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AML.Infrastructure.Data.Configurations;
internal class AdressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder
          .HasOne(e => e.Customer)
          .WithOne(e => e.Address)
          .HasForeignKey<Address>(e => e.CustomerId)
          .IsRequired();

        builder.Property(t => t.Street)
            .HasMaxLength(30)
            .IsRequired();
        builder.Property(t => t.City)
            .HasMaxLength(30)
            .IsRequired();
        builder.Property(t => t.ZipCode)
            .HasMaxLength(10)
            .IsRequired();
        builder.Property(t => t.Country)
            .HasMaxLength(20)
            .IsRequired();

    }
}
