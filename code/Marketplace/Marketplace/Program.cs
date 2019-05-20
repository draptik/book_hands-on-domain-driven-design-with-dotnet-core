﻿using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static System.Reflection.Assembly;


namespace Marketplace
{
    public static class Program
    {
        static Program()
            => CurrentDirectory = Path.GetDirectoryName(GetEntryAssembly().Location);

        private static string CurrentDirectory { get; }

        public static void Main(string[] args)
        {
            var configuration = BuildConfiguration(args);
            ConfigureWebHost(configuration).Build().Run();
        }

        private static IConfiguration BuildConfiguration(string[] args)
            => new ConfigurationBuilder()
                .SetBasePath(CurrentDirectory)
                .Build();

        private static IWebHostBuilder ConfigureWebHost(IConfiguration configuration)
            => new WebHostBuilder()
                .UseStartup<Startup>()
                .UseConfiguration(configuration)
                .ConfigureServices(services => services.AddSingleton(configuration))
                .UseContentRoot(CurrentDirectory)
                .UseKestrel();
    }
}