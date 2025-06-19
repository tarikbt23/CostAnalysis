using CostAnalysisTBT.Data.EF.Tables;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CostAnalysisTBT.Data.EF
{
    public class CostAnalysisDbContext : DbContext
    {
        public CostAnalysisDbContext(DbContextOptions<CostAnalysisDbContext> options) : base(options)
        { }

        public DbSet<tb_ExpenseTracker> ExpenseTracker { get; set; }
        public DbSet<tb_ExpenseTrackerRow> ExpenseTrackerRow { get; set; }
        public DbSet<tb_IncomeStatement> TransactionLog { get; set; }
        public DbSet<tb_TransactionLogRow> TransactionLogRow { get; set; }
        public DbSet<tb_IncomeStatement> IncomeStatement { get; set; }
        public DbSet<tb_IncomeStatementRow> IncomeStatementRow { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CostAnalysisDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
