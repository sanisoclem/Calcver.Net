using AutoFixture;

namespace Calcver.Tests.Helpers
{
    public class TagInfoCustomization : ICustomization
    {
        public void Customize(IFixture fixture) {
            fixture.Register<TagInfo>(() => {
                var ver = fixture.Create<SemanticVersion>();
                ver.Prerelease = null;
                ver.Metadata = null;
                var final = ver.ToString();
                return new TagInfo {
                    Commit = final,
                    Name = final
                };
            });
        }
    }
}