using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AML.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AML.Infrastructure.Data.Configurations;
internal class TransactionLimitConfiguration : IEntityTypeConfiguration<TransactionLimit>
{
    public void Configure(EntityTypeBuilder<TransactionLimit> builder)
    {
        builder.Property(tl => tl.Currency)
               .IsRequired();

        builder.Property(tl => tl.LimitValue)
               .HasColumnType("decimal(18,2)")
               .IsRequired();
    }
}
