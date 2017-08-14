using System;
using System.Linq;
using System.Net.Http;
using System.Web;
using MyApp.Framework.Application.Models;
using Newtonsoft.Json;

namespace MyApp.Framework.Extensions
{
    public static class HttpExtensions
    {
        private const string HttpContext = "MS_HttpContext";

        public static T ToListRequest<T>(this HttpRequestBase requestBase) where T : DynamicListRequest
        {
            var queryString = requestBase.QueryString[0];

            return JsonConvert.DeserializeObject<T>(queryString);
        }

        public static Uri GetUrlReferrer(this HttpRequestMessage request)
        {
            if (!request.Properties.ContainsKey(HttpContext)) return null;
            var ctx = request.Properties[HttpContext] as HttpContextWrapper;
            return ctx?.Request.UrlReferrer;
        }

        public static string PhysicalToVirtualPathConverter(this HttpServerUtilityBase utility, string path,
            HttpRequestBase context)
        {
            return path.Replace(context.ServerVariables["APPL_PHYSICAL_PATH"], "/").Replace(@"\", "/");
        }

        public static string AbsoluteContent(HttpRequest request,
            string contentPath)
        {
            return new Uri(request.Url, VirtualPathUtility.ToAbsolute(contentPath)).ToString();
        }

        public static string GetUserIp(this HttpRequestBase request)
        {
            string ip = null;
            try
            {
                if (request.IsSecureConnection)
                    ip = request.ServerVariables["REMOTE_ADDR"];

                if (string.IsNullOrEmpty(ip))
                {
                    ip = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                    if (!string.IsNullOrEmpty(ip))
                    {
                        if (ip.IndexOf(",", StringComparison.Ordinal) > 0)
                            ip = ip.Split(',').Last();
                    }
                    else
                    {
                        ip = request.UserHostAddress;
                    }
                }
            }
            catch (Exception)
            {
                ip = null;
            }

            return ip;
        }

        public static string GetBrowser(this HttpRequestBase request)
        {
            return $"{request.Browser.Browser} - {request.Browser.Version}";
        }
    }
}