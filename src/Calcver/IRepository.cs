using System;
using System.Collections.Generic;
using System.Text;

namespace Calcver {
    public interface IRepository {
        string RootPath { get; }
        TagInfo GetTag(string tagName);
        IEnumerable<TagInfo> GetTags();
        IEnumerable<CommitInfo> GetCommits(string since = null, string until = null);
    }
}
