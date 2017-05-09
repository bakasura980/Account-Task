using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Innexys_Work.Singleton
{
    public static class UriApiBuilder
    {
        private static UriBuilder apiLink = new UriBuilder("http", ConfigurationManager.AppSettings["DomainName"], int.Parse(ConfigurationManager.AppSettings["Port"]));

        public static void SetUrlPathWithParam(string controller,string action, string queryParam, string queryArgument)
        {
            apiLink.Path = $"api/{controller}/{action}";
            apiLink.Query = $"{queryParam}={queryArgument}";
        }

        public static void SetUrlPath(string controller, string action)
        {
            apiLink.Path = $"api/{controller}/{action}";
        }

        public static Uri GetUri()
        {
            return apiLink.Uri;
        }
    }
}