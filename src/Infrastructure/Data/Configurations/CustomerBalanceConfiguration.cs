using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using AML.Domain.Entities;

namespace AML.Infrastructure.Data.Configurations;
internal class CustomerBalanceConfiguration : IEntityTypeConfiguration<CustomerBalance>
{
    public void Configure(EntityTypeBuilder<CustomerBalance> builder)
    {
        builder
            .HasOne(e => e.Customer)
            .WithMany(e => e.Balances)
            .HasForeignKey(e => e.CustomerId)
            .IsRequired();

        builder.Property(t => t.Currency)
               .IsRequired();

        builder.Property(t => t.Balance)
               .IsRequired();



    }
}
