using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SAFETYService;
using SAFETYModel.DBModels;
using SAFETY.Middleware;

namespace SAFETY
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
            // DB Connection
            var connection = Configuration.GetConnectionString("DefaultConnectionString");
            services.AddDbContext<SAFETYContext>(options => options.UseSqlServer(connection));

            //  權限
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(option =>
                {
                    option.LoginPath = "/Login/Index";//登入頁
                    option.AccessDeniedPath = "/Login/Index";//登入頁
                    option.Cookie.SecurePolicy = CookieSecurePolicy.None;

                    //  用戶頁面停留太久，登入逾期
                    option.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                    //  未授權時跳轉，mvc頁面導向登入頁，api則回傳401
                    option.Events = new CookieAuthenticationEvents()
                    {
                        OnRedirectToLogin = (ctx) =>
                        {
                            // API權限不足時，回傳狀態碼: 401
                            if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == 200)
                            {
                                ctx.Response.StatusCode = 401;
                            }
                            else
                                ctx.Response.Redirect(ctx.RedirectUri);

                            return Task.CompletedTask;
                        },
                        OnRedirectToAccessDenied = (ctx) =>
                        {
                            if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == 200)
                            {
                                ctx.Response.StatusCode = 403;
                            }
                            else
                            {
                                ctx.Response.Redirect(ctx.RedirectUri);
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            //  Cookie
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // 將 Session 存在 ASP.NET Core 記憶體中
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                //options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                //options.Cookie.Name = "mywebsite";
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });

            //services.AddControllersWithViews()
            //        .AddNewtonsoftJson()
            //        .AddRazorRuntimeCompilation()
            //        .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);

            //return Data 維持大小寫
            services.AddControllersWithViews()
             .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null)
             .AddRazorRuntimeCompilation()
             .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
             .AddDataAnnotationsLocalization(options => {
                 options.DataAnnotationLocalizerProvider = (type, factory) =>
                     factory.Create(typeof(Resources.SharedResources));
             });

            services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });

            // 中文變亂碼解決方式
            services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));

            //  注入Service
            services.AddScoped<CommonService>();
            services.AddScoped<FileService>();
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            //  先驗證
            app.UseAuthentication();
            //  後授權
            app.UseAuthorization();
            // 多語系數設定
            var supportedCultures = new List<CultureInfo>()
            {
                new CultureInfo("zh-TW"),
                new CultureInfo("en"),
            };
            app.UseRequestLocalization(new RequestLocalizationOptions()
            {
                DefaultRequestCulture = new RequestCulture("zh-TW"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures,
            });
            // SessionMiddleware 加入 Pipeline
            app.UseSession();
            app.Use(async (context, next) =>
            {
                context.Session.SetString("SessionKey", "SessoinValue");
                await next.Invoke();
            });
            // 例外攔截
            app.UseExceptionHandleMiddleware();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=Index}/{id?}");
            });
        }
    }
}
