using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MMDB_WebApp.Extensions;
using MMDB_WebApp.Handlers;
using MMDB_WebApp.Helpers;
using Microsoft.EntityFrameworkCore;
using MMDB_WebApp.Data;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;
using MMDB_WebApp.Services;

namespace MMDB_WebApp
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
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();

            // HttpClient
            // ==========
            services.AddTransient<ValidateHeaderHandler>();
            services.AddHttpClient("MMDB_API", c =>
            {
                c.BaseAddress = new Uri("https://localhost:44367/api/");
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            }).AddHttpMessageHandler<ValidateHeaderHandler>();

            // Start session state configuration
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromSeconds(3600);
                options.Cookie.HttpOnly = true;
                // Make the session cookie essential
                options.Cookie.IsEssential = true;
            });
            // End session state configuration

            // Start Localization
            // ==================

            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddControllersWithViews()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix, options => options.ResourcesPath = "Resources")
                .AddDataAnnotationsLocalization();

            // End Localization
            // ================

            services.AddControllersWithViews();
            services.AddHttpContextAccessor();
            services.AddSingleton<IStateHelper, StateHelper>();

            services.AddDbContext<MMDB_WebAppContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("MMDB_WebAppContext")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IStateHelper stateHelper)
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
            app.UseSession();

            // If state data is available in cookie, add it to the session
            // User checked the remember me checkbox
            // ===========================================================

            app.Use(async delegate (HttpContext context, Func<Task> next)
            {
                if (context.Request.GetStateData("StateData") != null)
                {
                    stateHelper.SetState(context.Request.GetStateData("StateData"));
                }
                await next.Invoke().ConfigureAwait(false);
            });


            // Start Localization
            // ==================

            // Get language preference from cookie and set in session
            app.Use(async delegate (HttpContext context, Func<Task> next)
            {
                if (context.Request.Cookies.ContainsKey("culture"))
                {
                    string culture = context.Request.Cookies["culture"];
                    context.Session.SetString("culture", culture);
                }
                await next.Invoke().ConfigureAwait(false);
            });

            // Configure Localization options
            var supportedCultures = new[]
            {
                new CultureInfo("en-US"),
                new CultureInfo("nl"),
            };
            var localizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            };
            localizationOptions.RequestCultureProviders.Clear();
            localizationOptions.RequestCultureProviders.Add(new CultureProviderResolverService());
            app.UseRequestLocalization(localizationOptions);

            // End Localization
            // ================

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "MyArea",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
