using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Wiser.API.BL.Config;

namespace Wiser_WEB_API.Filters
{
    public class ExceptionHandlerFilter : IExceptionFilter
    {
        public readonly ILogger<ExceptionHandlerFilter> _logger;
        public ExceptionHandlerFilter(ILogger<ExceptionHandlerFilter> _logger)
        {
            this._logger = _logger;
        }
        public void OnException(ExceptionContext context)
        {
            string actionName = context.ActionDescriptor.DisplayName;
            _logger.LogError(context.Exception, string.Format("Exception in executing {0} in {1}", actionName, DateTime.UtcNow));
            HttpStatusCode status = HttpStatusCode.InternalServerError;
            HttpResponse response = context.HttpContext.Response;
            response.StatusCode = (int)status;
            response.ContentType = "application/json";
            response.WriteAsync(JsonConvert.SerializeObject(new Response<string>
            {
                Message = "An error has occured",
                Success = false
            }
           ));
        }
    }
}
