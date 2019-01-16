using Calcver.Git;
using FluentAssertions;
using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace Calcver.Gt.Tests {
    public class GitRepositoryTests {
        [Fact]
        public void GetCommits_Always_ReturnOldestFirst()
        {
            // arrange
            using (var sut = new GitRepository(".")) {
                // act
                var result = sut.GetCommits();

                // assert
                result.Select(c => sut.Repository.Lookup(c.Id).Peel<Commit>().Committer.When.UtcDateTime)
                    .Should()
                    .BeInAscendingOrder();
            }
        }

        [Fact]
        public void GetCommits_WhenSinceSpecified_DoNotIncludeSinceCommit()
        {
            // arrange
            using (var sut = new GitRepository(".")) {
                var since = sut.GetTags().First();
                var sinceCommit = sut.Repository.Lookup(since.Commit.Id).Peel<Commit>();

                // act
                var result = sut.GetCommits(since.Name);

                // assert
                result.Should().NotContain(c => c.Id == sinceCommit.Sha)
                    .And.OnlyContain(c => sut.Repository.Lookup(c.Id).Peel<Commit>().Committer.When.UtcDateTime >= sinceCommit.Committer.When.UtcDateTime);
            }
        }

        [Fact]
        public void GetCommits_WhenUntilSpecified_ThenMustIncludeUntilCommit()
        {
            // arrange
            using (var sut = new GitRepository(".")) {
                var until = sut.GetTags().First();
                var untilCommit = sut.Repository.Lookup(until.Commit.Id).Peel<Commit>();

                // act
                var result = sut.GetCommits(null, until.Name);

                // assert
                result.Should().Contain(c => c.Id == untilCommit.Sha)
                    .And.OnlyContain(c => sut.Repository.Lookup(c.Id).Peel<Commit>().Committer.When.UtcDateTime <= untilCommit.Committer.When.UtcDateTime);
            }
        }

        [Fact]
        public void GetTags_Always_ReturnOldestFirst()
        {
            // arrange
            using (var sut = new GitRepository(".")) {
                // act
                var result = sut.GetTags().ToList();

                // assert
                result.Select(c => sut.Repository.Lookup(c.Commit.Id).Peel<Commit>().Committer.When.UtcDateTime)
                    .Should()
                    .BeInAscendingOrder();
            }
        }
    }
}
