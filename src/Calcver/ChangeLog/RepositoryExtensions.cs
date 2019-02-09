using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calcver.ChangeLog {
    public static class RepositoryExtensions {

        public static VersionLog GetChangeLog(this IRepository repo, CalcverSettings settings = null)
            => repo.GetChangeLog(null, settings);

        public static VersionLog GetChangeLog(this IRepository repo, TagInfo tag, CalcverSettings settings = null)
        {
            TagInfo prevTag;
            if (tag == null) {
                prevTag = repo.GetVersionTags().LastOrDefault().Item1;
            }
            else {
                var tags = repo.GetVersionTags().ToList();
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

        public static IEnumerable<VersionLog> GetChangeLogs(this IRepository repo, CalcverSettings settings = null)
        {
            var tags = repo.GetVersionTags().ToList();
            TagInfo prevTag = null;
            foreach (var (tag, ver) in tags) {
                if (tag.Commit == prevTag?.Commit)
                    continue;
                yield return repo.GetChangeLogForRange(prevTag, tag);
                prevTag = tag;
            }

            var last = repo.GetChangeLogForRange(prevTag, null);
            if (last != null) {
                yield return last;
            }
        }

        private static VersionLog GetChangeLogForRange(this IRepository repo, TagInfo tagFrom, TagInfo target, CalcverSettings settings = null)
        {
            var commits = repo.GetCommits(tagFrom?.Name, target?.Name);

            if (commits.Count() == 0)
                return null;

            var retval = new VersionLog {
                Version = target?.GetVersion() ?? repo.GetVersion(settings),
                Changes = commits.GetConventionalCommits().ToList(),
                Tag = target // could be null
            };

            return retval;
        }
    }
}
