using System.Text.RegularExpressions;

namespace Calcver {
    public class ConventionalCommit : CommitInfo {
        private static Regex conventionalCommitRegex
            = new Regex(@"^(?'type'fix|feat|chore|doc)          # Type
                            (?>\((?'scope'\w+)\))?              # scope
                            \:[\s-[\r\n]]*
                            (?'title'\S[^\r\n]+)                # title
                            (?>\n(?'desc'(?>\n(?!BREAKING\sCHANGE\:)[^\r\n]+)+))?                  # description
                            (?>[\n]{2,}BREAKING\sCHANGE\:[\s-[\r\n]]*(?'breaking'[^\r\n]+))?         # breaking change
                        ", RegexOptions.IgnorePatternWhitespace);

        public static bool TryParse(CommitInfo commitInfo, out ConventionalCommit conventionalCommit)
        {
            conventionalCommit = new ConventionalCommit();

            var parsed = conventionalCommitRegex.Match(commitInfo.Message);
            if (!parsed.Success)
                return false;

            conventionalCommit.Id = commitInfo.Id;
            conventionalCommit.Message = commitInfo.Message;
            conventionalCommit.Type = parsed.Groups["type"].Value;
            conventionalCommit.Scope = parsed.Groups["scope"].Success ? parsed.Groups["scope"].Value : null;
            conventionalCommit.Title = parsed.Groups["title"].Value;
            conventionalCommit.Description = parsed.Groups["desc"].Success ? parsed.Groups["desc"].Value : null;
            conventionalCommit.BreakingChange = parsed.Groups["breaking"].Success ? parsed.Groups["breaking"].Value : null;
            return true;
        }

        public ConventionalCommit() { }

        public string Type { get; set; }
        public string Scope { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string BreakingChange { get; set; }

        public bool HasBreakingChange => !string.IsNullOrEmpty(BreakingChange);
        public bool IsFeature => Type == "feat";
    }
}