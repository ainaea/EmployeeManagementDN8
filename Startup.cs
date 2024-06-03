using EmployeeManagement.Models;
using EmployeeManagement.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement
{
    public class Startup
    {
        private IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(_config.GetConnectionString("EmployeeConnection")));
            //services.AddControllersWithViews();
            services.AddIdentity<ApplicationUser, IdentityRole>(option =>
            {
                //option.Password.RequiredLength = 10;
                //option.Password.RequiredUniqueChars = 3;
            })
                    .AddEntityFrameworkStores<AppDbContext>();

            //services.Configure<IdentityOptions>(option =>
            //{
            //    option.Password.RequiredLength = 10;
            //    option.Password.RequiredUniqueChars = 3;
            //});

            services.AddMvc(option =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                .RequireAuthenticatedUser()
                                .Build();
                option.Filters.Add(new AuthorizeFilter(policy));
            }).AddXmlSerializerFormatters();

            services.ConfigureApplicationCookie(option =>           //configuring default route
            {
                option.AccessDeniedPath = new PathString("/Administration/AccessDenied");
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("DeleteRolePolicy", policy => policy.RequireClaim("Delete Role")
                                                                        /*.RequireClaim("Edit Role")*/);        //for chaining multiple claims to a policy
                //options.AddPolicy("EditRolePolicy", policy => policy.RequireClaim("Edit Role", "true"));
                //options.AddPolicy("EditRolePolicy", policy => policy.RequireClaim("Edit Role", "abc", "cdf"));        //for abc or cdf


                //options.AddPolicy("EditRolePolicy", policy => policy.RequireAssertion( context => context.User.IsInRole("Edit Role") && context.User.HasClaim(claim => claim.Type == "Edit Role" && claim.Value == "true") || context.User.IsInRole("Super Admin")));               //using func for authorization

                options.AddPolicy("EditRolePolicy", policy => policy.AddRequirements(new ManageAdminRolesAndClaimsRequirement()));

                options.AddPolicy("AdminRolePolicy", policy => policy.RequireRole("Admin"));            //using Role with policy
                //options.InvokeHandlersAfterFailure = false;     //to prevent calling other handlers if prior handlers fail.
            });
            //services.AddSingleton<IEmployeeRepository, MockEmployeeRepository>();
            services.AddScoped<IEmployeeRepository, SQLEmployeeRepository>();
            //services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddSingleton<IAuthorizationHandler, CanEditOnlyOtherAdminRolesAndClaimsHandler>();
            services.AddSingleton<IAuthorizationHandler, SuperAdminHandler>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //DeveloperExceptionPageOptions developerExceptionPageOptions = new DeveloperExceptionPageOptions { SourceCodeLineCount = 3 };
                //app.UseDeveloperExceptionPage(developerExceptionPageOptions);
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                //app.UseStatusCodePages();     //display a non-customizable but not exception page
                //app.UseStatusCodePagesWithRedirects("/Error/{0}");      //displays the page argument
                app.UseStatusCodePagesWithReExecute("/Error/{0}");      //displays the page argument
            }

            //DefaultFilesOptions defaultFilesOptions = new DefaultFilesOptions();
            //defaultFilesOptions.DefaultFileNames.Clear();
            //defaultFilesOptions.DefaultFileNames.Add("foo.html");
            //app.UseDefaultFiles(defaultFilesOptions);
            app.UseStaticFiles();
            //app.UseMvcWithDefaultRoute();
            //app.UseRouting();
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //    name: "default",
            //    pattern: "{controller=Home}/{action=Index}/{id?}");
            //});

            //FileServerOptions fileServerOptions = new FileServerOptions();
            //fileServerOptions.DefaultFilesOptions.DefaultFileNames.Clear();
            //fileServerOptions.DefaultFilesOptions.DefaultFileNames.Add("default.html");
            //app.UseFileServer(fileServerOptions);

            //app.UseFileServer();
            app.UseAuthentication();
            app.UseRouting().UseAuthorization().UseEndpoints(endpoints => endpoints.MapControllerRoute(
                name: default,
                pattern: "{controller=Home}/{action=Index}/{id?}"));
            //app.Run(async (context) =>
            //{
            //    //throw new Exception("An errror has been detected");
            //    //await context.Response.WriteAsync(_config["Play"]);
            //    await context.Response.WriteAsync("Hello world");
            //});
        }
    }
}
