using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenGameList.Data;
using OpenGameList.ViewModels;
using System;
using System.IO;
using Microsoft.IdentityModel.Tokens;
using OpenGameListWebApp.ViewModels;

namespace OpenGameList
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            Protector = CreateDataProtector();
        }

        public IConfigurationRoot Configuration { get; }
        public IDataProtector Protector { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add a reference to Configuration object for Disabled
            services.AddSingleton<Microsoft.Extensions.Configuration.IConfiguration>(c => { return Configuration; });
            // Add framework services.
            services.AddMvc();
            // Add localhost CORS support
            services.AddCors(options => 
                options.AddPolicy("AllowLocalhost",
                    builder => builder.WithOrigins("http://localhost")
                        .AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));

            // Add EntityFramework's Identity support
            services.AddEntityFramework();
            // Add Identity services & stores
            services.AddIdentity<ApplicationUser, IdentityRole>(config =>
                {
                    config.User.RequireUniqueEmail = true;
                    config.Password.RequireDigit = true;
                    config.Password.RequireNonAlphanumeric = false;
                    config.Cookies.ApplicationCookie.AutomaticChallenge = false;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            // Add ApplicationDbContext
            string connectionString = Configuration["Data:DefaultConnection:ConnectionString"];
            services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

            // Register the OpenIddict services, including the default EF stores
            services.AddOpenIddict<ApplicationDbContext>()
                // Register the ASP.NET Core MVC binder used by OpenIddict, bind OpenIdConnectRequest or OpenIdConnectResponse parameters
                .AddMvcBinders()
                .UseJsonWebTokens()
                // Set a custom token endpoint (default is /connect/token)
                .EnableTokenEndpoint(Configuration["Authentication:OpenIddict:TokenEndPoint"])
                // Set a custom auth endpoint (default is /connect/authorize)
                .EnableAuthorizationEndpoint(Configuration["Authentication:OpenIddict:AuthorizationEndPoint"])
                // Allow client applications to use the grant_type=password flow.
                .AllowPasswordFlow()
                // Enable support for both authorization & implicit flows
                .AllowAuthorizationCodeFlow().AllowImplicitFlow()
                // Allow the client to refresh Tokens
                .AllowRefreshTokenFlow()
                // 設定 Token 失效時間
                .SetAccessTokenLifetime(TimeSpan.FromMinutes(15))
                //  Disable the HTTPS requirement (not recommended in production)
                .DisableHttpsRequirement()
                // Register a new ephemeral key for development.
                // We will register a X.509 certificate in production.
                .AddEphemeralSigningKey();

            // Add application's seed Data
            services.AddSingleton<DbSeeder>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            ILoggerFactory loggerFactory, DbSeeder dbSeeder)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            // Deep link，讓 SPA 可以記住 url 提供正確的 routing ，注意必須要放在 use static files 前面，否則會先執行 static files 取出的動作
            app.Use(async (context, next) =>
            {
                await next();

                if (context.Response.StatusCode == 404 && !Path.HasExtension(context.Request.Path.Value))
                {
                    context.Request.Path = "/";
                    context.Response.StatusCode = 200;
                    await next();
                }
            });

            app.UseDefaultFiles();

            app.UseStaticFiles(new StaticFileOptions()
            {
                OnPrepareResponse = (context) =>
                {
                    // Disabled caching for all static files
                    context.Context.Response.Headers["Cache-Control"] =
                        Configuration["StaticFiles:Headers:Cache-Control"];
                    context.Context.Response.Headers["Pragma"] =
                        Configuration["StaticFiles:Headers:Pragma"];
                    context.Context.Response.Headers["Expires"] =
                        Configuration["StaticFiles:Headers:Expires"];
                }
            });

            // Add a custom Jwt Provider to generate Tokens
            // app.UseJwtProvider();

            // Add the AspNetCore.Identity middleware (required for external auth providers)
            // IMPORTANT: 必須要放在 OpenIddict 或任何其他的外部 providers 之前
            app.UseIdentity();

            // 外部 Facebook 驗證
            app.UseFacebookAuthentication(new FacebookOptions()
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                AppId = Protector.Unprotect(Configuration["Authentication:Facebook:AppId"]),
                AppSecret = Protector.Unprotect(Configuration["Authentication:Facebook:AppSecret"]),
                CallbackPath = "/signin-facebook",
                Scope = { "email" }
            });

            app.UseGoogleAuthentication(new GoogleOptions()
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                ClientId = Protector.Unprotect(Configuration["Authentication:Google:ClientId"]),
                ClientSecret = Protector.Unprotect(Configuration["Authentication:Google:ClientSecret"]),
                CallbackPath = "/signin-google",
                Scope = { "email" }
            });

            // Add OpenIddict middleware, Must registered after app.UseIdentity() and the external social providers
            app.UseOpenIddict();

            // Add the Jwt Bearer Header Authentication to validate Tokens
            app.UseJwtBearerAuthentication(new JwtBearerOptions()
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                RequireHttpsMetadata = false,
                //  specify an explicit  Authority  property value to allow 
                // the JWT bearer middleware to download the signing key.
                Authority = Configuration["Authentication:OpenIddict:Authority"],
                TokenValidationParameters = new TokenValidationParameters()
                {
                    // Basic settings - signing key to validate with, audience and issuer.
                    // IssuerSigningKey = JwtProvider.SecurityKey,
                    // ValidateIssuerSigningKey = true,
                    // ValidIssuer = JwtProvider.Issuer,
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    // ValidateLifetime = true,
                    // ClockSkew = TimeSpan.Zero
                }
            });

            app.UseMvc();

            // Add Item Mapping
            Mapper.Initialize(config =>
            {
                config.CreateMap<Item, ItemViewModel>().ReverseMap();
                config.CreateMap<ApplicationUser, UserViewModel>();
                // 以下設定如果可以在前端設定比較容易理解，因此先移除
                // config.CreateMap<ItemViewModel, Item>()
                //       .ForMember(dest => dest.ViewCount, opt => opt.Condition(src => (src.ViewCount > 0)));
            });

            // Execute data seeding
            try
            {
                dbSeeder.SeedAsync().Wait();
            }
            catch (AggregateException e)
            {
                throw new Exception(e.ToString());
            }
        }

        private IDataProtector CreateDataProtector()
        {
            string destFolder = Path.Combine(
                Environment.GetEnvironmentVariable("LOCALAPPDATA"),
                "AppSecrets");
            var dataProtectionProvider = DataProtectionProvider.Create(
                new DirectoryInfo(destFolder),
                configuration =>
                {
                    configuration.SetApplicationName("SecretsManager");
                    configuration.ProtectKeysWithDpapi();
                }
            );
            return dataProtectionProvider.CreateProtector("General.Protection");
        }
    }
}
