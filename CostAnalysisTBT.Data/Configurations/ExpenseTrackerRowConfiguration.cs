using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CostAnalysisTBT.Data.EF.Tables;

namespace CostAnalysisTBT.Data.Configurations
{
    public class ExpenseTrackerRowConfiguration : IEntityTypeConfiguration<tb_ExpenseTrackerRow>
    {
        public void Configure(EntityTypeBuilder<tb_ExpenseTrackerRow> builder)
        {
            builder.ToTable("CostAnalysis.ExpenseTrackerRow");

            builder.HasKey(x => x.ExpenseTrackerRowRef);

            builder.Property(x => x.ExpenseTrackerRowRef)
                   .ValueGeneratedOnAdd();

            builder.Property(x => x.ExpenseDate).IsRequired();
            builder.Property(x => x.ExpenseCategory).IsRequired();
            builder.Property(x => x.Vendor).IsRequired(false);
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.Amount).IsRequired();

            builder.HasOne(x => x.ExpenseTracker)
                   .WithMany(x => x.ExpenseTrackerRow)
                   .HasForeignKey(x => x.ExpenseTrackerRef);
        }
    }
}
