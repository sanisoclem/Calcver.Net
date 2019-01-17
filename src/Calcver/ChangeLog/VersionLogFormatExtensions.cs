using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Markdig.Syntax;
using System.Threading.Tasks;
using Markdig;
using Markdig.Renderers.Normalize;
using System.Linq;

namespace Calcver.ChangeLog {
    public static class VersionLogFormatExtensions {
        public static void WriteMarkdownChangeLog(this TextWriter writer, IEnumerable<VersionLog> logs)
        {
            foreach (var log in logs) {
                writer.WriteLine($"### {log.Version}");
                var feats = log.Changes.Where(c => c.IsFeature).ToList();
                var fixes = log.Changes.Where(c => c.IsFix).ToList();
                var breaking = log.Changes.Where(c => c.HasBreakingChange).ToList();

                if (feats.Any()) {
                    writer.WriteLine("#### Features");
                    writer.WriteMarkdownList(feats.Select(c => c.Title));
                }
                if (fixes.Any()) {
                    writer.WriteLine("#### Fixes");
                    writer.WriteMarkdownList(fixes.Select(c => c.Title));
                }
                if (breaking.Any()) {
                    writer.WriteLine("#### Breaking Changes");
                    writer.WriteMarkdownList(breaking.Select(c => c.Title));
                }
            }
            writer.Flush();
        }
        public static void WriteJsonChangeLog(this TextWriter writer, IEnumerable<VersionLog> logs)
        {
            var serializer = new JsonSerializer();
            serializer.Serialize(writer, logs);
        }

        private static void WriteMarkdownList(this TextWriter writer, IEnumerable<string> items)
        {
            foreach (var item in items) {
                writer.WriteLine($" * {item}");
            }
        }
    }
}
