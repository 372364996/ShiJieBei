using ShiJieBeiComponents.Helpers;
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
        public ActionResult ResetPasswordByCenter()
        {


            return View(CurrentUser);
        }
        [HttpPost]
        public ActionResult ResetPasswordByCenter(string oldpwd, string pwd, string qrpwd)
        {
            if (!CurrentUser.Password.Equals(CryptoHelper.Md5(oldpwd)))
            {
                return Content($"<script>alert('旧密码不正确，请重新输入');window.location.href='/my/resetpasswordbycenter';</script>");
            }
            if (!pwd.Equals(qrpwd))
            {
                return Content($"<script>alert('两次密码输入不一致，请重新输入');window.location.href='/my/resetpasswordbycenter';</script>");
            }
            var user = _db.Users.FirstOrDefault(u => u.Id == CurrentUser.Id);
            if (user == null)
            {
                return RedirectToAction("Login");
            }
            user.Password = CryptoHelper.Md5(pwd);
            _db.SaveChanges();
            return RedirectToAction("Index","My");
        }
    }
}