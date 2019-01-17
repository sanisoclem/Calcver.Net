using Calcver.ChangeLog;
using Calcver.Git;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using P3.Konsole.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Calcver.Cli.Commands {
    public class ShowChangeLogCommand : ICommand<ShowChangeLogCommandParameters> {
        private readonly ILogger<ShowChangeLogCommand> _logger;

        public ShowChangeLogCommand(ILogger<ShowChangeLogCommand> logger)
        {
            _logger = logger;
        }

        public Task ExecuteAsync(ShowChangeLogCommandParameters parameter, CancellationToken token = default(CancellationToken))
        {
            var dir = parameter.Path ?? Directory.GetCurrentDirectory();
            VersionLog[] logs;
            var settings = new CalcverSettings {
                PrereleaseSuffix = parameter.PrereleaseSuffix
            };

            using (var repo = new GitRepository(dir)) {
                if (!string.IsNullOrEmpty(parameter.Tag) && parameter.All) {
                    _logger.LogCritical("Parameter {0} and {1} should not be specified together.", nameof(parameter.Tag), nameof(parameter.All));
                    return Task.CompletedTask;
                }

                if (!string.IsNullOrEmpty(parameter.Tag)) {
                    var tag = repo.GetTag(parameter.Tag);
                    if (tag == null) {
                        _logger.LogCritical("Tag {0} was not found", parameter.Tag);
                    }
                    logs = new VersionLog[] { repo.GetChangeLog(tag, settings) };
                }
                else if (parameter.All) {
                    // -- get all change logs
                    logs = repo.GetChangeLogs(settings).ToArray();
                }
                else {
                    // -- get the latest changelog
                    logs = new VersionLog[] { repo.GetChangeLog(settings) };
                }
            }

            WriteChangeLog(parameter.Destination, parameter.Format, logs);
            return Task.CompletedTask;
        }

        private void WriteChangeLog(string path, ChangeLogFormat format, params VersionLog[] logs)
        {
            using (var file = File.CreateText(path))
                switch (format) {
                    case ChangeLogFormat.Json:
                        file.WriteJsonChangeLog(logs);
                        break;
                    case ChangeLogFormat.Markdown:
                        file.WriteMarkdownChangeLog(logs);
                        break;
                    default:
                        _logger.LogCritical("Format {0} is not supported", format);
                        break;
                }
        }
    }
}