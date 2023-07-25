using Finance.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Transaction.Database.Entities;

public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<CategoryEntity>
{
    public void Configure(EntityTypeBuilder<CategoryEntity> builder)
    {
        builder.ToTable("Categories");

        builder.HasKey(t => t.Code);

        builder.Property(t => t.Code)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(t => t.Name)
           .IsRequired();

        builder.Property(t => t.ParentCode);

        // Define self-referencing relationship
        builder.HasOne(t => t.Parent)
               .WithMany(t => t.Children)
               .HasForeignKey(t => t.ParentCode)
               .OnDelete(DeleteBehavior.Restrict);
    }
}