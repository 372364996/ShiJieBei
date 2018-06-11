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
        public ActionResult SignUp(string email,string wallet, string pwd, string qrpwd, string returnUrl)
        {
            if (!pwd.Equals(qrpwd))
                return View((object)"密码不一致");
            User user = new User
            {
                Email = email,
                LastImgTime = DateTime.Now,
                CreateTime = DateTime.Now,
                Wallet = wallet,
                Account = new Account
                {
                    Money = 0,
                    MoneyLocked = 0,
                    
                }
            };
            string pwdHash = CryptoHelper.Md5(pwd);
            user.Password = pwdHash;
            _db.Users.Add(user);
            _db.SaveChanges();
            SetAuthCookie(user);
            return Redirect("/game");
        }
        [HttpPost]
        public ActionResult Login(string name, string pwd, string returnUrl)
        {
            string pwdHash = CryptoHelper.Md5(pwd);
            var user = _db.Users.FirstOrDefault(m => m.Name == name && m.Password == pwdHash);
            if (user != null)
            {
                SetAuthCookie(user);

                return Redirect("/game");
            }

            return View((object)"用户名或密码不正确");
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