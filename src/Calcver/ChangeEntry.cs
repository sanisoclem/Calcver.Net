namespace Calcver
{
    public class ChangeEntry
    {
        public CommitInfo Commit { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Module { get; set; }
        public object Type { get; set; }
        public bool BreakingChange { get; set; }
    }
}