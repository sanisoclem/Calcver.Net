using System;
using System.Collections.Generic;
using System.Linq;

namespace Calcver
{
    public class SemanticVersion : IComparable<SemanticVersion>, IEquatable<SemanticVersion>
    {
        public SemanticVersion(int major, int minor, int patch,
                    string preRelease = null, string meta = null) {
            Major = major;
            Minor = minor;
            Patch = patch;
            Prerelease = preRelease;
            Metadata = meta;
        }

        public int Major { get; set; }
        public int Minor { get; set; }
        public int Patch { get; set; }
        public string Prerelease { get; set; }
        public string Metadata { get; set; }

        public SemanticVersion GetBaseVersion()
            => new SemanticVersion(Major, Minor, Patch);
        public override string ToString() {
            var retval = $"{Major}.{Minor}.{Patch}";
            if (Prerelease != null)
                retval += $"-{Prerelease}";
            if (Metadata != null)
                retval += $"+{Metadata}";
            return retval;
        }

        public void BumpMajor() {
            Major += 1;
            Minor = 0;
            Patch = 0;
        }
        public void BumpMinor() {
            Minor += 1;
            Patch = 0;
        }
        public void BumpPatch() {
            Patch += 1;
        }

        public override int GetHashCode() {
            // The build version isn't included when calculating the hash,
            // as two versions with equal properties except for the build
            // are considered equal.

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
            } else if (other.Prerelease != null) {
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