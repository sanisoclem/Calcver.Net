using System.Collections.Generic;

namespace Calcver
{
    public class VersionLog
    {
        public SemanticVersion Version { get; set; }
        public TagInfo Tag { get; set; } 
        public List<ConventionalCommit> Changes { get; set; }
    }
}