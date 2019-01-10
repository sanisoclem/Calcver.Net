namespace Calcver
{
    public static class CommitInfoExtensions
    {
        public static string ShortSha(this CommitInfo commit)
            => commit.Id.Substring(0, 7);
    }
}