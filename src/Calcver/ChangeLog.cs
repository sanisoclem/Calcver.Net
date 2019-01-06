using System.Collections.Generic;

namespace Calcver
{
    public class ChangeLog
    {
        public SemanticVersion Version { get; set; }
        public TagInfo Tag { get; set; } 
        public List<ChangeEntry> Changes { get; set; } = new List<ChangeEntry>();
    }
}