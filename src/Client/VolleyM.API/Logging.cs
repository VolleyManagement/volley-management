using System;
using System.Collections.Generic;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Sinks.SystemConsole.Themes;

namespace VolleyM.API
{
    public static class Logging
    {
        public static LoggerConfiguration CreateLoggerConfig()
        {
            Serilog.Debugging.SelfLog.Enable(Console.Error);

            var config = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.File(new RenderedCompactJsonFormatter(), "volleym-api.log", LogEventLevel.Information)
                .WriteTo.Console(theme: GetVolleyMTheme());

            return config;
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
    }
}