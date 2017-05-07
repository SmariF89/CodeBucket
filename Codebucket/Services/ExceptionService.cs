using Codebucket.Models;
using Codebucket.Models.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Codebucket.Utilities
{
    public class ExceptionService
    {
        private ApplicationDbContext _db;
        private static ExceptionService theInstance = null;
        public static ExceptionService Instance
        {
            get
            {
                if (theInstance == null)
                {
                    theInstance = new ExceptionService();
                }
                return theInstance;
            }
        }

        public void LogException(Exception ex, string currentController, string currentAction)
        {
            _db = new ApplicationDbContext();         
            ExceptionLogger exceptionLog = new ExceptionLogger();

            exceptionLog._controllerName = currentController;
            exceptionLog._actionName = currentAction;
            exceptionLog._exceptionMessage = ex.Message;
            exceptionLog._exceptionStackTrace = ex.StackTrace;
            exceptionLog._logTime = DateTime.Now;
            exceptionLog._userName = System.Web.HttpContext.Current.User.Identity.Name;

            _db._exceptions.Add(exceptionLog);
            _db.SaveChanges();
        }
    }
}