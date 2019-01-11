using AutoFixture;
using NSubstitute;
using System;
using System.Linq;

namespace Calcver.Tests.Helpers {
    public static class RepositoryTestHelpers {
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
    }
}