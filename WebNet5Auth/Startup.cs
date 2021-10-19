using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using WebNet5Auth.Data;

namespace WebNet5Auth
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
            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddTransient<Identity_Initialize>();

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // пример конфигурации системы Identity
            services.Configure<IdentityOptions>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false; // Требование, чтобы был подтвержден Account

                options.Password.RequiredLength = 3;            // Минимальная длина парооля
                options.Password.RequireDigit = false;           // Пароль должен содержать цифры
                options.Password.RequireUppercase = false;       // Пароль должен содержать заглавные буквы
                options.Password.RequireLowercase = false;       // Пароль должен содержать строчные буквы
                options.Password.RequireNonAlphanumeric = false; // Пароль должен содержать не буквенно-цифровые символы

                options.User.RequireUniqueEmail = false;         // Требование уникальности Email

                options.Lockout.AllowedForNewUsers = true;       // Новые пользователи разблокированы
                options.Lockout.MaxFailedAccessAttempts = 1;     // Максимальное кол-во вводов не корректных пароля до блокировки
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(15);     // Время на которое блокируется уч. запись при неверно введенном пароле
            });

            // пример конфигурации Cookie
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "WebNet5Auth_Cookie";             // название Cookie
                options.Cookie.HttpOnly = true;                         // Передавать Cookie только по пртоколу http
                options.ExpireTimeSpan = TimeSpan.FromDays(7);          // Время существования Cookie

                options.LoginPath = "/Account/Login";                   // Если пользователь не авторизирован, то перенаправить его на указанную страницу
                options.AccessDeniedPath = "/Account/AccessDenied";      // Если отказано в доступе
            });

         
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Identity_Initialize init)
        {
            init.Initialize();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
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

            // включение системы аутентификации и авторизации
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                   name: "default",
                   pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
