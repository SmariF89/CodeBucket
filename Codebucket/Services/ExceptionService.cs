using Codebucket.Models;
using Codebucket.Models.Entities;
using System;

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

        // Saves a Exception log to the database
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