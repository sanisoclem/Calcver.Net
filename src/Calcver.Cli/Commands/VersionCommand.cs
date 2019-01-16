using Calcver.Git;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using P3.Konsole.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Calcver.Cli.Commands
{
    public class VersionCommand : ICommand<string[]>
    {
        private readonly ILogger<ShowChangeLogCommand> _logger;

        public VersionCommand(ILogger<ShowChangeLogCommand> logger) {
            _logger = logger;
        }

        public Task ExecuteAsync(string[] parameter, CancellationToken token = default(CancellationToken)) {
            var dir = parameter.Length > 0 ? parameter[0] : Directory.GetCurrentDirectory();
            var suff = parameter.Length > 1 ? parameter[1] : null;
            using (var repo = new GitRepository(dir)) {
                var version = repo.GetVersion(new CalcverSettings {
                    PrereleaseSuffix = suff
                });

                Console.WriteLine(version);
            }
            return Task.CompletedTask;
        }
    }
}
