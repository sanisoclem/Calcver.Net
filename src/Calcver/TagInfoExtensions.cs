using System.Collections.Generic;
using System.Linq;

namespace Calcver
{
    public static class TagInfoExtensions
    {
        public static SemanticVersion GetVersion(this TagInfo tag) {
            if (tag.Name.StartsWith("v") && tag.Name.Substring(1).TryParseSemanticVersion(out var version)) {
                return new SemanticVersion(version.Major, version.Minor, version.Patch, version.Prerelease, tag.Commit.ShortSha());
            }
            else if (tag.Name.TryParseSemanticVersion(out version)) {
                return new SemanticVersion(version.Major, version.Minor, version.Patch, version.Prerelease, tag.Commit.ShortSha());
            }
            return null;
        }

        public static IEnumerable<(TagInfo, SemanticVersion)> FilterVersionTags(this IEnumerable<TagInfo> tags)
            =>  tags.Select(t => (t, t.GetVersion())).Where(t => t.Item2 != null);

        public static string GetScores(IEnumerable<int> a, IEnumerable<int> b) {
            var (scoreA,scoreB) = a.Zip(b, (cardA, cardB) => (cardA, cardB))
                .SkipWhile((s, i) => i % 2 == 0)
                .Aggregate((0, 0), (scores, i) => (i.cardA - i.cardB, i.cardB - i.cardA));

            if (scoreA == scoreB)
                return "Tie";
            return scoreA > scoreB ? "A" : "B";
        }
    }
}