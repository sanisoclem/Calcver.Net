namespace Calcver {
    public class TagInfo
    {
        public string Name { get; set; }
        public CommitInfo Commit { get; set; }

        public SemanticVersion GetVersion()
        {
            if (Name.StartsWith("v") && SemanticVersion.TryParse(Name.Substring(1), out var version)) {
                return new SemanticVersion(version.Major, version.Minor, version.Patch, version.Prerelease, Commit.ShortId());
            }
            else if (SemanticVersion.TryParse(Name, out version)) {
                return new SemanticVersion(version.Major, version.Minor, version.Patch, version.Prerelease, Commit.ShortId());
            }
            return null;
        }
    }
}
