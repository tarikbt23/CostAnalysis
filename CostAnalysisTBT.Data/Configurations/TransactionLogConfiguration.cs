using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CostAnalysisTBT.Data.EF.Tables;

namespace CostAnalysisTBT.Data.Configurations
{
    public class TransactionLogConfiguration : IEntityTypeConfiguration<tb_TransactionLog>
    {
        public void Configure(EntityTypeBuilder<tb_TransactionLog> builder)
        {
            builder.ToTable("CostAnalysis.TransactionLog");

            builder.HasKey(x => x.TransactionLogRef);
            builder.Property(x => x.TransactionLogRef)
                   .ValueGeneratedOnAdd();

            builder.Property(x => x.CreatedUser)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(x => x.CreatedDate)
                   .IsRequired();

            builder.Property(x => x.IsDeleted)
                   .IsRequired();

            builder.HasMany(x => x.TransactionLogRow)
                   .WithOne(x => x.TransactionLog)
                   .HasForeignKey(x => x.TransactionLogRef)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
