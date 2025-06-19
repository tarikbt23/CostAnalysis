using MediatR;
using AutoMapper;
using FluentValidation;
using CostAnalysisTBT.Data.EF;
using CostAnalysisTBT.Business.Services;
using CostAnalysisTBT.Business.Models;
using EFCore.BulkExtensions;
using Microsoft.Data.SqlClient;
using CostAnalysisTBT.Business.Response;
using System.Reflection;
using CostAnalysisTBT.Business.Services.ExcelDataOverWrite;

namespace CostAnalysisTBT.Business
{
    public abstract class BaseExcelUploadHandler<TRequest, TRow, TEntity, THeader> : IRequestHandler<TRequest, ExcelUploadResponse>
        where TRequest : IRequest<ExcelUploadResponse>
        where TRow : CostAnalysisRowBase
        where TEntity : class
        where THeader : class, new()
    {
        protected readonly CostAnalysisDbContext _context;
        protected readonly IMapper _mapper;
        protected readonly InvalidRowExcelExporter _exporter;
        protected readonly IExcelDataOverwriteService _overwriteService;
        protected abstract IValidator<List<TRow>> Validator { get; }

        protected BaseExcelUploadHandler(
            CostAnalysisDbContext context,
            IMapper mapper,
            InvalidRowExcelExporter exporter,
            IExcelDataOverwriteService overwriteService)
        {
            _context = context;
            _mapper = mapper;
            _exporter = exporter;
            _overwriteService = overwriteService;
        }

        public async Task<ExcelUploadResponse> Handle(TRequest request, CancellationToken cancellationToken)
        {
            var rows = GetRows(request);
            var validationResult = Validator.Validate(rows);

            if (!validationResult.IsValid)
            {
                var invalidRows = rows.Where(r => !r.IsValid).ToList<CostAnalysisRowBase>();
                var excelBytes = _exporter.Export(invalidRows);
                var base64 = Convert.ToBase64String(excelBytes);
                var fileName = $"InvalidRows_{Guid.NewGuid():N}.xlsx";

                return new ExcelUploadResponse
                {
                    Success = false,
                    Message = "Hatalı satırlar tespit edildi.",
                    InvalidRowsExcelFile = excelBytes,
                    InvalidRowsExcelBase64 = base64,
                    FileName = fileName
                };
            }

            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                var isOverwriteProp = typeof(TRequest).GetProperty("IsOverwrite");
                var isOverwrite = (bool?)isOverwriteProp?.GetValue(request) ?? false;

                if (isOverwrite)
                {
                    await _overwriteService.OverwriteAsync(typeof(TEntity), typeof(THeader), cancellationToken);
                }

                var header = CreateHeaderBase(request);
                await _context.AddAsync(header, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                var entityRows = _mapper.Map<List<TEntity>>(rows);
                SetForeignKeyBase(entityRows, header);

                await _context.BulkInsertAsync(entityRows, new BulkConfig
                {
                    BatchSize = 1000,
                    SqlBulkCopyOptions = SqlBulkCopyOptions.TableLock
                }, cancellationToken: cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                return new ExcelUploadResponse
                {
                    Success = true,
                    Message = "Excel verileri başarıyla yüklendi."
                };
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        protected abstract List<TRow> GetRows(TRequest request);

        protected virtual THeader CreateHeaderBase(TRequest request)
        {
            var header = new THeader();
            var userProp = typeof(THeader).GetProperty("CreatedUser");
            var dateProp = typeof(THeader).GetProperty("CreatedDate");
            var deletedProp = typeof(THeader).GetProperty("IsDeleted");

            userProp?.SetValue(header, request?.GetType().GetProperty("CreatedUser")?.GetValue(request) ?? "bilinmiyor");
            dateProp?.SetValue(header, DateTime.Now);
            deletedProp?.SetValue(header, false);

            return header;
        }

        protected virtual void SetForeignKeyBase(List<TEntity> rows, object header)
        {
            var headerType = header.GetType();
            var idProp = headerType.GetProperties().FirstOrDefault(p => p.Name.EndsWith("Ref"));
            var idValue = idProp?.GetValue(header);

            var fkName = headerType.Name.Replace("tb_", "") + "Ref";
            var fkProp = typeof(TEntity).GetProperty(fkName);

            foreach (var row in rows)
            {
                fkProp?.SetValue(row, idValue);
            }
        }
    }
}
