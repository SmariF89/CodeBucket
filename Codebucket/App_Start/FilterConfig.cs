using System.Web;
using System.Web.Mvc;
using Codebucket.Handlers;

namespace Codebucket
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());   // old
            filters.Add(new CustomHandleErrorAttribute());            
            filters.Add(new AuthorizeAttribute());
        }
    }
}
