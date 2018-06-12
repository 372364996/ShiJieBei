using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShiJieBeiComponents.Domains;
using ShiJieBeiComponents.Helpers;

namespace ShiJieBei.Controllers
{
    public class GameOrderController : ShiJieBeiController
    {
        /// <summary>
        /// 下注（生成订单）
        /// </summary>
        /// <returns></returns>
        public JsonResult CreateOrder(int gameId, int userId, int gameResult)
        {

            var game = _db.Games.Find(gameId);
            var gameOrder = new GameOrders();
            gameOrder.GameId = gameId;
            gameOrder.UserId = userId;
            gameOrder.Number = Utils.GetOrderNumber();
            gameOrder.GameOrderStatus = (GameOrderStatus)gameResult;
            gameOrder.CreateTime = DateTime.Now;
            _db.GameOrders.Add(gameOrder);
            _db.SaveChanges();
            if (CurrentUser.Account.Vouchers < 10)
            {
                return Json(new { success = false, msg = "余额不足" });
            }

            AccountVouchersLog log = new AccountVouchersLog()
            {
                Account = CurrentUser.Account,
                AccountId = CurrentUser.Account.Id,
                Before = CurrentUser.Account.Vouchers,
                After = CurrentUser.Account.Vouchers - 10,
                CreateTime = DateTime.Now,
                Description = $"{CurrentUser.Name}下注比赛{game.ZhuChang}VS{game.KeChang},买{gameResult}",
                Vouchers = 10,
                Number = gameOrder.Number,
                DetailId = gameId,
                Type = AccountVouchersLogType.Billing,
            };
            _db.AccountVouchersLog.Add(log);
            CurrentUser.Account.Vouchers -= 10;
            _db.SaveChanges();
            return Json(new { success = true });
        }
    }
}