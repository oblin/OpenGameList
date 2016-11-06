﻿using AutoMapper;
using Microsoft.AspNetCore.Builder;
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
using OpenGameList.Infrastructure;
using Microsoft.IdentityModel.Tokens;

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
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            // Add EntityFramework's Identity support
            services.AddEntityFramework();
            // Add Identity services & stores
            services.AddIdentity<ApplicationUser, IdentityRole>(config => {
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
            app.UseJwtProvider();
            // Add the Jwt Bearer Header Authentication to validate Tokens
            app.UseJwtBearerAuthentication(new JwtBearerOptions(){
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                RequireHttpsMetadata = false,
                TokenValidationParameters = new TokenValidationParameters()
                {
                    // Basic settings - signing key to validate with, audience and issuer.
                    IssuerSigningKey = JwtProvider.SecurityKey,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = JwtProvider.Issuer,
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    // // When receiving a token, check that it is still valid.
                    ValidateLifetime = true,
                    // ClockSkew = TimeSpan.Zero
                }
            });

            app.UseMvc();

            // Add Item Mapping
            Mapper.Initialize(config => {
                config.CreateMap<Item, ItemViewModel>().ReverseMap();
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
    }
}
