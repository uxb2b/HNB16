using CommonLib.Core.Utility;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using WebHome.Controllers.Filters;
using WebHome.Properties;

namespace WebHome;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        //builder.Services.AddControllersWithViews()
        //    .AddRazorRuntimeCompilation();

        builder.Services.AddMemoryCache();

        // 註冊 CORS
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowMyClient", policy =>
            {
                policy.WithOrigins(AppSettings.Default.AllowCORS) // 允許的來源
                      .AllowAnyHeader()                  // 允許所有 headers
                      .AllowAnyMethod();                 // 允許 GET, POST, PUT, DELETE
            });

            // 如果要允許全部來源 (僅限測試用，不建議正式環境)
            //options.AddPolicy("AllowAll", policy =>
            //{
            //    policy.AllowAnyOrigin()
            //          .AllowAnyHeader()
            //          .AllowAnyMethod();
            //});
        });

        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        builder.Services.AddMvc(config =>
        {
            config.Filters.Add(new ExceptionFilter());
        }).ConfigureApiBehaviorOptions(options =>
        {
            options.SuppressMapClientErrors = true;
            options.SuppressModelStateInvalidFilter = true;
        })
        .AddRazorRuntimeCompilation();

        //註冊 CookieAuthentication，Scheme必填
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
        {
            //或許要從組態檔讀取，自己斟酌決定
            option.LoginPath = new PathString(AppSettings.Default.LoginUrl);//登入頁
            option.LogoutPath = new PathString(AppSettings.Default.LogoutUrl);//登出Action
                                                                              //用戶頁面停留太久，登入逾期，或Controller的Action裡用戶登入時，也可以設定↓
            option.ExpireTimeSpan = TimeSpan.FromMinutes(AppSettings.Default.LoginExpireMinutes);//沒給預設14天
                                                                            //↓資安建議false，白箱弱掃軟體會要求cookie不能延展效期，這時設false變成絕對逾期時間
                                                                            //↓如果你的客戶反應明明一直在使用系統卻容易被自動登出的話，你再設為true(然後弱掃policy請客戶略過此項檢查) 
            option.SlidingExpiration = true;
        });

        // 加上這行↓↓
        builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

        builder.Logging
            .AddConsole()
            .AddProvider(new FileLoggerProvider())
            .AddDebug();

        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = null; // JsonNamingPolicy.CamelCase;
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseCors("AllowMyClient");

        //app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();//Controller、Action才能加上 [Authorize] 屬性
        app.UseSession();

        app.Use(next =>
            context =>
            {
                context.Request.EnableBuffering();
                return next(context);
            });

        app.MapStaticAssets();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}")
            .WithStaticAssets();

        app.MapControllerRoute(
                    name: "actionName",
                    pattern: "{controller}/{*actionName}",
                    defaults: new { action = "HandleUnknownAction" });

        app.UseStaticFiles();

        app.Run();
    }
}
