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
            return RedirectToAction("GoIndex", "Game");
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
        //        public ActionResult InputTestData()
        //        {
        //            string[] names = {
        //"飞仔",
        //"0x12",
        //"林晓轩",
        //"xinyuan ",
        //"宋奇东  " ,
        //"xiaodu  ",
        //"hhhh    ",
        //"MT哥    " ,
        //"xiaoxi  ",
        //"a10168  ",
        //"无花    " ,
        //"席博豪  " ,
        //"自由    " ,
        //"黑天使  " ,
        //"风行    " ,
        //"水瓶哥  " ,
        //"天命    " ,
        //"孤魂炫  " ,
        //"天麒    " ,
        //"飞飞    " ,
        //"啊酷    " ,
        //"千百度  " ,
        //"午夜魂  " ,
        //"小迪    ",
        //"一半人的记忆",
        //"54321      ",
        //"caihao     ",
        //"浮生若梦    ",
        //"w408183907 ",
        //"vblock     ",
        //"流氓小子    ",
        //"蚂蚁牙黑    ",
        //"说好要嫁给你",
        //"ry-sj蚂蚁牙黑",
        //"SuperSyt    ",
        //"黑客武林     ",
        //"Hacker皇帝   ",
        //"旧梦         ",
        //"kingsue     ",
        //"143980892   ",
        //"小新         ",
        //"李小四       ",
        //"风吹过       ",
        //"蓦然回首     ",
        //"kill        ",
        //"tom         ",
        //"SATTTTTT    ",
        //"2981AA      ",
        //"那年南山     ",
        //"采菊客       "
        // };
        //            foreach (var item in names)
        //            {
        //                User user = new User
        //                {
        //                    Name = item.Trim(),
        //                    Email = $"{Utils.GetRandomString()}@163.com",
        //                    LastImgTime = DateTime.Now,
        //                    CreateTime = DateTime.Now,
        //                    HeadImg = $"{new Random().Next(1, 42)}.png",
        //                    Wallet = $"0x{Guid.NewGuid().ToString("N")}",
        //                    Token = Guid.NewGuid().ToString(),
        //                    Account = new Account
        //                    {
        //                        Money = 0,
        //                        MoneyLocked = 0,
        //                        Vouchers = 50
        //                    }
        //                };
        //                string pwdHash = CryptoHelper.Md5("123456");
        //                user.Password = pwdHash;
        //                _db.Users.Add(user);
        //                _db.SaveChanges();
        //            }

        //            return Content("导入成功");
        //        }
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
            if (data != null)
                return Content("<script>alert('邮箱已被注册');window.location.href='/home/signup';</script>");
            User user = new User
            {
                Name = $"{Utils.GetRandomString()}",
                Email = email,
                LastImgTime = DateTime.Now,
                CreateTime = DateTime.Now,
                HeadImg = $"{new Random().Next(1, 42)}.png",
                Wallet = wallet,
                Token = Guid.NewGuid().ToString(),
                Account = new Account
                {
                    Money = 0,
                    MoneyLocked = 0,
                    Vouchers = 50
                }
            };
            string pwdHash = CryptoHelper.Md5(pwd);
            user.Password = pwdHash;
            _db.Users.Add(user);
            _db.SaveChanges();
            //SetAuthCookie(user);
            string tokenUrl = $"http://www.tokenbwin.com/home/validemail?token={user.Token}";
            //string msg = $"点击下列链接 <a href='{tokenUrl}'>激活邮箱</a>";
            string msg = $"尊敬的{email}您好:<br />欢迎您注册tokenbwin竞猜平台。<br/>请访问以下链接完成您的邮箱激活验证。<a href = '{tokenUrl}' > 激活邮箱 </a><br />{tokenUrl}<br />如果无法打开链接，请复制上面的链接粘贴到浏览器的地址栏。";
            Utils.SendEmailByCdo("tokenbwin-激活邮件", email, msg);
            return RedirectToAction("SendEmail");
        }
        //public ActionResult InputTestOrders()
        //{
        //    foreach (var item in _db.Games)
        //    {
        //        for (int i = 0; i < 19; i++)
        //        {


        //            var randomUserId = new Random().Next(1, 50);
        //            var count = 1;
        //            var gameResult = new Random().Next(0, 2);
        //            var user = _db.Users.Where(u => u.Id == randomUserId).FirstOrDefault();
        //            int fee = 20 * count;

        //            int userId = user.Id;
        //            var game = _db.Games.Find(item.Id);
        //            var gameOrder = new GameOrders
        //            {
        //                GameCount = count,
        //                GameId = item.Id,
        //                UserId = userId,
        //                Number = Utils.GetOrderNumber(),
        //                GameOrderStatus = (GameOrderStatus)gameResult,
        //                CreateTime = DateTime.Now
        //            };
        //            _db.GameOrders.Add(gameOrder);

        //            string resultStr = "";
        //            if (gameOrder.GameOrderStatus == GameOrderStatus.Win)
        //            {
        //                resultStr = "主胜";
        //            }
        //            else if (gameOrder.GameOrderStatus == GameOrderStatus.Ping)
        //            {
        //                resultStr = "平";
        //            }
        //            else if (gameOrder.GameOrderStatus == GameOrderStatus.Lose)
        //            {
        //                resultStr = "客胜";
        //            }
        //            AccountVouchersLog log = new AccountVouchersLog()
        //            {
        //                Account = user.Account,
        //                AccountId = user.Account.Id,
        //                Before = user.Account.Vouchers,
        //                After = user.Account.Vouchers - fee,
        //                CreateTime = DateTime.Now,
        //                Description = $"竞猜【{game.ZhuChang}】VS【{game.KeChang}】,{resultStr}{count}注,消耗{fee}积分",
        //                Vouchers = fee,
        //                Number = gameOrder.Number,
        //                DetailId = item.Id,
        //                Type = AccountVouchersLogType.Pay,
        //            };
        //            _db.AccountVouchersLog.Add(log);
        //            user.Account.Vouchers -= fee;
        //        }

        //    }
        //    _db.SaveChanges();
        //    return Content("生成订单成功");
        //}
        public ActionResult UpdateOrderTime()
        {

            using (var db = new ShiJieBeiDbContext())
            {
                // 批量更新
                var entitys = db.GameOrders;
                entitys.ToList().ForEach(item =>
                {
                    Random random = new Random();
                    int day = random.Next(14, 20);
                    int hour = random.Next(0, 24);
                    int minute = random.Next(0, 60);
                    int second = random.Next(0, 60);
                    string tempStr =
                        $"{DateTime.Now:yyyy}-{DateTime.Now:MM}-{day} {hour}:{minute}:{second}";
                    DateTime rTime = Convert.ToDateTime(tempStr);
                    item.CreateTime = rTime;
                    db.Entry(item).State = System.Data.Entity.EntityState.Modified; //不加这句也可以，为什么？
                   
                });
                db.SaveChanges();
            }
            //foreach (var item in _db.GameOrders)
            //{
            //    Random random = new Random();
            //    int day = random.Next(14, 20);
            //    int hour = random.Next(0, 24);
            //    int minute = random.Next(0, 60);
            //    int second = random.Next(0, 60);
            //    string tempStr = string.Format("{0}-{1}-{2} {3}:{4}:{5}", DateTime.Now.ToString("yyyy"),DateTime.Now.ToString("MM"), day, hour, minute, second);
            //    DateTime rTime = Convert.ToDateTime(tempStr);
            //    item.CreateTime = rTime;
            //    _db.Entry(item).State = System.Data.Entity.EntityState.Modified;
            //}
            //_db.SaveChanges();
            return Content($"{DateTime.Now}更新时间成功");
        }
        public ActionResult SendEmail()
        {
            return View();
        }

        public ActionResult ValidEmail(string token)
        {
            var user = _db.Users.FirstOrDefault(u => u.Token == token);
            if (user == null)
            {
                return Redirect("/home/login");
            }
            user.IsEmailValid = true;
            _db.SaveChanges();
            SetAuthCookie(user);
            return Redirect("/game/goindex");
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

                return Redirect("/game/goindex");
            }

            return Content("<script>alert('邮箱或密码不正确');window.location.href='/home/login'</script>");
        }
        public ActionResult RetrievePassword()
        {
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
            string msg = $"尊敬的{email}您好:<br />欢迎您注册tokenbwin竞猜平台。<br/>请访问以下链接完成<a href = '{tokenUrl}' > 重置密码 </a><br />{tokenUrl}<br />如果无法打开链接，请复制上面的链接粘贴到浏览器的地址栏。";
            Utils.SendEmailByCdo("tokenbwin-重置密码", email, msg);
            return RedirectToAction("SendEmail");
        }

        public ActionResult ResetPassword(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return RedirectToAction("Login");
            }
            var user = _db.Users.FirstOrDefault(u => u.RetrievePassWordCode == code);
            if (user == null)
            {
                return RedirectToAction("Login");
            }
            return View(user);
        }
        [HttpPost]
        public ActionResult ResetPassword(string code, string pwd, string qrpwd)
        {
            if (string.IsNullOrEmpty(code))
            {
                return RedirectToAction("Login");
            }
            if (!pwd.Equals(qrpwd))
            {
                return Content($"<script>alert('两次密码输入不一致，请重新输入');window.location.href='/home/resetpassword?code={code}';</script>");
            }
            var user = _db.Users.FirstOrDefault(u => u.RetrievePassWordCode == code);
            if (user == null)
            {
                return RedirectToAction("Login");
            }
            user.Password = CryptoHelper.Md5(pwd);
            _db.SaveChanges();
            return RedirectToAction("Login");
        }
        private void SetAuthCookie(User user)
        {
            //建立身份验证票对象
            var mgr = _db.Managers.Where(u => u.UserId == user.Id);
            string roles = "user";
            if (mgr != null)
            {
                roles += ",mgr";
            }
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, user.Id.ToString(), Utils.ToLocalTime(DateTime.UtcNow), Utils.ToLocalTime(DateTime.UtcNow.AddDays(7)), false, roles, "/");
            //加密序列化验证票为字符串
            string hashTicket = FormsAuthentication.Encrypt(ticket);
            HttpCookie userCookie = new HttpCookie(FormsAuthentication.FormsCookieName, hashTicket);
            //userCookie.Path = "/";
            Response.Cookies.Add(userCookie);
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("/home/login");
        }
    }
}