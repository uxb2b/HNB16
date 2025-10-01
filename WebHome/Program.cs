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

        // ���U CORS
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowMyClient", policy =>
            {
                policy.WithOrigins(AppSettings.Default.AllowCORS) // ���\���ӷ�
                      .AllowAnyHeader()                  // ���\�Ҧ� headers
                      .AllowAnyMethod();                 // ���\ GET, POST, PUT, DELETE
            });

            // �p�G�n���\�����ӷ� (�ȭ����եΡA����ĳ��������)
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

        //���U CookieAuthentication�AScheme����
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
        {
            //�γ\�n�q�պA��Ū���A�ۤv�r�u�M�w
            option.LoginPath = new PathString(AppSettings.Default.LoginUrl);//�n�J��
            option.LogoutPath = new PathString(AppSettings.Default.LogoutUrl);//�n�XAction
                                                                              //�Τ᭶�����d�Ӥ[�A�n�J�O���A��Controller��Action�̥Τ�n�J�ɡA�]�i�H�]�w��
            option.ExpireTimeSpan = TimeSpan.FromMinutes(AppSettings.Default.LoginExpireMinutes);//�S���w�]14��
                                                                            //����w��ĳfalse�A�սc�z���n��|�n�Dcookie���ੵ�i�Ĵ��A�o�ɳ]false�ܦ�����O���ɶ�
                                                                            //���p�G�A���Ȥ���������@���b�ϥΨt�Ϋo�e���Q�۰ʵn�X���ܡA�A�A�]��true(�M��z��policy�ЫȤᲤ�L�����ˬd) 
            option.SlidingExpiration = true;
        });

        // �[�W�o�����
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
        app.UseAuthorization();//Controller�BAction�~��[�W [Authorize] �ݩ�
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
