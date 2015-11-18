using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5HW.ActionFilters
{
    public class TimeFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Debug.WriteLine("OnActionExecuting:"+DateTime.Now);
            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            Debug.WriteLine("OnActionExecuted:" + DateTime.Now);
            base.OnActionExecuted(filterContext);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            Debug.WriteLine("OnResultExecuting:" + DateTime.Now);
            base.OnResultExecuting(filterContext);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            Debug.WriteLine("OnResultExecuting:" + DateTime.Now);
            base.OnResultExecuted(filterContext);
        }
    }
}