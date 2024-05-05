using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AML.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Reflection.Metadata;

namespace AML.Infrastructure.Data.Configurations;
internal class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder
           .HasMany(p => p.Transactions)
           .WithOne(p => p.Customer)
           .HasForeignKey(e => e.CustomerId)
           .IsRequired();

        builder
           .HasMany(p => p.Balances)
           .WithOne(p => p.Customer)
           .HasForeignKey(e => e.CustomerId)
           .IsRequired();

        builder
           .HasOne(p => p.Address)
           .WithOne(e => e.Customer)
           .HasForeignKey<Address>(e => e.CustomerId)
           .IsRequired();

        builder.Property(t => t.FirstName)
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(t => t.LastName)
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(c => c.DateOfBirth)
            .IsRequired();
    }
}
