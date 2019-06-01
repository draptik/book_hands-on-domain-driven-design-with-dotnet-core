using Marketplace.ClassifiedAd;
using Marketplace.Domain.ClassifiedAd;
using Marketplace.Domain.Shared;
using Marketplace.Domain.UserProfile;
using Marketplace.Framework;
using Marketplace.Infrastructure;
using Marketplace.UserProfile;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
            var purgomalumClient = new PurgomalumClient();
            
            // RavenDb ======================================================
            // setup RavenDb
            var store = new DocumentStore
            {
                Urls = new[] {"http://localhost:8080"},
                Database = "Marketplace_Chapter9",
                Conventions =
                {
                    FindIdentityProperty = m => m.Name == "DbId"
                }
            };
            store.Initialize();

            services.AddSingleton<ICurrencyLookup, FixedCurrencyLookup>();
            services.AddScoped(c => store.OpenAsyncSession());
            services.AddScoped<IUnitOfWork, RavenDbUnitOfWork>();
            services.AddScoped<IClassifiedAdRepository, ClassifiedAdRepository>();
            services.AddScoped<IUserProfileRepository, UserProfileRepository>();
            services.AddScoped<ClassifiedAdsApplicationService>();
            services.AddScoped(c =>
                new UserProfileApplicationService(
                    c.GetService<IUserProfileRepository>(),
                    c.GetService<IUnitOfWork>(),
                    text => purgomalumClient.CheckForProfanity(text)
                        .GetAwaiter().GetResult()));
            // End RavenDb ==================================================
            
            
            
            // EF-Core ======================================================
//            const string connectionString = "Host=localhost;Database=Marketplace_Chapter8;Username=ddd;Password=book";
//            services
//                .AddEntityFrameworkNpgsql()
//                .AddDbContext<ClassifiedAdDbContext>(
//                    options => options.UseNpgsql(connectionString));
//
//            services.AddSingleton<ICurrencyLookup, FixedCurrencyLookup>();
//            services.AddScoped<IUnitOfWork, EfCoreUnitOfWork>();
//            services.AddScoped<IClassifiedAdRepository, ClassifiedAdRepository>();
//            services.AddScoped<ClassifiedAdsApplicationService>();
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

//            app.EnsureDatabase(); // <- only for EF-Core!!
            
            app.UseMvcWithDefaultRoute();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
                c.SwaggerEndpoint(
                    "/swagger/v1/swagger.json",
                    "ClassifiedAds v1"));
        }
    }
}