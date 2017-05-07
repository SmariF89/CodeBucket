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

            ExceptionLogger exceptionLog = new ExceptionLogger
            {
                _controllerName = currentController,
                _actionName = currentAction,
                _exceptionMessage = ex.Message,
                _exceptionStackTrace = ex.StackTrace,
                _logTime = DateTime.Now,
                _userName = System.Web.HttpContext.Current.User.Identity.Name
            };

            _db._exceptions.Add(exceptionLog);
            _db.SaveChanges();
        }
    }
}
