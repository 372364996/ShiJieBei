using System.Web.Mvc;

namespace ShiJieBei.Areas.Mgr
{
    public class MgrAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Mgr";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Mgr_default",
                "Mgr/{controller}/{action}/{id}",
                new { action = "Index", controller = "Home", id = UrlParameter.Optional },
                new string[] { "ShiJieBei.Areas.Mgr.Controllers" });
        }
    }
}