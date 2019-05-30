using Marketplace.Api;
using Marketplace.Domain;
using Marketplace.Framework;
using Marketplace.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Raven.Client.Documents;
using Swashbuckle.AspNetCore.Swagger;

namespace Marketplace
{
    public class Startup
    {
        public IHostingEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IHostingEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // RavenDb ======================================================
            // setup RavenDb
//            var store = new DocumentStore
//            {
//                Urls = new[] {"http://localhost:8080"},
//                Database = "Marketplace_Chapter8",
//                Conventions =
//                {
//                    FindIdentityProperty = m => m.Name == "DbId"
//                }
//            };
//            store.Initialize();
//
//            services.AddSingleton<ICurrencyLookup, FixedCurrencyLookup>();
//            services.AddScoped(c => store.OpenAsyncSession());
//            services.AddScoped<IUnitOfWork, RavenDbUnitOfWork>();
//            services.AddScoped<IClassifiedAdRepository, ClassifiedAdRepository>();
//            services.AddScoped<ClassifiedAdsApplicationService>();
            // End RavenDb ==================================================
            
            
            
            // EF-Core ======================================================
            const string connectionString = "Host=localhost;Database=Marketplace_Chapter8;Username=ddd;Password=book";
            services
                .AddEntityFrameworkNpgsql()
                .AddDbContext<ClassifiedAdDbContext>(
                    options => options.UseNpgsql(connectionString));

            services.AddSingleton<ICurrencyLookup, FixedCurrencyLookup>();
            services.AddScoped<IUnitOfWork, EfCoreUnitOfWork>();
            services.AddScoped<IClassifiedAdRepository, ClassifiedAdRepository>();
            services.AddScoped<ClassifiedAdsApplicationService>();
            // End EF-Core ==================================================
            
            
            services.AddMvc();
            services.AddSwaggerGen(c =>
                c.SwaggerDoc("v1",
                    new Info
                    {
                        Title = "ClassifiedAds",
                        Version = "v1"
                    }));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.EnsureDatabase(); // <- only for EF-Core!!
            
            app.UseMvcWithDefaultRoute();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
                c.SwaggerEndpoint(
                    "/swagger/v1/swagger.json",
                    "ClassifiedAds v1"));
        }
    }
}