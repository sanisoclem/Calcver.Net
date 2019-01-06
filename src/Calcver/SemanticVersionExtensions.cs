using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Calcver
{
    public static class SemanticVersionExtensions {

        private static Regex parseRegex = new Regex(@"^
            \s*v?
            ([0-9]|[1-9][0-9]+)       # major version
            \.
            ([0-9]|[1-9][0-9]+)       # minor version
            \.
            ([0-9]|[1-9][0-9]+)       # patch version
            (\-([0-9A-Za-z\-\.]+))?   # pre-release version
            (\+([0-9A-Za-z\-\.]+))?   # build metadata
            \s*
            $",
            RegexOptions.IgnorePatternWhitespace);

        public static SemanticVersion ParseSemanticVersion(this string input) {
            if (TryParseSemanticVersion(input, out var version)) {
                return version;
            }
            else {
                throw new ArgumentException($"Invalid version string: {input}", nameof(input));
            }
        }
        public static bool TryParseSemanticVersion(this string input, out SemanticVersion version) {
            version = null;

            var match = parseRegex.Match(input);
            if (!match.Success)
                return false;

            var major = Int32.Parse(match.Groups[1].Value);
            var minor = Int32.Parse(match.Groups[2].Value);
            var patch = Int32.Parse(match.Groups[3].Value);
            string pre = null, meta = null;

            if (match.Groups[4].Success) {
                var inputPreRelease = match.Groups[5].Value;
                var cleanedPreRelease = string.Join(".", inputPreRelease
                    .Split('.')
                    .Select(s => new { IsNumeric = int.TryParse(s, out var n), Number = n, String = s })
                    .Select(s => s.IsNumeric ? s.Number.ToString() : s.String).ToArray());

                if (inputPreRelease != cleanedPreRelease) {
                    return false;
                }
                pre = cleanedPreRelease;
            }

            if (match.Groups[6].Success) {
                meta = match.Groups[7].Value;
            }
            version = new SemanticVersion(major, minor, patch, pre, meta);
            return true;
        }
    }
}