using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calcver {
    public static class RepositoryExtensions {
        public static IEnumerable<(TagInfo, SemanticVersion)> GetVersionTags(this IRepository repo)
           => repo.GetTags().Select(t => (t, t.GetVersion())).Where(t => t.Item2 != null);

        public static SemanticVersion GetVersion(this IRepository repo, CalcverSettings settings = null)
        {
            var (lastTag, lastStableVersion) = repo.GetVersionTags().LastOrDefault();

            if (lastStableVersion == null)
                lastStableVersion = new SemanticVersion(0, 0, 0);

            var includedCommits = repo.GetCommits(lastTag?.Name).ToList();

            if (includedCommits.Count == 0) {
                return new SemanticVersion(lastStableVersion.Major, lastStableVersion.Minor, lastStableVersion.Patch, lastStableVersion.Prerelease, lastTag?.Commit.ShortId());
            }
            else {
                return lastStableVersion.CalculatePrereleaseVersion(includedCommits, settings?.PrereleaseSuffix);
            }
        }

        private static SemanticVersion CalculatePrereleaseVersion(this SemanticVersion version, IList<CommitInfo> commits, string buildNumber = null)
        {
            var retval = version.GetBaseVersion();
            bool major = false, minor = false;
            foreach (var commit in commits.GetConventionalCommits()) {
                if (commit.HasBreakingChange) {
                    major = true;
                    break;
                }
                else if (minor) {
                    continue;
                }
                else if (commit.IsFeature) {
                    minor = true;
                }
            }

            var metadata = commits.Last().ShortId();
            var prerelease = $"{commits.Count}";
            if (buildNumber != null)
                prerelease += $".{buildNumber}";

            if (major)
                return retval.BumpMajor(prerelease, metadata);
            else if (minor)
                return retval.BumpMinor(prerelease, metadata);

            return retval.BumpPatch(prerelease, metadata);
        }
    }
}