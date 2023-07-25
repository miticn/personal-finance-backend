using Finance.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Transaction.Database.Entities;

public class TransactionSplitEntityTypeConfiguration : IEntityTypeConfiguration<TransactionSplitEntity>
{
    public void Configure(EntityTypeBuilder<TransactionSplitEntity> builder)
    {
        builder.ToTable("TransactionSplits");

        // Composite key
        builder.HasKey(t => new { t.TransactionId, t.catcode });

        builder.Property(t => t.TransactionId)
            .IsRequired();

        builder.Property(t => t.catcode)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(t => t.amount)
            .IsRequired();

        builder.HasOne(t => t.Category)
            .WithMany()
            .HasForeignKey(t => t.catcode)
            .IsRequired();
    }
}