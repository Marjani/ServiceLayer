using System.Reflection;
using System.Web.Mvc;

namespace MyApp.Framework.MvcToolkit.ActionSelectors
{
    public sealed class IsMobileAttribute : ActionMethodSelectorAttribute
    {
        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            return controllerContext.HttpContext.Request.Browser.IsMobileDevice;
        }
    }
}