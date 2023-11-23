using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
namespace EMRReport.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
             .AddJsonFile("appsettings.json")
             .Build();

            Log.Logger = new LoggerConfiguration()
                //.ReadFrom.Configuration(configuration)
                .WriteTo.File(new JsonFormatter(),"Logs/important-logs.json",restrictedToMinimumLevel: LogEventLevel.Warning)
                .WriteTo.File("all-daily-.logs",rollingInterval: RollingInterval.Day)
                .MinimumLevel.Debug()// Set default minimum log level
                .CreateLogger();

            try
            {
                Log.Information("Starting ujkp");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application startup failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseWebRoot("wwwroot");
                    webBuilder.UseStartup<Startup>();
                }).UseSerilog();
            

           // .ConfigureLogging(loggerBuilder =>
            //{
              //  loggerBuilder.AddLog4Net("log4net.config");
              //  loggerBuilder.AddEventLog();
            //});
    }
}