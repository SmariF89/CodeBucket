using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Codebucket.Utilities;

namespace Codebucket.Handlers
{
    public class CustomHandleErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            //Get the exception
            Exception ex = filterContext.Exception;

            //Get current controller and action
            string currentController = (string)filterContext.RouteData.Values["controller"];
            string currentActionName = (string)filterContext.RouteData.Values["action"];

            //Example using singleton logger class in Utilities folder which write exception to file
            ExceptionService.Instance.LogException(ex, currentController, currentActionName);

            //Set the view name to be returned, maybe return different error view for different exception types
            string viewName;

            if (ex is HttpRequestValidationException)
            {
                viewName = "MaliciousInputError";
            }
            else if (ex is Exception)
            {
                viewName = "Error";
            }
            else
            {
                viewName = "Error";
            }
            ///TODO::Add more to this.

            //Create the error model information
            HandleErrorInfo model = new HandleErrorInfo(filterContext.Exception, currentController, currentActionName);
            ViewResult result = new ViewResult
            {
                ViewName = viewName,
                ViewData = new ViewDataDictionary<HandleErrorInfo>(model),
                TempData = filterContext.Controller.TempData
            };

            filterContext.Result = result;
            filterContext.ExceptionHandled = true;

            // Call the base class implementation:
            base.OnException(filterContext);
        }
    }
}