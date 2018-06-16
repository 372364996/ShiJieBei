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
        public ActionResult Rank()
        {
            var userList = _db.Users.Where(u => u.IsEmailValid).OrderByDescending(o => o.Account.Vouchers).ToList();
            ViewBag.MyRank = userList.IndexOf(CurrentUser);
            return View(userList);
        }
    }
}