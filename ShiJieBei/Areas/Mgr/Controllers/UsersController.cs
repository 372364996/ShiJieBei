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
    public class UsersController : MgrController
    {
        private ShiJieBeiDbContext db = new ShiJieBeiDbContext();

        // GET: Mgr/Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Mgr/Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Mgr/Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Mgr/Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserGuid,Name,Password,Email,Wallet,RetrievePassWordCode,Sex,Country,Province,City,HeadImg,HeadImgHash,CreateTime,LastImgTime,Mobile,Token,IsEmailValid,IsGiveVouchers,DescName,DescHeadImg,TrueName,WeiChat,PartnerId")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Mgr/Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Mgr/Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserGuid,Name,Password,Email,Wallet,RetrievePassWordCode,Sex,Country,Province,City,HeadImg,HeadImgHash,CreateTime,LastImgTime,Mobile,Token,IsEmailValid,IsGiveVouchers,DescName,DescHeadImg,TrueName,WeiChat,PartnerId")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Mgr/Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Mgr/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Charge(int id) {
           var user= db.Users.FirstOrDefault(u => u.Id == id);
            if (user==null)
            {
                return Content("用户不存在");
            }
            return View(user);
        }
        /// <summary>
        /// 管理员给用户进行充值
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Charges( decimal vouchers, int userid)
        {
            var user = db.Users.FirstOrDefault(u=>u.Id==userid);
            try
            {
                Charge(user, Utils.GetChargeNumber(user.Id), vouchers, "管理员：" + CurrentManager.Name + "给用户:" + user.Name + "充值" + vouchers + "个点数", CurrentManager.Id);
                return Json(new { success = true,msg="充值成功" });
            }
            catch (Exception exp)
            {
                logger.Error("给用户：" + userid + "充值金额失败:", exp);
                return Json(new { success = false,msg="充值失败" });

            }
        }
        private void Charge(User user, string number, decimal vouchers, string description, int gameId)
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
                Type = AccountVouchersLogType.Charge,
                DetailId = gameId
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
