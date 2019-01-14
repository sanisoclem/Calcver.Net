namespace Calcver {
    public class CommitInfo
    {
        public string Id { get; set; }
        public string Message { get; set; }
        public string ShortId()
            => Id.Substring(0, 7);
    }
}
