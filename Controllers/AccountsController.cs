using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OpenGameList.Data;

namespace OpenGameList.Controllers
{
    public class AccountsController : BaseController
    {
        public AccountsController (ApplicationDbContext context,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager) 
        : base(context, signInManager, userManager) { }

        [HttpGet("ExternalLogin/{provider}")]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            switch(provider.ToLower())
            {
                case "facebook":
                case "google":
                    // Request a redirect to the external login provider 的登入頁面
                    var redirectUrl = Url.Action("ExternalLoginCallback",
                        "Accounts", new { ReturnUrl = returnUrl });
                    var properties = _signInManager.ConfigureExternalAuthenticationProperties(
                        provider, redirectUrl);
                    return Challenge(properties, provider);
                default:
                    return BadRequest(new { Error = $"Provider {provider} is not supported."});
            }
        }
        
        [HttpGet("ExternalLoginCallback")]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null,
            string remoteError = null)
        {
            try 
            {
                if (remoteError != null)
                    throw new Exception(remoteError);
                // 由外部的 provider 解開 login information
                ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                    throw new Exception("Error: No login info availabe");

                // 檢查是否使用者已經登入到外部的 provider
                var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
                if (user == null)
                {
                    // 這裡代表使用者並未使用此外部的帳號註冊過，但仍然需要檢查是否有用 email address 註冊
                    // 因為有可能利用此 email 從其他的外部登入伺服器，或者 local account
                    // Retrieve the 'email address' claim
                    var emailKey = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";
                    var email = info.Principal.FindFirst(emailKey).Value;

                    // 檢查資料庫是否有符合這個 email 的使用者
                    user = await _userManager.FindByEmailAsync(email);
                    if (user == null)
                    {
                        // 代表使用者從未註冊過，因此將此使用者註冊到 local db
                        DateTime now = DateTime.Now;
                        // 使用外部登入伺服器的 nameidentifier claim 建立 unique username
                        var idKey = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
                        var username = $"{info.LoginProvider}{info.Principal.FindFirst(idKey).Value}";

                        user = new ApplicationUser 
                        {
                            UserName = username,
                            Email = email,
                            CreatedDate = now,
                            LastModifiedDate = now
                        };

                        // 將此使用者加入到資料庫，並且使用 random password (因為此使用者應該只會從外部登入)
                        await _userManager.CreateAsync(user, "Pass4External");
                        await _userManager.AddToRoleAsync(user, "Registered");

                        // Remove Lockout and E-mail confirmation
                        user.EmailConfirmed = true;
                        user.LockoutEnabled = false;
                    }
                    // 使用者已經註冊過了建立登入資料
                    await _userManager.AddLoginAsync(user, info);
                    await _context.SaveChangesAsync();
                }
                // 這裡代表內部資料庫已經有此使用者建立，可以開始處理登入作業：
                // 1. 首先建立 auth JSON object 傳給外部登入伺服器驗證
                var auth = new {
                    type = "External",
                    providerName = info.LoginProvider
                };
                // 2. 產生 <script> tag 呼叫 javascript function registered into the parent window global scope
                //    這個會自動開啟新的瀏覽器，導入到外部的登入伺服器讓使用者輸入登入資訊
                return Content($@"
                    <script type='text/javascript'>
                        window.opener.externalProviderLogin('{JsonConvert.SerializeObject(auth)}');
                        window.close();
                    </script>
                ", "text/html");
                // 3. 使用者輸入登入資料後，就會自動 close window，後續則是由外部登入服務依據我們註冊的 callback address 傳入登入是否成功的資訊
            }
            catch (Exception ex)
            {
                // return a HTTP status 400 to the client
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            if (User.Identity.IsAuthenticated)
                _signInManager.SignOutAsync().Wait();
            return Ok();
        }
    }
}