using AutoFixture;

namespace Calcver.Tests.Helpers {
    public class TagInfoCustomization : ICustomization {
        public void Customize(IFixture fixture)
        {
            fixture.Register(() => {
                var ver = fixture.Create<SemanticVersion>().GetBaseVersion().ToString();
                return new TagInfo {
                    Commit = new CommitInfo { Id = ver.PadLeft(64,'x'), Message = string.Empty },
                    Name = ver
                };
            });
        }
    }
}