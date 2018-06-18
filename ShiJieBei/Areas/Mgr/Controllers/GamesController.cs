using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShiJieBeiComponents.Domains;
using ShiJieBeiComponents.Helpers;
using ShiJieBeiComponents.Repositories.EF;

namespace ShiJieBei.Areas.Mgr.Controllers
{
    public class GamesController : MgrController
    {
        private ShiJieBeiDbContext db = new ShiJieBeiDbContext();

        // GET: Mgr/Games
        public ActionResult Index()
        {
            return View(db.Games.ToList());
        }

        // GET: Mgr/Games/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Games games = db.Games.Find(id);
            if (games == null)
            {
                return HttpNotFound();
            }
            return View(games);
        }

        // GET: Mgr/Games/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Mgr/Games/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ZhuChang,KeChang,ZhuChangSuoXie,KeChangSuoXie,ZhuChangScore,KeChangScore,Status,StartTime")] Games games)
        {
            if (ModelState.IsValid)
            {
                games.CreateTime = DateTime.Now;
                db.Games.Add(games);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(games);
        }

        // GET: Mgr/Games/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Games games = db.Games.Find(id);
            if (games == null)
            {
                return HttpNotFound();
            }
            return View(games);
        }

        // POST: Mgr/Games/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ZhuChang,KeChang,ZhuChangSuoXie,KeChangSuoXie,ZhuChangScore,KeChangScore,Status,StartTime")] Games games)
        {
            if (ModelState.IsValid)
            {
                games.CreateTime = DateTime.Now;
                db.Entry(games).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(games);
        }

        // GET: Mgr/Games/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Games games = db.Games.Find(id);
            if (games == null)
            {
                return HttpNotFound();
            }
            return View(games);
        }

        // POST: Mgr/Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Games games = db.Games.Find(id);
            db.Games.Remove(games);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult JieSuan(int id)
        {
            Games game = db.Games.Find(id);
            if (game.IsDone)
            {
                return Json(new { success = false,msg="已结算" });
            }
            game.IsDone = true;
            db.SaveChanges();
            var totalVouchers = game.GameOrders.Count * 20;
            var winGames = game.GameOrders.Where(o => o.GameOrderStatus == game.Status).ToList();

            decimal vouchers = totalVouchers / winGames.Count;
            foreach (var item in game.GameOrders)
            {
                if (item.GameOrderStatus==game.Status)
                {
                    item.IsWin = true;
                }
                else
                {
                    item.IsWin = false;
                }
            }
            db.SaveChanges();      
            foreach (var item in winGames)
            {
                try
                {
                    Charge(item.User, Utils.GetChargeNumber(item.UserId), vouchers, $"{item.User.Name}下注比赛{game.ZhuChang}VS{game.KeChang},买{item.GameOrderStatus},获得{vouchers}积分",game.Id);
                }
                catch (Exception exp)
                {
                    logger.Error("给用户：" + item.UserId + "结算失败:", exp);
                    return Json(new { success = false, userid = item.UserId, Money = "0", MoneyLocked = "0", Vouchers = "0" });
                }
            }
            return Json(new { success = true });

            
        }
        public void Charge(User user, string number, decimal vouchers, string description,int gameId)
        {
            var log = db.AccountVouchersLog.FirstOrDefault(a => a.Number == number);
            if (log != null)
            {
                throw new Exception(String.Format("订单号{0}已经处理过,Description:{1},Vouchers:{2}", number, description, vouchers));
            }
            //生成账户记录
            AccountVouchersLog accountLog = new AccountVouchersLog()
            {
                Account = user.Account,
                AccountId = user.Id,
                Before = user.Account.Vouchers,
                After = user.Account.Vouchers + vouchers,
                Vouchers = vouchers,
                Description = description,
                CreateTime = DateTime.Now,
                Number = number,
                Type = AccountVouchersLogType.Income,
                DetailId=gameId
            };
            db.AccountVouchersLog.Add(accountLog);
            //修改账户余额
            var userData = db.Users.Where(u => u.Id == user.Id).FirstOrDefault();
            userData.Account.Vouchers += vouchers;

            db.SaveChanges();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
