using System;
using System.Collections.Generic;
using Destructurama;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.SystemConsole.Themes;

namespace VolleyM.API
{
    public static class Logging
    {
        public static LoggerConfiguration CreateLoggerConfig()
        {
            Serilog.Debugging.SelfLog.Enable(Console.Error);

            var config = ReadConfiguration();

            var logConfig = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .Enrich.FromLogContext()
                .Destructure.UsingAttributes()
                .WriteTo.File(new RenderedCompactJsonFormatter(), "volleym-api.log", LogEventLevel.Debug)
                .WriteTo.Console(theme: GetVolleyMTheme());

            if (ShouldConfigureElasticSearch(config))
            {
                logConfig.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
                {
                    AutoRegisterTemplate = true,
                    AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                    MinimumLogEventLevel = LogEventLevel.Debug,
                    InlineFields = true
                });
            }


            return logConfig;
        }

        private static SystemConsoleTheme GetVolleyMTheme()
        {
            var dictionary = new Dictionary<ConsoleThemeStyle, SystemConsoleThemeStyle>();

            foreach (var (key, value) in SystemConsoleTheme.Literate.Styles)
            {
                dictionary[key] = value;
            }

            dictionary[ConsoleThemeStyle.LevelInformation] = new SystemConsoleThemeStyle { Foreground = ConsoleColor.DarkGreen };

            return new SystemConsoleTheme(dictionary);
        }

        private static IConfigurationRoot ReadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("serilog.json", true, reloadOnChange: true)
                .AddJsonFile($"serilog.{GetEnvironmentName()}.json", true, reloadOnChange: true);

            return builder.Build();
        }

        private static string GetEnvironmentName() =>
            Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        private static bool ShouldConfigureElasticSearch(IConfiguration config)
        {
            var elasticSwitch = config["Serilog:UseElastic"];

            if (bool.TryParse(elasticSwitch, out var result))
            {
                return result;
            }

            return false;
        }
    }
}