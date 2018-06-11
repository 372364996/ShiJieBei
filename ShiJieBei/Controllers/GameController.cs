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
            return View();
        }
    }
}