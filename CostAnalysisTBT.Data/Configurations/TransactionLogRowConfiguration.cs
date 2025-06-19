using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CostAnalysisTBT.Data.EF.Tables;

namespace CostAnalysisTBT.Data.Configurations
{
    public class TransactionLogRowConfiguration : IEntityTypeConfiguration<tb_TransactionLogRow>
    {
        public void Configure(EntityTypeBuilder<tb_TransactionLogRow> builder)
        {
            builder.ToTable("CostAnalysis.TransactionLogRow");

            builder.HasKey(x => x.TransactionLogRowRef);
            builder.Property(x => x.TransactionLogRowRef)
                   .ValueGeneratedOnAdd();

            builder.Property(x => x.TransactionLogRef)
                   .IsRequired();

            builder.Property(x => x.TransactionType)
                   .IsRequired();

            builder.Property(x => x.Amount)
                   .IsRequired();

            builder.Property(x => x.Description)
                   .IsRequired();

            builder.HasOne(x => x.TransactionLog)
                   .WithMany(x => x.TransactionLogRow)
                   .HasForeignKey(x => x.TransactionLogRef);
        }
    }
}
