using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using log4net;
using Newtonsoft.Json;
using ShiJieBeiComponents.Helpers;
using ShiJieBeiComponents.Domains;
using ShiJieBeiComponents.Repositories.EF;

namespace ShiJieBei.Controllers
{
    public class ShiJieBeiController : Controller
    {
        protected static ILog logger = LogManager.GetLogger(typeof(ShiJieBeiController));

        private User user = null;
        private readonly ShiJieBeiDbContext _db=new ShiJieBeiDbContext();
        
        protected User CurrentUser
        {
            get
            {
                if (user == null && User.Identity.IsAuthenticated)
                {
                    var userId = Convert.ToInt32(User.Identity.Name);
                    user = _db.Users.SingleOrDefault(u=>u.Id== userId);
                }

                return user;
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