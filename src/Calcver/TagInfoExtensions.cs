using System.Collections.Generic;
using System.Linq;

namespace Calcver
{
    public static class TagInfoExtensions
    {
        public static SemanticVersion GetVersion(this TagInfo tag) {
            if (tag.Name.StartsWith("v") && tag.Name.Substring(1).TryParseSemanticVersion(out var version)) {
                return version;
            }
            else if (tag.Name.TryParseSemanticVersion(out version)) {
                return version;
            }
            return null;
        }

        public static IEnumerable<(TagInfo, SemanticVersion)> FilterVersionTags(this IEnumerable<TagInfo> tags)
            => tags.Select(t => (t, t.GetVersion())).Where(t => t.Item2 != null);
    }
}