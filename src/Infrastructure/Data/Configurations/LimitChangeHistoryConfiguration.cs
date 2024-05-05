using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AML.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AML.Infrastructure.Data.Configurations;
internal class LimitChangeHistoryConfiguration : IEntityTypeConfiguration<LimitChangeHistory>
{
    public void Configure(EntityTypeBuilder<LimitChangeHistory> builder)
    {
        builder.Property(lch => lch.Currency)
                  .IsRequired();

        builder.Property(lch => lch.OldLimitValue)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(lch => lch.NewLimitValue)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

    }
}
