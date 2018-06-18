using ShiJieBeiComponents.Helpers;
using ShiJieBeiComponents.Repositories.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShiJieBei.Areas.Mgr.Controllers
{
    public class HomeController : MgrController
    {
        public readonly ShiJieBeiDbContext _db = new ShiJieBeiDbContext();
        // GET: Mgr/Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string name, string pwd, string returnUrl)
        {
            string pwdHash = CryptoHelper.Md5(pwd);
            var mgr =_db.Managers.Where(m => m.Name == name && m.Password == pwdHash).SingleOrDefault();
            if (mgr != null)
            {
                Session["CurrentManager"] = mgr.Id;

                if (String.IsNullOrEmpty(returnUrl))
                {
                    returnUrl = "/mgr";
                }

                return Redirect(returnUrl);
            }

            return View((object)"用户名或密码不正确");
        }
    }
}