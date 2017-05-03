using Codebucket.Models.ViewModels;
using Codebucket.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Codebucket.Controllers
{
    public class ApplicationUserController : Controller
    {
        ApplicationUserService _applicationUserService = new ApplicationUserService();

        // GET: ApplicationUser
        public ActionResult Index()
        {
            return View();
        }

        // GET: CreateApplicationUser
        [HttpGet]
        public ActionResult CreateApplicationUser()
        {
            return null;
        }

        // POST: CreateApplicationUser
        [HttpPost]
        public ActionResult CreateApplicationUser(ApplicationUserViewModel model)
        {
            return null;
        }

        //// GET: EditApplicationUser
        //[HttpGet]
        //public ActionResult EditApplicationUser(int? id)
        //{
        //    return null;
        //}

        //// POST: EditApplicationUser
        //[HttpPost]
        //public ActionResult EditApplicationUser(ApplicationUserViewModel model)
        //{
        //    return null;
        //}
    
    }
}