using AutoFixture;
using Calcver.Tests.Helpers;
using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Calcver.Tests.UnitTests {
    public class RepositoryExtensionsTests {
        [Theory]
        [ClassData(typeof(VersionCalculationTestData))]
        public void GetVersion_Always_CalculateCorrectVersion(
            string history,
            string expectedVersion,
            string headCommit)
        {
            // arrange
            var repository = Substitute.For<IRepository>();
            repository.CreateHistory(history,headCommit);

            // act 
            var version = repository.GetVersion();

            // assert
            version.Should().Be(SemanticVersion.Parse(expectedVersion));
        }

        [Theory]
        [InlineAutoNData("1.0.0")]
        [InlineAutoNData("1.0.0-123")]
        [InlineAutoNData("v1.0.0")]
        [InlineAutoNData("v1.0.0-123")]
        public void GetVersion_WhenOnTag_UseTagAsVersion(
            string lastTag,
            IRepository repository)
        {
            // arrange
            repository.CreateMockCommits(lastTag, 0);

            // act 
            var version = repository.GetVersion();

            // assert
            version.Should().Be(SemanticVersion.Parse(lastTag));
        }

        [Theory]
        [InlineAutoNData("1.0.1", 5, "r01", "feat: something", "1.1.0-5.r01")]
        [InlineAutoNData("1.0.1", 5, "r01", "feat(module): something", "1.1.0-5.r01")]
        [InlineAutoNData("1.0.1", 5, "r01", "fix: something", "1.0.2-5.r01")]
        [InlineAutoNData("1.0.1", 5, "r01", "fix(module): something", "1.0.2-5.r01")]
        [InlineAutoNData("v1.0.1", 5, "r01", "chore: something", "1.0.2-5.r01")]
        [InlineAutoNData("v1.0.1", 5, "r01", "docs: something", "1.0.2-5.r01")]
        [InlineAutoNData("v1.0.1", 5, "r01", "fix: something\n\nBREAKING CHANGE: something", "2.0.0-5.r01")]
        [InlineAutoNData("v1.0.1", 5, "r01", "fix: something\n\nSOMEMESSAGE\nMESSAGELINE2\n\nBREAKING CHANGE: something", "2.0.0-5.r01")]
        [InlineAutoNData("v1.0.1", 5, "r01", "fix(module): something\n\nBREAKING CHANGE: something", "2.0.0-5.r01")]
        public void GetVersion_WhenNoTag_ThenReturnPrerelease(
            string lastTag,
            int numCommits, // Assume > 0
            string buildNum,
            string commitMessage,
            string expected,
            IRepository repository)
        {
            // arrange
            repository.CreateMockCommits(lastTag, numCommits, commitMessage);

            // act 
            var version = repository.GetVersion(new CalcverSettings { PrereleaseSuffix = buildNum });

            // assert
            version.Should().Be(SemanticVersion.Parse(expected));
        }

        [Theory]
        [InlineAutoNData("1.2.3", 5)]
        [InlineAutoNData("v1.2.3", 0)]
        [InlineAutoNData(null, 3)]
        public void GetVersion_Always_PutFirst7CharsOfCommitHashInMetadata(
            string lastTag,
            int numCommits,
            IRepository repository)
        {
            // arrange
            var commitSha = repository.CreateMockCommits(lastTag, numCommits);

            // act 
            var version = repository.GetVersion();

            // assert
            version.Metadata.Should().Be(commitSha.Substring(0, 7));
        }

        [Theory]
        [AutoNData]
        public void GetVersion_WhenEmptyRepo_ReturnZero(
            IRepository repository)
        {
            // arrange
            repository.CreateMockCommits(null, 0);

            // act 
            var version = repository.GetVersion();

            // assert
            version.Should().Be(SemanticVersion.Parse( "0.0.0"));
        }
    }
}