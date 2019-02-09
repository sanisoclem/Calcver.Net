using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calcver {
    public static class CommitExtensions {
        public static IEnumerable<ConventionalCommit> GetConventionalCommits(this IEnumerable<CommitInfo> commits)
         => commits.Select(c => ConventionalCommit.TryParse(c, out var cc) ? cc : null).Where(cc => cc != null);
    }
}
