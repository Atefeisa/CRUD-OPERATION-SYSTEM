using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MVC.BLL;
using MVC.BLL.Interfaces;
using MVC.BLL.Repository;
using MVC.DAL.Data;
using MVC.DAL.Models;
using MVC.PL.Helper;
using MVC.PL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.PL
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }




        // This method gets called by the runtime. Use this method to add services to the container.


        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllersWithViews(); // Register Built in MVC Services to the container


            //services.AddScoped<AppDbContext>();


            services.AddScoped<IDepartmentRepository, DepartmentRepository>();     //  Allow DI For DepartmentRepository

            services.AddScoped<IEmployeeRepository, EmployeeRepository>();     //  Allow DI For EmployeeRepository

            services.AddScoped<IUnitOfWork, UnitOfWork>();


            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));

            });


            //  services.AddScoped<DepartmentRepository>();


            //services.AddScoped();     // LifeTime Per Request -> Object UnReachable

            //services.AddSingleton();  // LifeTime Per App

            //services.AddTransient();  // LifeTime Per Operation

            //services.AddAutoMapper(typeof(MappingProfiles));





            services.AddScoped<IScopedService, ScopedService>(); //Per Request

            services.AddTransient<ITransientService, TransientService>(); //Per Operation

            services.AddSingleton<ISingeltonService, SingeltonService>(); //Per App

            services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));

            //services.AddScoped<UserManager<ApplicationUser>> ();
            //services.AddScoped<SignInManager<ApplicationUser>>();

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();


            services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/Account/SignIn";
                //config.AccessDeniedPath = "/Home/Error";


            });


            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,config =>
            //    {
            //        config.LoginPath = "/Account/SignIn";
            //        config.AccessDeniedPath = "/Home/Error";

            //    }) ;




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
