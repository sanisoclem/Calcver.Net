using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Calcver.Git
{
    public class GitRepository : IRepository, IDisposable
    {
        Repository _repo;

        public GitRepository(string path) {
            _repo = new Repository(Repository.Discover(path));
        }

        public IEnumerable<CommitInfo> GetCommits(string since = null, string until = null) {
            var filter = new CommitFilter();
            if (since != null)
                filter.ExcludeReachableFrom = _repo.Lookup(since);
            if (until != null)
                filter.IncludeReachableFrom = _repo.Lookup(until);

            return _repo.Commits.QueryBy(filter)
                .Select(c => c.ToCommitInfo());
        }

        public IEnumerable<TagInfo> GetTags() {
            return _repo.Tags.Select(t => new TagInfo {
                Name = t.FriendlyName,
                Commit = (_repo.Lookup(t.PeeledTarget.Sha) as Commit).ToCommitInfo(),
            }).Reverse();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
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

        public void Dispose() {
            Dispose(true);
        }
        #endregion
    }

    public static class Extensions
    {
        public static CommitInfo ToCommitInfo(this Commit commit)
            => commit == null ? null : new CommitInfo {
                Id = commit.Sha,
                Message = commit.Message
            };
    }
}