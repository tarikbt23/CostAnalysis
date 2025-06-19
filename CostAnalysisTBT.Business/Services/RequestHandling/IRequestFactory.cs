using CostAnalysisTBT.Business.Response;
using MediatR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostAnalysisTBT.Business.Services.RequestHandling
{
    public interface IRequestFactory
    {
        IRequest<ExcelUploadResponse> CreateRequest(string fileType, IList rows, bool isOverwrite);
    }
}
