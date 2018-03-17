namespace VolleyManagement.UI.Infrastructure
{
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Crosscutting.Contracts.Infrastructure;

#pragma warning disable S3993 // Custom attributes should be marked with "System.AttributeUsageAttribute"
    /// <summary>
    /// The volley exception filter attribute.
    /// </summary>
    public class VolleyExceptionFilterAttribute : FilterAttribute, IExceptionFilter
#pragma warning restore S3993 // Custom attributes should be marked with "System.AttributeUsageAttribute"
    {
        #region Fields

        private static readonly Type _volleyAppBaseExceptionType = typeof(Exception);

        #endregion

        #region Injection

        /// <summary>
        /// Gets or sets the log.
        /// </summary>
        public ILog Log { get; set; }

        #endregion

        #region IExceptionFilter implementation

        /// <summary>
        /// The on exception.
        /// </summary>
        /// <param name="filterContext"> The filter context. </param>
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
            {
                return;
            }

            // Build result
            filterContext.Result = IsXmlHttpRequest(filterContext.RequestContext)
                                       ? BuildJsonResult(filterContext)
                                       : RedirectToErrorView(filterContext);

            // Log exception data
            if (_volleyAppBaseExceptionType.IsInstanceOfType(filterContext.Exception))
            {
                LogVolleyManagementException(filterContext);
            }
            else
            {
                LogUnhandledException(filterContext);
            }
        }

        #endregion

        #region Exception processing

        private static bool IsXmlHttpRequest(RequestContext requestContext)
        {
            return requestContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

        private ActionResult BuildJsonResult(ExceptionContext filterContext)
        {
            return new JsonResult {
                Data =
                               string.Format(
                                   "Unhandled exception occured. Message: {0}",
                                   filterContext.Exception.Message),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        private ActionResult RedirectToErrorView(ExceptionContext filterContext)
        {
            return new ViewResult { ViewName = "Error", ViewData = { Model = filterContext.Exception } };
        }

        #endregion

        #region Logging

        private void LogVolleyManagementException(ExceptionContext filterContext)
        {
            // Do logging
        }

        private void LogUnhandledException(ExceptionContext filterContext)
        {
            // Do logging
        }

        #endregion
    }
}