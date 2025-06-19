using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CostAnalysisTBT.Data.EF.Tables;

namespace CostAnalysisTBT.Data.Configurations
{
    public class IncomeStatementRowConfiguration : IEntityTypeConfiguration<tb_IncomeStatementRow>
    {
        public void Configure(EntityTypeBuilder<tb_IncomeStatementRow> builder)
        {
            builder.ToTable("CostAnalysis.IncomeStatementRow");

            builder.HasKey(x => x.IncomeStatementRowRef);

            builder.Property(x => x.IncomeStatementRowRef)
                   .ValueGeneratedOnAdd();

            builder.Property(x => x.IncomeStatementRef)
                   .IsRequired();

            builder.Property(x => x.IncomeDate)
                   .IsRequired();

            builder.Property(x => x.IncomeSource)
                   .IsRequired();

            builder.Property(x => x.Client )
                   .IsRequired();

            builder.Property(x => x.Description)
                   .IsRequired();

            builder.Property(x => x.Amount)
                   .IsRequired();

            builder.HasOne(x => x.IncomeStatement)
                   .WithMany(x => x.IncomeStatementRow)
                   .HasForeignKey(x => x.IncomeStatementRef);
        }
    }
}
