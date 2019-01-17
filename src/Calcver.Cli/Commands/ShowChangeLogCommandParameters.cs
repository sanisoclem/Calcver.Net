using P3.Konsole.Parser;

namespace Calcver.Cli.Commands {
    public class ShowChangeLogCommandParameters {
        [CommandParameter("d", LongName = "destination", IsRequired = true)]
        public string Destination { get; set; }

        [CommandParameter("a", LongName = "all", HasValue = false)]
        public bool All { get; set; }

        [CommandParameter("t", LongName = "tag")]
        public string Tag { get; set; }

        [CommandParameter("p", LongName = "path")]
        public string Path { get; set; }

        [CommandParameter("s", LongName = "suffix")]
        public string PrereleaseSuffix { get; set; }

        [CommandParameter("f", LongName = "format", IsRequired = false)]
        public ChangeLogFormat Format { get; set; }
    }
}
