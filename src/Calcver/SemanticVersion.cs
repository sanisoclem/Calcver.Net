using System;
using System.Collections.Generic;
using System.Linq;

namespace Calcver
{
    public class SemanticVersion : IComparable<SemanticVersion>, IEquatable<SemanticVersion>
    {
        readonly string _toString;
        public SemanticVersion(int major, int minor, int patch,
                    string preRelease = null, string meta = null) {
            Major = major;
            Minor = minor;
            Patch = patch;
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
            => new SemanticVersion(Major + 1, 0, 0, prerelease, metadata);

        public SemanticVersion BumpMinor(string prerelease = null, string metadata = null)
            => new SemanticVersion(Major, Minor + 1, 0, prerelease, metadata);

        public SemanticVersion BumpPatch(string prerelease = null, string metadata = null)
            => new SemanticVersion(Major, Minor, Patch + 1, prerelease, metadata);

        public override string ToString()
            => _toString;

        public override int GetHashCode() {
            unchecked  // Allow integer overflow with wrapping
            {
                int hash = 17;
                hash = hash * 23 + Major.GetHashCode();
                hash = hash * 23 + Minor.GetHashCode();
                hash = hash * 23 + Patch.GetHashCode();
                if (Prerelease != null) {
                    hash = hash * 23 + Prerelease.GetHashCode();
                }
                return hash;
            }
        }

        public bool Equals(SemanticVersion other) {
            if (ReferenceEquals(other, null)) {
                return false;
            }
            return CompareTo(other) == 0;
        }

        public int CompareTo(SemanticVersion other) {
            if (ReferenceEquals(other, null)) {
                return 1;
            }
            return PartComparisons(other).Where(r => r != 0).FirstOrDefault();
        }

        private IEnumerable<int> PartComparisons(SemanticVersion other) {
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

        public override bool Equals(object other) {
            return Equals(other as SemanticVersion);
        }

        public static bool operator ==(SemanticVersion a, SemanticVersion b) {
            if (ReferenceEquals(a, null)) {
                return ReferenceEquals(b, null);
            }
            return a.Equals(b);
        }

        public static bool operator !=(SemanticVersion a, SemanticVersion b) {
            return !(a == b);
        }

        public static bool operator >(SemanticVersion a, SemanticVersion b) {
            if (ReferenceEquals(a, null)) {
                return false;
            }
            return a.CompareTo(b) > 0;
        }

        public static bool operator >=(SemanticVersion a, SemanticVersion b) {
            if (ReferenceEquals(a, null)) {
                return ReferenceEquals(b, null) ? true : false;
            }
            return a.CompareTo(b) >= 0;
        }

        public static bool operator <(SemanticVersion a, SemanticVersion b) {
            if (ReferenceEquals(a, null)) {
                return ReferenceEquals(b, null) ? false : true;
            }
            return a.CompareTo(b) < 0;
        }

        public static bool operator <=(SemanticVersion a, SemanticVersion b) {
            if (ReferenceEquals(a, null)) {
                return true;
            }
            return a.CompareTo(b) <= 0;
        }

    }
}