using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CostAnalysisTBT.Data.EF.Tables;

namespace CostAnalysisTBT.Data.Configurations
{
    public class ExpenseTrackerConfiguration : IEntityTypeConfiguration<tb_ExpenseTracker>
    {
        public void Configure(EntityTypeBuilder<tb_ExpenseTracker> builder)
        {
            builder.ToTable("CostAnalysis.ExpenseTracker");

            builder.HasKey(x => x.ExpenseTrackerRef);

            builder.Property(x => x.ExpenseTrackerRef)
                   .ValueGeneratedOnAdd();

            builder.Property(x => x.CreatedUser)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(x => x.CreatedDate)
                   .IsRequired();

            builder.Property(x => x.IsDeleted)
                   .IsRequired();

            builder.HasMany(x => x.ExpenseTrackerRow)
                   .WithOne(x => x.ExpenseTracker)
                   .HasForeignKey(x => x.ExpenseTrackerRef)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
