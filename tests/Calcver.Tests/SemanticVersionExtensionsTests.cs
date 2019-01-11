using AutoFixture;
using Calcver.Tests.Helpers;
using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Calcver.Tests {
    public class SemanticVersionExtensionsTests {
        [Theory]
        [InlineData("1.0")]
        [InlineData("1.0.0.0")]
        [InlineData("1.0.0-05")]
        [InlineData("123")]
        public void ParseSemanticVersions_WhenInvalid_ThenThrow(string version)
        {
            // act
            Action action = () => version.ParseSemanticVersion();

            // assert
            action.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData("1.0")]
        [InlineData("1.0.0.0")]
        [InlineData("1.0.0-05")]
        [InlineData("123")]
        public void TryParseSemanticVersions_WhenInvalid_ReturnFalse(string version)
        {
            // act
            var result = version.TryParseSemanticVersion(out _);

            // assert
            result.Should().BeFalse();
        }

        [Theory]
        [InlineData("1.0.0")]
        [InlineData("1.0.0-5.1+meta")]
        [InlineData("1.0.0+meta")]
        [InlineData("1.0.0-5.87.521+meta")]
        public void ParseSemanticVersions_WhenValid_ResultsMatchesInput(string version)
        {
            // act
            var result = version.ParseSemanticVersion();

            // assert
            result.ToString().Should().Be(version.ToString()); // tostring to include metadata
        }
    }
}