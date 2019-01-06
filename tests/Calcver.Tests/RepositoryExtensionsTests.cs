using AutoFixture;
using Calcver.Tests.Helpers;
using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Calcver.Tests
{
    public class RepositoryExtensionsTests
    {
        [Theory]
        [InlineAutoNData("1.0.0")]
        [InlineAutoNData("1.0.0-123")]
        [InlineAutoNData("v1.0.0")]
        [InlineAutoNData("v1.0.0-123")]
        public void GetVersion_WhenOnTag_UseTagAsVersion(
            string lastTag,
            IRepository repository) {
            // arrange
            repository.CreateMockCommits(lastTag, 0);

            // act 
            var version = repository.GetVersion();

            // assert
            version.Should().Be(lastTag.ParseSemanticVersion());
        }

        [Theory]
        [InlineAutoNData("1.0.1", 5, "r01", "feat: something", "1.1.0-5.r01")]
        [InlineAutoNData("1.0.1", 5, "r01", "feat(module): something", "1.1.0-5.r01")]
        [InlineAutoNData("1.0.1", 5, "r01", "fix: something", "1.0.2-5.r01")]
        [InlineAutoNData("1.0.1", 5, "r01", "fix(module): something", "1.0.2-5.r01")]
        [InlineAutoNData("v1.0.1", 5, "r01", "chore: something", "1.0.2-5.r01")]
        [InlineAutoNData("v1.0.1", 5, "r01", "docs: something", "1.0.2-5.r01")]
        [InlineAutoNData("v1.0.1", 5, "r01", "fix: something\nBREAKING CHANGE: something", "2.0.0-5.r01")]
        [InlineAutoNData("v1.0.1", 5, "r01", "fix(module): something\nBREAKING CHANGE: something", "2.0.0-5.r01")]
        public void GetVersion_WhenNoTag_ThenReturnPrerelease(
            string lastTag,
            int numCommits, // Assume > 0
            string buildNum,
            string commitMessage,
            string expected,
            IRepository repository) {
            // arrange
            repository.CreateMockCommits(lastTag, numCommits, commitMessage);

            // act 
            var version = repository.GetVersion(new CalcverSettings { PrereleaseSuffix = buildNum });

            // assert
            version.Should().Be(expected.ParseSemanticVersion());
        }

        [Theory]
        [InlineAutoNData("1.2.3", 5)]
        [InlineAutoNData("v1.2.3", 0)]
        [InlineAutoNData(null, 3)]
        public void GetVersion_AlwaysPutCommitHashMetadata(
            string lastTag,
            int numCommits,
            IRepository repository) {
            // arrange
            var commitSha = repository.CreateMockCommits(lastTag, numCommits);

            // act 
            var version = repository.GetVersion();

            // assert
            version.Metadata.Should().Be(commitSha);
        }

        [Theory]
        [AutoNData]
        public void GetVersion_WhenEmptyRepo_ReturnZero(
            IRepository repository) {
            // arrange
            repository.CreateMockCommits(null, 0);

            // act 
            var version = repository.GetVersion();

            // assert
            version.Should().Be("0.0.0".ParseSemanticVersion());
        }

        [Theory]
        [AutoNData]
        public void GetChangeLog_WhenNoCommits_ReturnNull(
            IRepository repository) {

            // arrange
            repository.GetTags().Returns(new TagInfo[] { });
            repository.GetCommits().ReturnsForAnyArgs(new CommitInfo[] { });

            // act
            var result = repository.GetChangeLog(null);

            // assert
            result.Should().BeNull();
        }

        [Theory]
        [AutoNData]
        public void GetChangeLog_WhenInvalidTag_ThrowArgumentException(
            TagInfo tag,
            TagInfo otherTag,
            IRepository repository) {

            // arrange
            repository.GetTags().Returns(new TagInfo[] { tag });

            // act
            Action action = () => repository.GetChangeLog(otherTag);

            // assert
            action.Should().Throw<ArgumentException>();
        }

        [Theory]
        [AutoNData]
        public void GetChangeLog_WhenHeadIsTagged_ReturnChangelogForTag(
            TagInfo v1,
            TagInfo v2,
            List<CommitInfo> v2Commits,
            IRepository repository) {

            // arrange
            repository.GetTags().Returns(new TagInfo[] { v1, v2 });
            repository.GetCommits(v2.Name, null).Returns(new CommitInfo[] { });
            repository.GetCommits(v1.Name, v2.Name).Returns(v2Commits);


            // act
            var result = repository.GetChangeLog(null);

            // assert
            result.Should().NotBeNull();
        }

        [Theory]
        [AutoNData]
        public void GetChangeLogs_WhenHeadIsTagged_ReturnCountShouldMatchTagCount(
            TagInfo v1,
            TagInfo v2,
            List<CommitInfo> commits,
            IRepository repository) {

            // arrange
            repository.GetTags().Returns(new TagInfo[] { v1, v2 });
            repository.GetCommits(v2.Name, null).Returns(new CommitInfo[] { });
            repository.GetCommits(v1.Name, v2.Name).Returns(commits);
            repository.GetCommits(null, v1.Name).Returns(commits);


            // act
            var result = repository.GetChangeLogs();

            // assert
            result.Count().Should().Be(2);
        }

        [Theory]
        [AutoNData]
        public void GetChangeLogs_WhenHeadIsNotTagged_ReturnCountShouldBeTagCountPlusOne(
            TagInfo v1,
            TagInfo v2,
            List<CommitInfo> commits,
            IRepository repository) {

            // arrange
            repository.GetTags().Returns(new TagInfo[] { v1, v2 });
            repository.GetCommits(v2.Name, null).Returns(commits);
            repository.GetCommits(v1.Name, v2.Name).Returns(commits);
            repository.GetCommits(null, v1.Name).Returns(commits);


            // act
            var result = repository.GetChangeLogs();

            // assert
            result.Count().Should().Be(3);
        }
    }

    public static class RepositoryTestHelpers
    {
        public static string CreateMockCommits(this IRepository repo, string lastTag, int numCommits = 0, string commitMessage = null) {
            var f = new Fixture();
            var commits = f.CreateMany<CommitInfo>(numCommits).ToList();

            if (lastTag != null) {
                var tag = new TagInfo {
                    Name = lastTag,
                    Commit = lastTag
                };
                repo.GetTags().Returns(new TagInfo[] { tag });
            }

            if (commitMessage != null && numCommits > 0)
                commits[0].Message = commitMessage;

            
            repo.GetCommits(lastTag).Returns(commits);

            return commits.LastOrDefault()?.Id ?? lastTag;
        }
    }
}