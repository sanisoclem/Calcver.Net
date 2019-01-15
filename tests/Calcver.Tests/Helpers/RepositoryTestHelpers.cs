using AutoFixture;
using NSubstitute;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Calcver.Tests.Helpers {
    public static class RepositoryTestHelpers {
        static readonly Regex historyRegex = new Regex(@"(?'sha'\w+) \((?'tag'v?[0-9\.]+)?\) (?'msg'.*)");

        public static string CreateMockCommits(this IRepository repo, string lastTag, int numCommits = 0, string commitMessage = null)
        {
            var f = new Fixture();
            var commits = f.CreateMany<CommitInfo>(numCommits).ToList();
            var tagCommitId = Convert.ToString(DateTime.Now.Ticks, 8);

            if (lastTag != null) {
                var tag = new TagInfo {
                    Name = lastTag,
                    Commit = new CommitInfo { Id = tagCommitId, Message = string.Empty }
                };
                repo.GetTags().Returns(new TagInfo[] { tag });
            }

            if (commitMessage != null && numCommits > 0)
                commits[0].Message = commitMessage;

            repo.GetCommits(lastTag).Returns(commits);

            return commits.LastOrDefault()?.Id ?? tagCommitId;
        }

        public static string CreateHistory(this IRepository repo, string historyDescription)
        {
            // -- parse the history data
            var hist = historyDescription.Split("\n\r".ToCharArray(), StringSplitOptions.RemoveEmptyEntries) // split by lines
                .Select(line => historyRegex.Match(line.Trim()))
                .Select((r,i) => new {
                    Index = i,
                    Sha = r.Groups["sha"].Value,
                    Tag = r.Groups["tag"].Value,
                    Msg = r.Groups["msg"].Value
                }).Reverse().ToList();

            // -- compute commits to return
            repo.GetCommits(Arg.Any<string>(), Arg.Any<string>()).Returns(c => {
                var qry = hist.AsEnumerable();
                if (c[0] is string since) {
                    var commit = qry.Single(a => a.Tag == since);
                    qry = qry.SkipWhile(a => a.Index >= commit.Index);
                }
                if (c[1] is string until) {
                    var commit = qry.Single(a => a.Tag == until);
                    qry = qry.Where(a => a.Index >= commit.Index);
                }
                return qry.Select(a => new CommitInfo {
                    Id = a.Sha,
                    Message = a.Msg.Replace(@"\n", "\n")
                });
            });

            repo.GetTags().Returns(c => hist.Where(a => !string.IsNullOrEmpty(a.Tag)).Select(a => new TagInfo {
                Commit = new CommitInfo {
                    Id = a.Sha,
                    Message = a.Msg
                },
                Name = a.Tag
            }));

            return hist.Last().Sha;
        }
    }
}