using Calcver.Cli.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using P3.Konsole;
using P3.Konsole.Commands;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Calcver.Cli {
    class Program {
        private static readonly IDictionary<string, string> SwitchMappings = new Dictionary<string, string>() {
        };
        private static readonly IDictionary<string, string> DefaultConfiguration = new Dictionary<string, string>() {
            {"Serilog:WriteTo:0:Name","Console" },
            {"Serilog:WriteTo:0:Args:theme","Serilog.Sinks.SystemConsole.Themes.AnsiConsoleTheme::Code, Serilog.Sinks.Console" },
            {"Serilog:WriteTo:0:Args:outputTemplate","[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {NewLine}{Exception}" }
        };

        public static async Task Main(string[] args)
        {
            var app = new KonsoleApplicationBuilder()
                .Configure(builder => builder
                    .AddInMemoryCollection(DefaultConfiguration)
                    .AddJsonFile("config.json", true)
                    .AddCommandLine(args, SwitchMappings))
                .ConfigureServices((col, config) => col
                  .AddLogging(lb => {
                      Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(config).CreateLogger();
                      lb.AddSerilog();
                  })
                    )
                .ConfigureCommands((builder, config) => builder
                    .DefineCommand<ShowChangeLogCommand>("log")
                    .DefineCommand<VersionCommand>("version")
                    .DefineCatchAllCommand<ShowChangeLogCommand>())
                .Build();

            try {
                var cancellation = new CancellationTokenSource();

                Console.CancelKeyPress += (sender, e) => {
                    e.Cancel = true;
                    cancellation.Cancel();
                };

                // -- execute command in child scope
                using (var scope = app.ServiceProvider.CreateScope()) {
                    await app.ExecuteCommandAsync(args, scope.ServiceProvider, cancellation.Token);
                }
            }
            catch (InvalidCommandException) {
                // PRINT USAGE
                // see: https://github.com/sanisoclem/Konsole/issues/9
                Console.Write("Invalid command");
                Environment.Exit(-1);
            }
            catch (Exception ex) {
                Console.WriteLine(ex.ToString());
                Environment.Exit(-2);
            }
        }
    }
}