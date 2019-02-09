using AutoFixture;
using Calcver.ChangeLog;
using Calcver.Tests.Helpers;
using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Calcver.Tests.UnitTests.ChangeLog {
    public class RepositoryExtensionsTests {
        [Theory]
        [AutoNData]
        public void GetChangeLog_WhenNoCommits_ReturnNull(
            IRepository repository)
        {
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
            IRepository repository)
        {
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
            IRepository repository)
        {
            // arrange
            repository.GetTags().Returns(new TagInfo[] { v1, v2 });
            repository.GetCommits(v2.Name, null).Returns(new CommitInfo[] { });
            repository.GetCommits(v1.Name, v2.Name).Returns(v2Commits);

            // act
            var result = repository.GetChangeLog(null);

            // assert
            result.Should().NotBeNull();
            result.Tag.Should().NotBeNull();
            result.Version.Metadata.Should().Be(result.Tag.Commit.ShortId());
        }

        [Theory]
        [AutoNData]
        public void GetChangeLogs_WhenHeadIsTagged_ReturnCountShouldMatchTagCount(
            TagInfo v1,
            TagInfo v2,
            List<CommitInfo> commits,
            IRepository repository)
        {
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
            IRepository repository)
        {
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
        [Theory]
        [AutoNData]
        public void GetChangeLogs_WhenMultipleTagsOnCommit_OnlyTakeFirstTag(
            TagInfo v1,
            TagInfo v2,
            TagInfo v1b,
            List<CommitInfo> commits,
            IRepository repository)
        {
            // arrange
            v1b.Commit = v1.Commit;

            repository.GetTags().Returns(new TagInfo[] { v1, v1b, v2 });
            repository.GetCommits(v2.Name, null).Returns(commits);
            repository.GetCommits(v1.Name, v2.Name).Returns(commits);
            repository.GetCommits(v1b.Name, v2.Name).Returns(commits);
            repository.GetCommits(v1.Name, v1b.Name).Returns(new CommitInfo[] { });
            repository.GetCommits(null, v1.Name).Returns(commits);
            repository.GetCommits(null, v1b.Name).Returns(commits);

            // act
            var result = repository.GetChangeLogs();

            // assert
            result.Should().NotContain(c => c.Tag != null && c.Tag.Name == v1b.Name);
        }
    }
}