using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OpenGameList.Data;

namespace OpenGameList.Infrastructure
{
    public class JwtProvider
    {
        private readonly RequestDelegate _next;
        // JWT-related members
        private TimeSpan _tokenExpiration;
        private SigningCredentials _signingCredentials;
        // EF and Identity members
        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signManager;
        // Setup key value & Issuer
        private static readonly string _privateKey = "private_key_1234";

        public static readonly SymmetricSecurityKey SecurityKey =
            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_privateKey));
        public static readonly string Issuer = "OpenGameListWebApp";
        // 提供給 client 登入時候，呼叫的 POST url，在此處理登入資料
        public static string TokenEndPoint = "/api/connect/token";

        public JwtProvider(RequestDelegate next,
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _next = next;
            // Instantiate JWT-related members
            _tokenExpiration = TimeSpan.FromMinutes(10);
            _signingCredentials = new SigningCredentials(SecurityKey,
                SecurityAlgorithms.HmacSha256);
            _context = context;
            _userManager = userManager;
            _signManager = signInManager;
        }

        public Task Invoke(HttpContext httpContext)
        {
            // 檢查如果 request path 符合我們的 TokenEndPoint 才代表是正確的登入方式
            if (!httpContext.Request.Path.Equals(TokenEndPoint, StringComparison.Ordinal))
                return _next(httpContext);

            // 檢查如果 request 是 POST /x-www-form-urlencoded (代表輸入的內容)
            if (httpContext.Request.Method.Equals("POST")
                && httpContext.Request.HasFormContentType)
            {
                return CreateToken(httpContext);
            }
            else
            {
                httpContext.Response.StatusCode = 400;
                return httpContext.Response.WriteAsync("Bad Request!");
            }
        }

        private async Task CreateToken(HttpContext httpContext)
        {
            try {
                // retrieve the relevant FORM Data
                string username = httpContext.Request.Form["username"];
                string password = httpContext.Request.Form["password"];
                // check if there's an user with the given username
                var user = await _userManager.FindByNameAsync(username);
                // 處理使用者可能使用 email 登入
                if (user == null && username.Contains("@"))
                    user = await _userManager.FindByEmailAsync(username);

                var success = user != null && await _userManager.CheckPasswordAsync(user, password);
                if (success)
                {
                    DateTime now = DateTime.UtcNow;
                    // 建立 Claims 資料，参阅：https://tools.ietf.org/html/rfc7519#section-4
                    var claims = new[] 
                    {
                        new Claim(JwtRegisteredClaimNames.Iss, Issuer),
                        // 最重要的項目，此為這個 Claim 的 subject，也就是整串 Claims 的 Name
                        // 後續可以透過 ClaimTypes.NameIdentifier 取出此值，也就是取出使用者 ID
                        new Claim(JwtRegisteredClaimNames.Sub, user.Id),                    // (Subject) Claim
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),  // (JWT ID) Claim
                        new Claim(JwtRegisteredClaimNames.Iat,                              // (Issued At) Claim
                            new DateTimeOffset(now).ToUnixTimeSeconds().ToString(),
                            ClaimValueTypes.Integer64)
                        // TODO: 可以加入更多的 claims
                    };
                    // Create the JWT and write it to a string
                    var token = new JwtSecurityToken(
                        claims: claims, 
                        notBefore: now,
                        expires: now.Add(_tokenExpiration),
                        signingCredentials: _signingCredentials
                    );

                    var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);

                    // build the json Response
                    var jwt = new {
                        access_token = encodedToken,
                        expires_in = (int)_tokenExpiration.TotalSeconds
                    };
                    // return token
                    httpContext.Response.ContentType = "application/json";
                    await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(jwt));
                    return;
                }
            }
            catch (Exception ex)
            {
                // TODO: handle errors
                throw ex;
            }

            httpContext.Response.StatusCode = 400;
            await httpContext.Response.WriteAsync("Invalid username or password.");
        }
    }

    public static class JwtProviderExtensions
    {
        public static IApplicationBuilder UseJwtProvider(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JwtProvider>();
        }
    }
}