using System.Linq;
using System.Web.Mvc;

namespace MyApp.Framework.MvcToolkit.Filters
{
    public sealed class HandleBusinessValidationException : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            var fluentValidationException = filterContext.Exception as FluentValidation.ValidationException;

            if (fluentValidationException == null) return;

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new JsonResult
                {
                    Data = new
                    {
                        Success = false,
                        Errors = fluentValidationException.Errors.ToDictionary(a => a.PropertyName, e => e.ErrorMessage)
                    }
                };
            }
            else
            {
                foreach (var error in fluentValidationException.Errors)
                {
                    filterContext.Controller.ViewData.ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

            }

            filterContext.ExceptionHandled = true;
        }
    }
}
