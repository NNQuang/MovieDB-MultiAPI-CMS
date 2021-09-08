using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using ClientService.Helpers.AutoMapper;
using ClientService.Helpers.Image;
using ClientService.Helpers.Security;
using System;
using System.Net.Http.Headers;
using ClientService.Helpers.Comment;
using ClientService.Helpers;

namespace ClientService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = tokenOptions.Issuer,
                ValidAudience = tokenOptions.Audience,
                IssuerSigningKey = HashHelper.CreateHash(tokenOptions.SecurityKey)
            });
            services.AddControllersWithViews();
            services.AddAutoMapper(typeof(ActorProfile), typeof(DirectorProfile), typeof(GenreProfile), typeof(MovieProfile));
            services.AddSession();
            services.AddScoped<IImageHelper, ImageHelper>();
            services.AddScoped<ICommentHelper, CommentHelper>();
            services.AddHttpClient("movie", m =>
            {
                m.BaseAddress = new Uri(Configuration.GetValue<string>("GatewayIp"));
                m.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient("user", u =>
            {
                u.BaseAddress = new Uri(Configuration.GetValue<string>("GatewayIp"));
                u.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseExceptionHandler("/Home/Error");
            }
            app.UseStatusCodePagesWithReExecute("/Error/{0}");
            app.UseCookiePolicy();
            app.UseSession();
            // Token'ı sessiona ekliyoruz.
            app.Use(async (context, next) =>
            {
                var token = context.Session.GetString("JWToken");
                if (!string.IsNullOrEmpty(token))
                {
                    context.Request.Headers.Add("Authorization", "Bearer " + token);
                }
                await next();
            });

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                    name: "Admin",
                    areaName: "Admin",
                    pattern: "Admin/{controller=Home}/{action=Index}");


                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
