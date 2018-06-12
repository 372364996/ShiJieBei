using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShiJieBei.Models;

namespace ShiJieBei.Controllers
{
    [Authorize(Roles = "user")]
    public class GameController : ShiJieBeiController
    {
        // GET: Game
        public ActionResult Index()
        {
            var model = new GameViewModel
            {
                User = CurrentUser,
                Games = _db.Games.ToList()
            };
            return View(model);
        }
    }
}