using FluentMigrator.Runner;
using GenricFrame.AppCode.DAL;
using GenricFrame.AppCode.Data;
using GenricFrame.AppCode.Helper;
using GenricFrame.AppCode.Interfaces;
using GenricFrame.AppCode.Migrations;
using GenricFrame.AppCode.Reops;
using GenricFrame.AppCode.Reops.Entities;
using GenricFrame.Models;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using System.Reflection;

namespace GenricFrame.AppCode.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void RegisterService(this IServiceCollection services, IConfiguration configuration)
        {
            string dbConnectionString = configuration.GetConnectionString("SqlConnection");
            GlobalDiagnosticsContext.Set("connectionString", dbConnectionString);
            IConnectionString ch = new ConnectionString { connectionString = dbConnectionString };
            services.AddSingleton<IConnectionString>(ch);
            services.AddSingleton<IDapperRepository, DapperRepository>();
            services.AddScoped<ApplicationDbContext>();
            services.AddTransient<IUserStore<ApplicationUser>, UserStore>();
            services.AddTransient<IRoleStore<ApplicationRole>, RoleStore>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<ILog, LogNLog>();
            services.AddSingleton<IRepository<EmailConfig>, EmailConfigRepo>();
            services.AddSingleton<IRepository<Category>, CategoryRepo>();
            services.AddSingleton<IRepository<Unit>, UnitRepo>();
            services.AddSingleton<IRepository<Product>, ProductRepo>();
            services.AddSingleton<IRepository<Banners>, BannersRepo>();
            services.AddSingleton<IRepository<News>, NewsRepo>();
            services.AddSingleton<IRepository<ApplicationUser>, UsersRepo>();
            services.AddSingleton<Database>();
            services.AddAutoMapper(typeof(Startup));
            services.AddHangfire(x => x.UseSqlServerStorage(dbConnectionString));
            services.AddHangfireServer();
            services.AddLogging(c => c.AddFluentMigratorConsole())
                .AddFluentMigratorCore()
                .ConfigureRunner(c => c.AddSqlServer2016()
                .WithGlobalConnectionString(configuration.GetConnectionString("SqlConnection"))
                .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations());
        }
    }
}