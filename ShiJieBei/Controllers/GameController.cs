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
        [AllowAnonymous]
        public ActionResult Index()
        {
            var model = new GameViewModel 
            {
                Games = _db.Games.ToList()
            };
            var data = model.Games.Where(g => g.StartTime.AddMinutes(90) > DateTime.Now).OrderBy(o => o.StartTime).FirstOrDefault();
            ViewBag.GameList = model.Games;
            return View(model);
        }
        [AllowAnonymous]
        public ActionResult GoIndex()
        {
          
            var data = _db.Games.ToList().Where(g => g.StartTime.AddMinutes(90) > DateTime.Now).OrderBy(o => o.StartTime).FirstOrDefault();
            return Redirect($"/game/#{data.Id}");
        }
        public ActionResult Rank()
        {
            var userList = _db.Users.Where(u => u.IsEmailValid).OrderByDescending(o => o.Account.Vouchers).ToList();
            ViewBag.MyRank = userList.IndexOf(CurrentUser);
            return View(userList);
        }
        [AllowAnonymous]
        public ActionResult Play(int gameId)
        {

            var game = _db.Games.FirstOrDefault(g => g.Id == gameId);
            if (game == null)
            {
                return RedirectToAction("Index");
            }
            PlayGameViewModel data = new PlayGameViewModel
            {
                Game = game,
                User = CurrentUser,
                GameOrders = game.GameOrders
            };
            ViewBag.IsLogin = CurrentUser != null;
            return View(data);
        }

    }
}