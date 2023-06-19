using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            services.AddControllersWithViews(option => {
                option.ModelBinderProviders.Insert(0, new DefaultBinderProvider());
            })   
                .AddRazorRuntimeCompilation()
                 .AddSessionStateTempDataProvider()
                 .AddFluentValidation(configuration => { configuration.RegisterValidatorsFromAssemblyContaining<Startup>(); })
                 .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.ConfigureSession($"{nameof(WebApp)}.Session", 60000);
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IConfigurationContext, ConfigurationContext>();
            services.AddTransient<ISessionContext, SessionContext>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IQuotationRepository, QuotationRepository>();
            services.AddScoped<IConfigRepository, ConfigRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();

            var connectionString = Configuration.GetConnectionString("Default");
            services.AddSingleton(option => new DbContextOptionBuilder(connectionString));
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

            app.UseAuthorization();
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
