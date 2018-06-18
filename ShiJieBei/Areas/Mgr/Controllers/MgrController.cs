using log4net;
using Newtonsoft.Json;
using ShiJieBeiComponents.Domains;
using ShiJieBeiComponents.Helpers;
using ShiJieBeiComponents.Repositories.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShiJieBei.Areas.Mgr.Controllers
{
    public class MgrController : Controller
    {
        protected static ILog logger = LogManager.GetLogger(typeof(MgrController));
        public readonly ShiJieBeiDbContext _db = new ShiJieBeiDbContext();
        private Manager mgr = null;
        protected Manager CurrentManager
        {
            get
            {
                if (mgr == null)
                {
                    int userId = Convert.ToInt32(User.Identity.Name);
                    mgr = _db.Managers.Where(m => m.UserId == userId && m.Status == ManagerStatus.Normal).Single();
                }
                return mgr;
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var mgr = filterContext.HttpContext.Session["CurrentManager"];
            if (mgr == null && filterContext.HttpContext.Request.Url.ToString().ToLower().IndexOf("mgr/home/login") == -1)
            {
                filterContext.HttpContext.Response.Redirect("/mgr/home/login?returnUrl=" + HttpUtility.UrlEncode(filterContext.HttpContext.Request.Url.ToString()));
                filterContext.HttpContext.Response.End();
            }
            else
            {
                base.OnActionExecuting(filterContext);
            }
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            string error = Utils.GetRandomString("0123456789", 6);
            Session["errorcode"] = error;
            var context = filterContext.HttpContext;
            string data = "";
            if (context.Request.Form != null && context.Request.Form.Count > 0)
            {
                data = JsonConvert.SerializeObject(context.Request.Form);
            }
            string msg = String.Format(@"{0}
URL:{1}
REFER:{2}
USER:{3}
DATA:{4}
{5}", error,
                context.Request.Url.ToString(),
                context.Request.UrlReferrer != null ? filterContext.HttpContext.Request.UrlReferrer.ToString() : "NULL",
                context.User.Identity.IsAuthenticated ? context.User.Identity.Name : "NOT AUTH",
                data,
                Utils.ExceptionToString(filterContext.Exception));
            logger.Error(msg);
            //发送错误日志Email
            Utils.SendErrorLogEmail(context, error, msg);
            base.OnException(filterContext);
        }
    }
}