using CostAnalysisTBT.Data.EF;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CostAnalysisTBT.Business.Services.ExcelDataOverWrite
{
    public class ExcelDataOverwriteService : IExcelDataOverwriteService
    {
        private readonly CostAnalysisDbContext _context;

        public ExcelDataOverwriteService(CostAnalysisDbContext context)
        {
            _context = context;
        }

        public async Task OverwriteAsync(Type rowType, Type headerType, CancellationToken cancellationToken)
        {
            var rowSet = GetDbSet(rowType);
            var existingRows = await ToListAsync(rowSet, cancellationToken);

            var removeMethod = rowSet.GetType()
                .GetMethods()
                .FirstOrDefault(m =>
                    m.Name == "RemoveRange" &&
                    m.GetParameters().Length == 1 &&
                    m.GetParameters()[0].ParameterType.IsGenericType &&
                    m.GetParameters()[0].ParameterType.GetGenericTypeDefinition() == typeof(IEnumerable<>));

            var castedList = typeof(Enumerable)
                .GetMethod(nameof(Enumerable.Cast))!
                .MakeGenericMethod(rowType)
                .Invoke(null, [existingRows]);

            removeMethod?.Invoke(rowSet, [castedList!]);

            var headerSet = GetDbSet(headerType);
            var existingHeaders = await ToListAsync(headerSet, cancellationToken);

            foreach (var header in existingHeaders)
            {
                var isDeletedProp = header.GetType().GetProperty("IsDeleted");
                isDeletedProp?.SetValue(header, true);
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

        private object GetDbSet(Type entityType)
        {
            var method = typeof(DbContext).GetMethods()
                .First(m => m.Name == "Set" && m.IsGenericMethod && m.GetParameters().Length == 0);

            var genericMethod = method.MakeGenericMethod(entityType);
            return genericMethod.Invoke(_context, null)!;
        }

        private async Task<List<object>> ToListAsync(object dbSet, CancellationToken cancellationToken)
        {
            var method = typeof(EntityFrameworkQueryableExtensions)
                .GetMethods()
                .FirstOrDefault(m =>
                    m.Name == "ToListAsync"
                    && m.IsGenericMethod
                    && m.GetParameters().Length == 2
                    && m.GetParameters()[0].ParameterType.GetGenericTypeDefinition() == typeof(IQueryable<>));

            var entityType = dbSet.GetType().GetInterfaces()
                .FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IQueryable<>))
                ?.GetGenericArguments()[0];

            if (entityType == null || method == null)
                throw new InvalidOperationException("ToListAsync method or entity type not resolved.");

            var genericMethod = method.MakeGenericMethod(entityType);
            var task = (Task)genericMethod.Invoke(null, [dbSet, cancellationToken])!;
            await task.ConfigureAwait(false);

            var result = task.GetType().GetProperty("Result")!.GetValue(task);
            return ((IEnumerable<object>)result!).ToList();
        }
    }
}
