using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShiJieBei.Controllers
{
    [Authorize(Roles = "user")]
    public class GameController : ShiJieBeiController
    {
        // GET: Game
        public ActionResult Index()
        {
            ViewBag.Name = CurrentUser.Name;
            ViewBag.Vouchers = CurrentUser.Account.Vouchers;
            return View();
        }
    }
}