using ClassLibrary.Data;
using ClassLibrary.Mvc.Services.AppSettings;
using Customizable.Menu.Filters;
using Customizable.Menu.Models.AppSettings;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Reflection;

namespace Customizable.Menu
{
    public static class HostingExtensions
    {
        private static AppSettings? _appSettings;

        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((ctx, lc) => lc
                .ReadFrom.Configuration(ctx.Configuration));

            _appSettings = new(builder.Configuration);
            builder.Services.AddAppSettingsService(options =>
            {
                options.AppSettings = _appSettings;
            });

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<SecurityFilterAttribute>();

            var migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(
                    _appSettings.ConnectionStrings.ApplicationDbConnection,
                    sql => {
                        sql.MigrationsAssembly(migrationsAssembly);
                        sql.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    }
                ));

            // Enable HSTS and HTTPS Redirect
            if (_appSettings.IsProduction)
            {
                builder.Services.AddHttpsRedirection(options =>
                {
                    options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
                    options.HttpsPort = 443;
                });
            }
            return builder.Build();
        }

        public static WebApplication ConfigurePipeline(this WebApplication app)
        {
            app.UseSerilogRequestLogging();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseExceptionHandler("/Home/Error/500");
            app.UseStatusCodePagesWithRedirects("~/Home/Error/{0}");

            // Add HSTS and HTTPS Redirect to pipeline
            if (_appSettings != null && _appSettings.IsProduction)
                app.UseHttpsRedirection();

            // Perform Migrations
            foreach (string message in ApplicationDbContext.Migrate(app))
                Log.Information(message);

            ApplicationDbBackup.Run(app);

            app.UseRouting();
            app.UseStaticFiles();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{guid?}"
            );

            return app;
        }
    }
}
