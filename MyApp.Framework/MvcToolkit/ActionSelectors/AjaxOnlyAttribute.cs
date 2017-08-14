using System.Reflection;
using System.Web.Mvc;

namespace MyApp.Framework.MvcToolkit.ActionSelectors
{
    public sealed class AjaxOnlyAttribute : ActionMethodSelectorAttribute
    {
        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            return controllerContext.HttpContext.Request.IsAjaxRequest();
        }
    }
}