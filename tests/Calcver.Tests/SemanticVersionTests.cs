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
    public class SemanticVerionTests
    {
        [Theory]
        [InlineData("0.0.0", null, true)]
        [InlineData("1.0.0", "0.1.0", true)]
        [InlineData("0.1.0", "0.0.1", true)]
        [InlineData("1.0.0", "1.0.0-test", true)]
        [InlineData("1.2.3", "1.2.2", true)]
        [InlineData("1.2.3", "1.2.3", false)]
        [InlineData("1.2.3", "1.2.4", false)]
        [InlineData(null, "1.0.0", false)]
        [InlineData(null, null, false)]
        public void GreaterLessThanOperator_ShouldReturnCorrectResults(
            string ver1,
            string ver2,
            bool expected) {

            // arrange
            var v1 = ver1 == null ? null : SemanticVersion.Parse(ver1);
            var v2 = ver2 == null ? null : SemanticVersion.Parse(ver2);

            // act
            var result = v1 > v2 && v1 >= v2;
            var result2 = v2 < v1 && v2 <= v1;

            // assert
            result.Should().Be(expected);
            result2.Should().Be(expected);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("0.0.0", "0.0.0+meta")]
        [InlineData("1.0.0-5", "1.0.0-5+meta")]
        public void EqualityOperatorsShouldReturnCorrectResults(
            string ver1,
            string ver2) {

            // arrange
            var v1 = ver1 == null ? null: SemanticVersion.Parse(ver1);
            var v2 = ver2 == null ? null : SemanticVersion.Parse(ver2);

            // act
            var result = v1 == v2 && v1 >= v2 && v1 <= v2;
            var result2 = v1 != v2;

            result.Should().BeTrue();
            result2.Should().BeFalse();
        }

        [Theory]
        [InlineData("1.0")]
        [InlineData("1.0.0.0")]
        [InlineData("1.0.0-05")]
        [InlineData("123")]
        [InlineData("parseinvalid")]
        public void Parse_WhenInvalid_ThenThrow(string version) {
            // act
            Action action = () => SemanticVersion.Parse(version);

            // assert
            action.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData("1.0")]
        [InlineData("1.0.0.0")]
        [InlineData("1.0.0-05")]
        [InlineData("555")]
        [InlineData("tryparseinvalid")]
        public void TryParse_WhenInvalid_ReturnFalse(string version) {
            // act
            var result = SemanticVersion.TryParse(version, out _);

            // assert
            result.Should().BeFalse();
        }

        [Theory]
        [InlineData("1.0.0")]
        [InlineData("1.0.0-5.1+meta")]
        [InlineData("1.0.0+meta")]
        [InlineData("1.0.0-5.87.521+meta")]
        public void Parse_WhenValid_ResultsMatchesInput(string version)
        {
            // act
            var result = SemanticVersion.Parse(version);

            // assert
            result.ToString().Should().Be(version.ToString()); // tostring to include metadata
        }
    }
}