using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShiJieBei.Models;
using ShiJieBeiComponents.Domains;
using ShiJieBeiComponents.Helpers;

namespace ShiJieBei.Controllers
{
    [Authorize(Roles = "user")]
    public class GameOrderController : ShiJieBeiController
    {
        /// <summary>
        /// 下注（生成订单）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CreateOrder(int gameId, int count, int gameResult)
        {
            int fee = 20*count;
            if (CurrentUser==null)
            {
                return Json(new { success = false, msg = "未登录" });
            }
            if (CurrentUser.Account.Vouchers < fee)
            {
                return Json(new { success = false, msg = "余额不足,微信联系客服mytokenera进行充值" });
            }
            int userId = CurrentUser.Id;
            var game = _db.Games.Find(gameId);
            var gameOrder = new GameOrders
            {
                GameCount = count,
                GameId = gameId,
                UserId = userId,
                Number = Utils.GetOrderNumber(),
                GameOrderStatus = (GameOrderStatus) gameResult,
                CreateTime = DateTime.Now
            };
            _db.GameOrders.Add(gameOrder);
            _db.SaveChanges();

            string resultStr = "";
            if (gameOrder.GameOrderStatus==GameOrderStatus.Win)
            {
                resultStr = "主胜";
            }
            else if (gameOrder.GameOrderStatus == GameOrderStatus.Ping )
            {
                resultStr = "平";
            }
            else if (gameOrder.GameOrderStatus == GameOrderStatus.Lose)
            {
                resultStr = "客胜";
            }
            AccountVouchersLog log = new AccountVouchersLog()
            {
                Account = CurrentUser.Account,
                AccountId = CurrentUser.Account.Id,
                Before = CurrentUser.Account.Vouchers,
                After = CurrentUser.Account.Vouchers - fee,
                CreateTime = DateTime.Now,
                Description = $"竞猜【{game.ZhuChang}】VS【{game.KeChang}】,{resultStr}{count}注,消耗{fee}积分",
                Vouchers = fee,
                Number = gameOrder.Number,
                DetailId = gameId,
                Type = AccountVouchersLogType.Pay,
            };
            _db.AccountVouchersLog.Add(log);
            CurrentUser.Account.Vouchers -= fee;
            _db.SaveChanges();
            return Json(new { success = true });
        }

        public ActionResult MyGameList()
        {

            var gameList = CurrentUser.GameOrders.ToList().Join(_db.Games, go => go.GameId, g => g.Id, (go, g) => new { CreateTime = go.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"), Number = go.Number, go.GameOrderStatus, g.KeChang, g.ZhuChang, StartTime = g.StartTime.ToString("yyyy-MM-dd HH:mm:ss") });

            List<MyGameListViewModel> myGameListViewModel = new List<MyGameListViewModel>();
            foreach (var item in gameList)
            {
                myGameListViewModel.Add(new MyGameListViewModel
                {
                    Number = item.Number,
                    CreateTime = item.CreateTime,
                    GameOrderStatus = item.GameOrderStatus,
                    KeChang = item.KeChang,
                    ZhuChang = item.ZhuChang,
                    StartTime = item.StartTime
                });
            }
            return View(myGameListViewModel);
        }
    }
}