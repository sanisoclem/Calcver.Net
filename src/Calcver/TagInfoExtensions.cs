using System.Collections.Generic;
using System.Linq;

namespace Calcver {
    public static class TagInfoExtensions {
        public static SemanticVersion GetVersion(this TagInfo tag)
        {
            if (tag.Name.StartsWith("v") && SemanticVersion.TryParse(tag.Name.Substring(1), out var version)) {
                return new SemanticVersion(version.Major, version.Minor, version.Patch, version.Prerelease, tag.Commit.ShortId());
            }
            else if (SemanticVersion.TryParse(tag.Name, out version)) {
                return new SemanticVersion(version.Major, version.Minor, version.Patch, version.Prerelease, tag.Commit.ShortId());
            }
            return null;
        }
    }
}