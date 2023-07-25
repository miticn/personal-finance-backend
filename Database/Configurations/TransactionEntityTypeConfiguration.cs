using Finance.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Transaction.Database.Entities;

public class TransactionEntityTypeConfiguration : IEntityTypeConfiguration<TransactionEntity>
{
    public void Configure(EntityTypeBuilder<TransactionEntity> builder)
    {
        builder.ToTable("Transactions");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.BeneficiaryName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(t => t.Date)
           .IsRequired();

        builder.Property(t => t.Direction)
            .IsRequired()
            .HasMaxLength(1);

        builder.Property(t => t.Amount)
            .IsRequired();

        builder.Property(t => t.Description)
            .HasMaxLength(500);

        builder.Property(t => t.Currency)
            .IsRequired()
            .HasMaxLength(3);

        builder.Property(t => t.MCC);

        builder.Property(t => t.Kind)
            .HasMaxLength(3);

        builder.Property(t => t.Catcode)
            .HasMaxLength(100);

        builder.HasOne<CategoryEntity>(t => t.Category)
            .WithMany()
            .HasForeignKey(t => t.Catcode);
    }
}
