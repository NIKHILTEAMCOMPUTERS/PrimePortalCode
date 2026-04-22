using Microsoft.AspNetCore.Mvc.Filters;

namespace RMS.Filters
{
    public class BeforeActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //use filterContext.HttpContext.Request...
        }
    }
}
