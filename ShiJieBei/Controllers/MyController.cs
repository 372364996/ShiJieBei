using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShiJieBei.Controllers
{
    [Authorize(Roles = "user")]
    public class MyController : ShiJieBeiController
    {
        // GET: My
        public ActionResult Index()
        {
            return View(CurrentUser);
        }
    }
}