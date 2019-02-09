using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Calcver.Git {
    public class GitRepository : IRepository, IDisposable {
        readonly string _root;
        Repository _repo;

        public GitRepository(string path)
        {
            _root = Repository.Discover(path);
            _repo = new Repository(_root);
        }
        public string RootPath => _root;
        public Repository Repository => _repo;

        public IEnumerable<CommitInfo> GetCommits(string since = null, string until = null)
        {
            var filter = new CommitFilter { SortBy = CommitSortStrategies.Reverse | CommitSortStrategies.Topological };
            if (since != null)
                filter.ExcludeReachableFrom = _repo.Lookup(since);
            if (until != null)
                filter.IncludeReachableFrom = _repo.Lookup(until);

            return _repo.Commits.QueryBy(filter)
                .Select(TranslateCommit);
        }

        public TagInfo GetTag(string tagName)
            => _repo.Tags.Where(t => t.FriendlyName == tagName)
            .Select(t => new TagInfo {
                Name = t.FriendlyName,
                Commit = TranslateCommit((t.PeeledTarget as Commit))
            }).SingleOrDefault();
        
        public IEnumerable<TagInfo> GetTags()
        {
            var commits = GetCommits();
            return from com in commits
                   join tag in _repo.Tags on com.Id equals tag.PeeledTarget.Sha
                   select new TagInfo {
                       Name = tag.FriendlyName,
                       Commit = com
                   };
        }

        private CommitInfo TranslateCommit(Commit commit)
            => commit == null ? null : new CommitInfo {
                Id = commit.Sha,
                Message = commit.Message
            };

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue) {
                if (disposing) {
                    if (_repo != null) {
                        _repo.Dispose();
                        _repo = null;
                    }
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}