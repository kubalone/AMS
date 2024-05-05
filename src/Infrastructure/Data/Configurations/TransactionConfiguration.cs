using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AML.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AML.Infrastructure.Data.Configurations;
internal class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder
            .HasOne(e => e.Customer)
            .WithMany(e => e.Transactions)
            .HasForeignKey(e => e.CustomerId)
            .IsRequired();

        builder.Property(t => t.Description)
            .HasMaxLength(200)
            .IsRequired();
        builder.Property(t => t.Description)
            .HasMaxLength(200)
            .IsRequired();
        builder.Property(t => t.Amount)
             .HasColumnType("decimal(18,2)") 
             .IsRequired(); 

        builder.Property(t => t.Currency)
               .IsRequired(); 


        builder.Property(t => t.TransactionType)
             .IsRequired();
        builder.Property(t => t.IsSuspicious)
               .IsRequired();


    }
}
