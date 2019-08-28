using System.Net;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PartnerFinder.CustomFilters
{
    public class ObjectExistenceFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception.GetType() != typeof(ObjectNotFoundException)) return;
            var result = new ObjectResult(new { message = context.Exception.Message }) { StatusCode = (int)HttpStatusCode.NotFound };
            context.Result = result;
        }
    }
}
