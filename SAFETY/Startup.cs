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

            //  �v��
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(option =>
                {
                    option.LoginPath = "/Login/Index";//�n�J��
                    option.AccessDeniedPath = "/Login/Index";//�n�J��
                    option.Cookie.SecurePolicy = CookieSecurePolicy.None;

                    //  �Τ᭶�����d�Ӥ[�A�n�J�O��
                    option.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                    //  �����v�ɸ���Amvc�����ɦV�n�J���Aapi�h�^��401
                    option.Events = new CookieAuthenticationEvents()
                    {
                        OnRedirectToLogin = (ctx) =>
                        {
                            // API�v�������ɡA�^�Ǫ��A�X: 401
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

            // �N Session �s�b ASP.NET Core �O���餤
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

            //return Data �����j�p�g
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

            // �����ܶýX�ѨM�覡
            services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));

            //  �`�JService
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

            //  ������
            app.UseAuthentication();
            //  ����v
            app.UseAuthorization();
            // �h�y�t�Ƴ]�w
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
            // SessionMiddleware �[�J Pipeline
            app.UseSession();
            app.Use(async (context, next) =>
            {
                context.Session.SetString("SessionKey", "SessoinValue");
                await next.Invoke();
            });
            // �ҥ~�d�I
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
