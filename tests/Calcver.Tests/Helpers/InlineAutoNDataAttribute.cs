using AutoFixture.Xunit2;

namespace Calcver.Tests.Helpers
{
    public class InlineAutoNDataAttribute : InlineAutoDataAttribute
    {
        public InlineAutoNDataAttribute(params object[] values)
            : base(new AutoNDataAttribute(), values) {
        }
    }
}