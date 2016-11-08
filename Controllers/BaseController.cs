using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OpenGameList.Data;

namespace OpenGameList.Controllers
{
    [Route("api/[controller]")]
    public class BaseController : Controller
    {
        protected ApplicationDbContext _context; 
        protected SignInManager<ApplicationUser> _signInManager;
        protected UserManager<ApplicationUser> _userManager;

        public BaseController (ApplicationDbContext context,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        /// <summary>
        /// 取出登入使用者的 ID (從外部 Auth or 內部的 Auth)
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetCurrentUserId()
        {
            // if the user is not authenticated, throw an exception
            if (!User.Identity.IsAuthenticated)
                throw new NotSupportedException();

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null) {
                // internal provider
                return User.FindFirst(ClaimTypes.NameIdentifier).Value;
            } else {
                // external provider
                var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
                if (user == null) throw new NotSupportedException();
                return user.Id;
            }
        }

        protected JsonSerializerSettings DefaultJsonSettings => 
            new JsonSerializerSettings {
                Formatting = Formatting.Indented
            };        
    }
}