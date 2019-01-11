using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using System;
using System.Collections.Generic;
using System.Text;

namespace Calcver.Tests.Helpers {
    public class AutoNDataAttribute : AutoDataAttribute {
        public AutoNDataAttribute()
            : base(() => new Fixture()
                .Customize(new AutoNSubstituteCustomization())
                .Customize(new TagInfoCustomization()))
        {
        }
        protected AutoNDataAttribute(IFixture fixture)
            : base(() => fixture.Customize(new AutoNSubstituteCustomization()))
        {
        }
    }
}