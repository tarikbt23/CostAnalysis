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
    public class RequestFactory : IRequestFactory
    {
        public IRequest<ExcelUploadResponse> CreateRequest(string fileType, IList rows, bool isOverwrite)
        {
            var requestTypeName = $"CostAnalysisTBT.Business.Commands.{fileType}.Request";
            var requestType = Type.GetType(requestTypeName)
                                  ?? throw new Exception($"'{fileType}' için Request tipi bulunamadı: {requestTypeName}");

            var requestInstance = Activator.CreateInstance(requestType)
                                  ?? throw new Exception("Request nesnesi oluşturulamadı.");

            requestType.GetProperty("Rows")
                       ?.SetValue(requestInstance, rows);

            requestType.GetProperty("CreatedUser")
                       ?.SetValue(requestInstance, "tarik");

            requestType.GetProperty("IsOverwrite")
                       ?.SetValue(requestInstance, isOverwrite);

            return (IRequest<ExcelUploadResponse>)requestInstance;
        }
    }

}
