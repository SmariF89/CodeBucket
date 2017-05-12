using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Codebucket.Controllers
{
    public class ErrorController : Controller
    {
        // GET: /Error/
        public ActionResult Index()
        {
            Response.StatusCode = 500;
            return View("Error");
        }
        
        // GET: /Error/PageNotFound, see Web.config line 30-33. Uses this method when site gets status code
        // 404 and throws a custom "pagen not found".
        public ActionResult PageNotFound()
        {
            Response.StatusCode = 404;
            return View();
        }
    }

}