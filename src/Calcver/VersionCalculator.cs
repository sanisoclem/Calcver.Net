using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calcver
{
    public class CalcverContext
    {

    }

    public static class VersionCalculator
    {
        public static SemanticVersion GetVersion(this IRepository repo, CalcverSettings settings = null) {
            var (lastTag, lastStableVersion) = repo.GetLastStableVersion();

            if (lastStableVersion == null)
                lastStableVersion = new SemanticVersion(0, 0, 0);

            var includedCommits = repo.GetCommits(lastTag?.Name).ToList();

            if (includedCommits.Count == 0) {
                return new SemanticVersion(lastStableVersion.Major, lastStableVersion.Minor, lastStableVersion.Patch, lastStableVersion.Prerelease, lastTag?.Commit);
            }
            else {
                return lastStableVersion.CalculatePrereleaseVersion(includedCommits, settings?.PrereleaseSuffix);
            }
        }

        public static ChangeLog GetChangeLog(this IRepository repo, CalcverSettings settings = null)
            => repo.GetChangeLog(null, settings);

        public static ChangeLog GetChangeLog(this IRepository repo, TagInfo tag, CalcverSettings settings = null) {
            TagInfo prevTag;
            if (tag == null) {
                prevTag = repo.GetTags().FilterVersionTags().LastOrDefault().Item1;
            }
            else {
                var tags = repo.GetTags().FilterVersionTags().ToList();
                var match = new (TagInfo, SemanticVersion)[] { (null, null) }.Concat(tags)
                    .Zip(tags, (prev, next) => new { prev, next }).SingleOrDefault(i => i.next.Item1.Name == tag.Name);
                if (match == null)
                    throw new ArgumentException("Invalid tag", nameof(tag));
                prevTag = match.prev.Item1 ?? null;
            }
            var result = repo.GetChangeLogForRange(prevTag, tag);

            // result can be null if tag is null (when prevTag is HEAD or no commits in repo)
            if (result == null && prevTag != null) {
                // prevTag is HEAD
                return repo.GetChangeLog(prevTag);
            }

            return result;
        }

        public static IEnumerable<ChangeLog> GetChangeLogs(this IRepository repo, CalcverSettings settings = null) {
            var tags = repo.GetTags().FilterVersionTags().ToList();
            TagInfo prevTag = null;
            foreach (var (tag, ver) in tags) {
                yield return repo.GetChangeLogForRange(prevTag, tag);
                prevTag = tag;
            }

            var last = repo.GetChangeLogForRange(prevTag, null);
            if (last != null) {
                yield return last;
            }
        }

        private static ChangeLog GetChangeLogForRange(this IRepository repo, TagInfo tagFrom, TagInfo target, CalcverSettings settings = null) {
            var commits = repo.GetCommits(tagFrom?.Name, target?.Name);

            if (commits.Count() == 0)
                return null;

            var retval = new ChangeLog {
                Version = target?.GetVersion() ?? repo.GetVersion(settings),
                Tag = target // could be null
            };

            foreach (var commit in commits) {
                // TODO
            }

            return retval;
        }

        

        private static (TagInfo, SemanticVersion) GetLastStableVersion(this IRepository repository)
            => repository.GetTags().FilterVersionTags().LastOrDefault();

        private static SemanticVersion CalculatePrereleaseVersion(this SemanticVersion version, IList<CommitInfo> commits, string buildNumber = null) {
            var retval = version.GetBaseVersion();
            bool major = false, minor = false;
            foreach (var commit in commits) {
                if (CalcverSettings.MajorBumpRegex.IsMatch(commit.Message)) {
                    major = true;
                    break;
                }
                else if (minor) {
                    continue;
                }
                else if (CalcverSettings.MinorBumpRegex.IsMatch(commit.Message)) {
                    minor = true;
                }
            }

            var metadata = commits.Last().Id;
            var prerelease = $"{commits.Count}";
            if (buildNumber != null)
                prerelease += $".{buildNumber}";

            if (major)
                return retval.BumpMajor(prerelease,metadata);
            else if (minor)
                return retval.BumpMinor(prerelease, metadata);
            
            return retval.BumpPatch(prerelease, metadata);
        }
    }
}