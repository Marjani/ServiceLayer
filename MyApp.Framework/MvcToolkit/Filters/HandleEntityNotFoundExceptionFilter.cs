using System.Web.Mvc;
using MyApp.Framework.Domain.Entities;

namespace MyApp.Framework.MvcToolkit.Filters
{
    public sealed class HandleEntityNotFoundExceptionFilter : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            var entityNotFoundException = filterContext.Exception as EntityNotFoundException;

            if (entityNotFoundException == null) return;

            filterContext.Result = new HttpNotFoundResult();

            filterContext.ExceptionHandled = true;
        }
    }
}