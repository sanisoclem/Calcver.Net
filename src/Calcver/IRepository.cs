using System;
using System.Collections.Generic;
using System.Text;

namespace Calcver
{
    public interface IRepository
    {
        IEnumerable<TagInfo> GetTags();
        IEnumerable<CommitInfo> GetCommits(string since = null, string until = null);
    }

    public interface IRepositoryFactory
    {
        IRepository GetRepository(string path);
    }

    public class TagInfo
    {
        public string Name { get; set; }
        public string Commit { get; set; }

    }
    public class CommitInfo
    {
        public string Id { get; set; }
        public string Message { get; set; }
    }
}
