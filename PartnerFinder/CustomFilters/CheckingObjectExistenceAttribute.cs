using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PartnerFinder.CustomFilters
{
    public class CheckingObjectExistenceAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var objectResult = context.Result as ObjectResult;

            if (objectResult?.Value == null)
                context.Result = new NotFoundResult();
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {

        }
    }
}
