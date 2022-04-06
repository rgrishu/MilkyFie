using Milkyfie.AppCode.Data;
using Milkyfie.AppCode.Extensions;
using Milkyfie.AppCode.Interfaces;
using Milkyfie.AppCode.Middleware;
using Milkyfie.Models;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using GraphQL;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Playground;
using Milkyfie.AppCode.GraphQL.ProductQL;

namespace Milkyfie
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
            services.RegisterService(Configuration);
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            })
               .AddJwtBearer(options =>
               {
                   options.RequireHttpsMetadata = false;
                   options.SaveToken = false;
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,
                       ValidIssuer = Configuration["Jwt:Issuer"],
                       ValidAudience = Configuration["Jwt:Issuer"],
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                   };
               });
            /* End Jwd */
            services.AddControllersWithViews();
            services.AddMvc();
            services.AddSwaggerGen();
            #region Identity

            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 3;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.MaxFailedAccessAttempts = 3;
            }).AddUserStore<UserStore>()
            .AddRoleStore<RoleStore>()
            .AddUserManager<ApplicationUserManager>();
            //       .AddDefaultTokenProviders();
            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
            });
            #endregion
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
            });
            //services.AddHttpContextAccessor();
            #region GraphQL
            services.AddGraphQLServer().AddQueryType(q => q.Name("Query")).AddType<GraphQLQueryResolver>();
            #endregion
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILog logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UsePlayground(new PlaygroundOptions
                {
                    QueryPath = "/api",
                    Path = "/playground"
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.ConfigureExceptionHandler(logger);
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();
            // custom jwt auth middleware
            app.UseMiddleware<JwtMiddleware>();
            app.UseHangfireDashboard("/mydashboard");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=WebSite}/{action=Index}/{id?}");
                endpoints.MapGraphQL();
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1");
            });
        }
    }
}
