using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Calcver {
    public class SemanticVersion : IComparable<SemanticVersion>, IEquatable<SemanticVersion> {
        readonly string _toString;

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

        public static SemanticVersion Parse(string input)
        {
            if (TryParse(input, out var version)) {
                return version;
            }
            else {
                throw new ArgumentException($"Invalid version string: {input}", nameof(input));
            }
        }
        public static bool TryParse(string input, out SemanticVersion version)
        {
            version = null;

            var match = parseRegex.Match(input);
            if (!match.Success)
                return false;

            var major = int.Parse(match.Groups[1].Value);
            var minor = int.Parse(match.Groups[2].Value);
            var patch = int.Parse(match.Groups[3].Value);
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
            version = new SemanticVersion((major, minor, patch), pre, meta);
            return true;
        }

        public SemanticVersion(int major, int minor, int patch)
            : this((major, minor, patch)) { }
        public SemanticVersion(SemanticVersion stable, string preRelease = null, string meta = null)
            : this((stable.Major, stable.Minor, stable.Patch), preRelease, meta) { }
        public SemanticVersion((int major, int minor, int patch) stable,
                    string preRelease = null, string meta = null)
        {
            Major = stable.major;
            Minor = stable.minor;
            Patch = stable.patch;
            Prerelease = preRelease;
            Metadata = meta;

            _toString = $"{Major}.{Minor}.{Patch}";
            if (Prerelease != null)
                _toString += $"-{Prerelease}";
            if (Metadata != null)
                _toString += $"+{Metadata}";
        }

        public int Major { get; }
        public int Minor { get; }
        public int Patch { get; }
        public string Prerelease { get; }
        public string Metadata { get; }

        public SemanticVersion GetBaseVersion()
            => new SemanticVersion(Major, Minor, Patch);

        public SemanticVersion BumpMajor(string prerelease = null, string metadata = null)
            => new SemanticVersion((Major + 1, 0, 0), prerelease, metadata);

        public SemanticVersion BumpMinor(string prerelease = null, string metadata = null)
            => new SemanticVersion((Major, Minor + 1, 0), prerelease, metadata);

        public SemanticVersion BumpPatch(string prerelease = null, string metadata = null)
            => new SemanticVersion((Major, Minor, Patch + 1), prerelease, metadata);

        public override string ToString()
            => _toString;

        public override int GetHashCode()
        {
            unchecked  // Allow integer overflow with wrapping
            {
                int hash = 17;
                hash = (hash * 23) + Major.GetHashCode();
                hash = (hash * 23) + Minor.GetHashCode();
                hash = (hash * 23) + Patch.GetHashCode();
                if (Prerelease != null) {
                    hash = (hash * 23) + Prerelease.GetHashCode();
                }
                return hash;
            }
        }

        public bool Equals(SemanticVersion other)
        {
            if (ReferenceEquals(other, null)) {
                return false;
            }
            return CompareTo(other) == 0;
        }

        public int CompareTo(SemanticVersion other)
        {
            if (ReferenceEquals(other, null)) {
                return 1;
            }
            return PartComparisons(other).Where(r => r != 0).FirstOrDefault();
        }

        private IEnumerable<int> PartComparisons(SemanticVersion other)
        {
            yield return Major.CompareTo(other.Major);
            yield return Minor.CompareTo(other.Minor);
            yield return Patch.CompareTo(other.Patch);
            if (Prerelease != null && other.Prerelease != null) {
                yield return Prerelease.CompareTo(other.Prerelease);
            }
            else if (Prerelease != null) {
                yield return -1;
            }
            else if (other.Prerelease != null) {
                yield return 1;
            }
        }

        public override bool Equals(object other)
        {
            return Equals(other as SemanticVersion);
        }

        public static bool operator ==(SemanticVersion a, SemanticVersion b)
        {
            if (ReferenceEquals(a, null)) {
                return ReferenceEquals(b, null);
            }
            return a.Equals(b);
        }

        public static bool operator !=(SemanticVersion a, SemanticVersion b)
        {
            return !(a == b);
        }

        public static bool operator >(SemanticVersion a, SemanticVersion b)
        {
            if (ReferenceEquals(a, null)) {
                return false;
            }
            return a.CompareTo(b) > 0;
        }

        public static bool operator >=(SemanticVersion a, SemanticVersion b)
        {
            if (ReferenceEquals(a, null)) {
                return ReferenceEquals(b, null) ? true : false;
            }
            return a.CompareTo(b) >= 0;
        }

        public static bool operator <(SemanticVersion a, SemanticVersion b)
        {
            if (ReferenceEquals(a, null)) {
                return ReferenceEquals(b, null) ? false : true;
            }
            return a.CompareTo(b) < 0;
        }

        public static bool operator <=(SemanticVersion a, SemanticVersion b)
        {
            if (ReferenceEquals(a, null)) {
                return true;
            }
            return a.CompareTo(b) <= 0;
        }
    }
}