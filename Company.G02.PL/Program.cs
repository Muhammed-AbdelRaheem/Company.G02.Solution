using Company.G02.BLL;
using Company.G02.BLL.Interfaces;
using Company.G02.BLL.Repositories;
using Company.G02.DAL.Data.Contexts;
using Company.G02.DAL.Models;
using Company.G02.PL.Mapping;
using Company.G02.PL.Mapping.Departments;
using Company.G02.PL.Mapping.Employees;
using Company.G02.PL.Mapping.Roles;
using Company.G02.PL.Mapping.Users;
using Company.G02.PL.Settings;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Company.G02.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            #region Configurations

            builder.Services.AddDbContext<AppDbContext>(
                           options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefalutConnection")));


            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();

            builder.Services.AddScoped<IUnitOfwork, UnitOfWork>();


            builder.Services.AddAutoMapper(typeof(EmployeeProfile), typeof(DepartmentProfile), typeof(UserProfile), typeof(RoleProfile));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(config => { config.LoginPath = "/Account/SignIn"; config.AccessDeniedPath = "/Account/AccessDenied"; });


            ///GoogleLogin 

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
                {
                    o.LoginPath = "/Account/Signin";
                    o.ExpireTimeSpan = TimeSpan.FromDays(5);
                    o.AccessDeniedPath = "/Account/AccessDenied";


                })


                .AddGoogle(o =>
                {
                    IConfiguration GoogleAuthSection = builder.Configuration.GetSection("Authentication:Google");
                    o.ClientId = GoogleAuthSection["ClientId"];
                    o.ClientSecret = GoogleAuthSection["ClientSecret"];
                    o.SignInScheme = IdentityConstants.ExternalScheme;

                });

            builder.Services.Configure<TwilloSettings>(builder.Configuration.GetSection("Twilio"));
            builder.Services.AddTransient<ISmsService, SmsService>();



            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
