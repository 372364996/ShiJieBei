using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ShiJieBei
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected static ILog logger = LogManager.GetLogger(typeof(MvcApplication));
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;
            HttpContext ctx = app.Context; //获取本次Http请求的HttpContext对象  

            if (ctx.User != null)
            {
                #region 每次请求的请求信息写入日志
                string msg = string.Format(@"{0}
URL:{1}
REFER:{2}
USER:{3}
用户IP:{4}
", "请求信息======",
                    ctx.Request.Url.ToString(),
                    ctx.Request.UrlReferrer != null ? ctx.Request.UrlReferrer.ToString() : "NULL",
                    ctx.User.Identity.IsAuthenticated ? ctx.User.Identity.Name : "NOT AUTH", ctx.Request.UserHostAddress);
                logger.Debug(msg);
                #endregion

                if (ctx.Request.IsAuthenticated == true) //验证过的一般用户才能进行角色验证  
                {
                    System.Web.Security.FormsIdentity fi = (System.Web.Security.FormsIdentity)ctx.User.Identity;
                    System.Web.Security.FormsAuthenticationTicket ticket = fi.Ticket; //取得身份验证票  
                    string userData = ticket.UserData;//
                    logger.Debug("从UserData中恢复role信息=====" + userData);
                    string[] roles = userData.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries); //将角色数据转成字符串数组,得到相关的角色信息  
                    ctx.User = new System.Security.Principal.GenericPrincipal(fi, roles); //这样当前用户就拥有角色信息了
                }
            }
        }
    }
}
