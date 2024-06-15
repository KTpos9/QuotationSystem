using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuotationSystem.Data.Configurations;
using QuotationSystem.Data.Helpers;
using QuotationSystem.Data.Sessions;
using Zero.Core.Mvc.Binders;
using Zero.Core.Mvc.Startup;
using FluentValidation.AspNetCore;
using QuotationSystem.Data.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Zero.Core.Mvc.Authorizations.Requirements;
using QuotationSystem.ApplicationCore.Constants;
using Zero.Core.Mvc.Authorizations;
using Zero.Core.Mvc.Authorizations.Contexts;
using Zero.Core.Mvc.View;
using Microsoft.AspNetCore.Mvc.Razor;
using Zero.Core.Mvc.ViewLocators;
using QuotationSystem.Data.Repositories.Interfaces;
using FluentValidation;
using QuotationSystem.Services;

namespace QuotationSystem
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
            services.AddMemoryCache();
            services.ConfigureSession($"{nameof(QuotationSystem)}.Session", 600);

            services.AddControllersWithViews(option =>
            {
                option.ModelBinderProviders.Insert(0, new DefaultBinderProvider());
            })
                .AddRazorRuntimeCompilation()
                .AddSessionStateTempDataProvider();
                 //.SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssemblyContaining<Startup>();

            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new NamespaceViewLocationExpander());
            });

            services.ConfigureAntiforgery(nameof(QuotationSystem));
            services.ConfigureFormOptions();
            services.ConfigureResponseCompression();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.LoginPath = "/Account/Login/";
            });

            services.AddAuthorization(option =>
            {
                option.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .AddRequirements(new LoginRequirement(Policy.Login))
                    .Build();

                option.AddPolicy(Policy.UserManagement, policy => policy.Requirements.Add(new RoleRequirement(RoleId.UserManagement)));
                option.AddPolicy(Policy.ItemManagement, policy => policy.Requirements.Add(new RoleRequirement(RoleId.ItemManagement)));
                option.AddPolicy(Policy.QuotationManagement, policy => policy.Requirements.Add(new RoleRequirement(RoleId.QuotationManagement)));
            });

            services.AddSingleton<IAuthorizationHandler, LoginPolicyHandler>();
            services.AddSingleton<IAuthorizationHandler, RolePolicyHandler>();
            services.AddTransient<ILoginPolicyContext, SessionContext>();
            services.AddTransient<IRolePolicyContext, SessionContext>();

            services.AddHttpContextAccessor();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IConfigurationContext, ConfigurationContext>();
            services.AddTransient<ISessionContext, SessionContext>();
            services.AddScoped<IViewRenderService, ViewRenderService>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IQuotationRepository, QuotationRepository>();
            services.AddScoped<IConfigRepository, ConfigRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IUnitRepository, UnitRepository>();
            services.AddScoped<IRunningNoRepository, RunningNoRepository>();
            services.AddScoped<IWHRepository, WHRepository>();
            services.AddScoped<IStockRepository, StockRepository>();

            services.AddScoped<IExcelService, ExcelService>();

            var connectionString = Configuration.GetConnectionString("Default");
            services.AddScoped(option => new DbContextOptionBuilder(connectionString));
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
            app.UseSession();

            app.UseStaticFilesWithCache();

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
