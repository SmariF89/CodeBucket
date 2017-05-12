using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Codebucket.Controllers
{
    public class HomeController : Controller
    {
        // Redirects the action from the home controller index to overview index.
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Overview");
        }
    }
}