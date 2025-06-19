using CostAnalysisTBT.Data.EF.Tables;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostAnalysisTBT.Data.Configurations
{
    public class IncomeStatementConfiguration : IEntityTypeConfiguration<tb_IncomeStatement>
    {
        public void Configure(EntityTypeBuilder<tb_IncomeStatement> builder)
        {
            builder.ToTable("CostAnalysis.IncomeStatement");

            builder.HasKey(x => x.IncomeStatementRef);
            builder.Property(x => x.IncomeStatementRef)
                   .ValueGeneratedOnAdd();

            builder.Property(x => x.CreatedUser)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(x => x.CreatedDate)
                   .IsRequired();

            builder.Property(x => x.IsDeleted)
                   .IsRequired();

            builder.HasMany(x => x.IncomeStatementRow)
                   .WithOne(x => x.IncomeStatement)
                   .HasForeignKey(x => x.IncomeStatementRef)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
