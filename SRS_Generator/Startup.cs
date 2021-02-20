using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SRS_Generator.Config;
using SRS_Generator.Data;
using SRS_Generator.Infrastructure;
using SRS_Generator.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SRS_Generator
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("AppSettings.json");
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCustomServices(Configuration);
            services.AddCustomDbContext(Configuration);
            services.AddIntegrationServices(Configuration);

            var serviceProvider = services.BuildServiceProvider();

            var clientSettings = new ClientSettings();
            var commandSettings = new CommandSettings();
            Configuration.GetSection("ClientSettings").Bind(clientSettings);
            Configuration.GetSection("CommandSettings").Bind(commandSettings);

            var bot = new Bot(serviceProvider, clientSettings.Token, commandSettings.Prefixes);
            services.AddSingleton(bot);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

        }
    }

    public static class CustomExtensionMethods
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ClientSettings>(configuration.GetSection("ClientSettings"));
            services.Configure<CommandSettings>(configuration.GetSection("CommandSettings"));

            return services;
        }

        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<GuildContext>(options =>
            {
                var connectionString = configuration["ConnectionString"];
                options.UseSqlite(connectionString);
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            return services;
        }

        public static IServiceCollection AddIntegrationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IGuildService, GuildService>();
            services.AddTransient<IGuildMemberService, GuildMemberService>();

            return services;
        }
    }
}
