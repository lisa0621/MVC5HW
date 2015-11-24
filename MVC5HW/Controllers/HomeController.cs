using MVC5HW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MVC5HW.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //取得目前登入者的帳號
            string Account = User.Identity.Name;
            //如果要判斷此登入者有沒有登入的寫法
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                //目前已登入 
            }
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

        /// <summary>
        /// 呈現後台使用者登入頁
        /// </summary>
        /// <param name="ReturnUrl">使用者原本Request的Url</param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Login(string ReturnUrl)
        {
            //ReturnUrl字串是使用者在未登入情況下要求的的Url
            LoginVM vm = new LoginVM() { ReturnUrl = ReturnUrl };
            return View(vm);
        }

        /// <summary>
        /// 後台使用者進行登入
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="u">使用者原本Request的Url</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginVM vm)
        {
            //沒通過Model驗證(必填欄位沒填，DB無此帳密)
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            
            //都成功...
            //進行表單登入 ※之後User.Identity.Name的值就是vm.Account帳號的值
            //導向預設Url(Web.config裡的defaultUrl定義)或使用者原先Request的Url
            FormsAuthentication.RedirectFromLoginPage(vm.帳號, false);

            //剛剛已導向，此行不會執行到
            return Redirect(FormsAuthentication.GetRedirectUrl(vm.帳號, false));
        }

        /// <summary>
        /// 後台使用者登出動作
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Logout()
        {
            //清除Session中的資料
            Session.Abandon();
            //登出表單驗證
            FormsAuthentication.SignOut();
            //導至登入頁
            return RedirectToAction("Login", "Home");
        }

    }
}