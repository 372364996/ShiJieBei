using ShiJieBeiComponents.Domains;
using ShiJieBeiComponents.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ShiJieBeiComponents.Repositories.EF;

namespace ShiJieBei.Controllers
{
    public class HomeController : Controller
    {
        private readonly ShiJieBeiDbContext _db = new ShiJieBeiDbContext();
        [Authorize(Roles = "user")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult SignUp()
        {

            return View();
        }
        [HttpPost]
        public ActionResult SignUp(string email, string wallet, string pwd, string qrpwd, string returnUrl)
        {
            if (!pwd.Equals(qrpwd))
                return Content("<script>alert('密码不一致');window.location.href='/home/signup';</script>");
           
            var data = _db.Users.FirstOrDefault(m => m.Email == email);
            if (data!=null)
                return Content("<script>alert('邮箱已被注册');window.location.href='/home/signup';</script>");
            User user = new User
            {
                Name = $"User-{Utils.GetRandomString()}",
                Email = email,
                LastImgTime = DateTime.Now,
                CreateTime = DateTime.Now,
                Wallet = wallet,
                Token = Guid.NewGuid().ToString(),
                Account = new Account
                {
                    Money = 0,
                    MoneyLocked = 0,
                    Vouchers = 10000
                }
            };
            string pwdHash = CryptoHelper.Md5(pwd);
            user.Password = pwdHash;
            _db.Users.Add(user);
            _db.SaveChanges();
            SetAuthCookie(user);
            string tokenUrl = $"http://www.tokenbwin.com/home/validemail?token={user.Token}";
            string msg = $"点击下列链接 <a href='{tokenUrl}'>激活邮箱</a>";
            Utils.SendEmail("激活邮箱", email, msg);
            return RedirectToAction("SendEmail");
        }

        public ActionResult SendEmail()
        {
            return View();
        }

        public ActionResult ValidEmail(string token)
        {
            var data = _db.Users.FirstOrDefault(u => u.Token == token);
            if (data == null)
            {
                return Redirect("/home/login");
            }
            data.IsEmailValid = true;
            _db.SaveChanges();
            return Redirect("/game");
        }

        [HttpPost]
        public ActionResult Login(string email, string pwd, string returnUrl)
        {
            string pwdHash = CryptoHelper.Md5(pwd);
            var user = _db.Users.FirstOrDefault(m => m.Email == email && m.Password == pwdHash);
            if (user != null)
            {
                if (!user.IsEmailValid)
                {
                    return Content("<script>alert('账号未激活');window.location.href='/home/login';</script>");
                }
                SetAuthCookie(user);

                return Redirect("/game");
            }

            return  Content("<script>alert('邮箱或密码不正确');window.location.href='/home/login'</script>");
        }
        public ActionResult RetrievePassword() {
            return View();
        }
        [HttpPost]
        public ActionResult RetrievePassword(string email)
        {
           

            var user = _db.Users.FirstOrDefault(m => m.Email == email);
            if (user == null)
                return Content("<script>alert('邮箱未注册');window.location.href='/home/signup';</script>");
            user.RetrievePassWordCode = Utils.GetRandomString();
            _db.SaveChanges();
            string tokenUrl = $"http://www.tokenbwin.com/home/resetpassword?code={user.RetrievePassWordCode}";
            string msg = $"点击下列链接 <a href='{tokenUrl}'>重置密码</a>";
            Utils.SendEmail("重置密码",email, msg);
            return RedirectToAction("SendEmail");
        }
        public ActionResult ResetPassword(string code) {
            if (string.IsNullOrEmpty(code))
            {
                return RedirectToAction("Login");
            }
            var user = _db.Users.FirstOrDefault(u=>u.RetrievePassWordCode==code);
            if (user==null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }
        private void SetAuthCookie(User user)
        {
            //建立身份验证票对象
            // var mgr = UserService.LoadManager(user.Id);
            string roles = "user";
            //if (mgr != null)
            //{
            //    roles += ",mgr";
            //}
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, user.Id.ToString(), Utils.ToLocalTime(DateTime.UtcNow), Utils.ToLocalTime(DateTime.UtcNow.AddDays(7)), false, roles, "/");
            //加密序列化验证票为字符串
            string hashTicket = FormsAuthentication.Encrypt(ticket);
            HttpCookie userCookie = new HttpCookie(FormsAuthentication.FormsCookieName, hashTicket);
            //userCookie.Path = "/";
            Response.Cookies.Add(userCookie);
        }
    }
}