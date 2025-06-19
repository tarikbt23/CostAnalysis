using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using CostAnalysisTBT.Business.Response;
using CostAnalysisTBT.Business.Services.RowParsing;
using CostAnalysisTBT.Business.Services.RequestHandling;
using CostAnalysisTBT.Business.Services.SchemaValidation;
using CostAnalysisTBT.Business.Exceptions;

namespace CostAnalysisTBT.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExcelUploadController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IRowParserService _rowParserService;
        private readonly IRequestFactory _requestFactory;
        private readonly IExcelSchemaValidator _excelSchemaValidator;

        public ExcelUploadController(
            IMediator mediator,
            IRowParserService rowParserService,
            IRequestFactory requestFactory,
            IExcelSchemaValidator excelSchemaValidator)
        {
            _mediator = mediator;
            _rowParserService = rowParserService;
            _requestFactory = requestFactory;
            _excelSchemaValidator = excelSchemaValidator;
        }

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ExcelUploadResponse>> UploadExcel(
            [FromForm] ExcelUploadRequest request,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (request.File == null || request.File.Length == 0)
                return BadRequest("Dosya bulunamadı.");

            if (string.IsNullOrWhiteSpace(request.FileType))
                return BadRequest("fileType form-data olarak gönderilmelidir.");

            if (Path.GetExtension(request.File.FileName)?.ToLower() != ".xlsx")
                return BadRequest("Sadece .xlsx dosyaları kabul edilir.");

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var stream = new MemoryStream();
            await request.File.CopyToAsync(stream, cancellationToken);
            using var package = new ExcelPackage(stream);

            var sheet = package.Workbook.Worksheets.FirstOrDefault();
            if (sheet == null)
                return BadRequest("Excel içinde hiç sayfa bulunamadı.");

            try
            {
                _excelSchemaValidator.ValidateSchema(request.FileType, sheet);

                var rows = _rowParserService.ParseRows(request.FileType, sheet);
                var command = _requestFactory.CreateRequest(request.FileType, rows, request.IsOverwrite);
                ((dynamic)command).IsOverwrite = request.IsOverwrite;
                var result = await _mediator.Send(command, cancellationToken);

                if (!result.Success)
                {
                    if (result.InvalidRowsExcelFile != null)
                    {
                        result.InvalidRowsExcelBase64 = Convert.ToBase64String(result.InvalidRowsExcelFile);
                        result.InvalidRowsExcelFile = null;
                    }
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch(BusinessException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }

        }
    }
}
