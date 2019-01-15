using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Calcver.Tests {
    public class VersionCalculationTestData : TheoryData<string,string>{
        public VersionCalculationTestData()
        {
            Add("0.2.1", @"
                f99ccc7aa (v0.2.1) fix: whatever
                bcdc689aa (v0.2.0) chore: something
                a3f34f0aa () fix: something
                993448aaa () feat: something
                daeb9aeaa (v0.1.1) fix: something
                5ab0a20aa (v0.1.0) fix: something
                0864088aa () fix: something
                bbc0251aa () feat: something
                7f27feeaa () feat: something
                85c07c8aa () feat: something
                ebce5eeaa () feat: something
                e37d110aa () chore: something");

            Add("0.2.1-1", @"
                f99ccc7aa () fix: whatever
                bcdc689aa (v0.2.0) chore: something
                a3f34f0aa () fix: something
                993448aaa () feat: something
                daeb9aeaa (v0.1.1) fix: something
                5ab0a20aa (v0.1.0) fix: something
                0864088aa () fix: something
                bbc0251aa () feat: something
                7f27feeaa () feat: something
                85c07c8aa () feat: something
                ebce5eeaa () feat: something
                e37d110aa () chore: something");

            Add("0.2.0-4", @"
                f99ccc7aa () fix: whatever
                bcdc689aa () chore: something
                a3f34f0aa () fix: something
                993448aaa () feat: something
                daeb9aeaa (v0.1.1) fix: something
                5ab0a20aa (v0.1.0) fix: something
                0864088aa () fix: something
                bbc0251aa () feat: something
                7f27feeaa () feat: something
                85c07c8aa () feat: something
                ebce5eeaa () feat: something
                e37d110aa () chore: something");

            Add("0.2.0-5", @"
                f99ccc7aa () fix: whatever
                bcdc689aa () chore: something
                a3f34f0aa () fix: something
                993448aaa () feat: something
                daeb9aeaa () fix: something
                5ab0a20aa (v0.1.0) fix: something
                0864088aa () fix: something
                bbc0251aa () feat: something
                7f27feeaa () feat: something
                85c07c8aa () feat: something
                ebce5eeaa () feat: something
                e37d110aa () chore: something");

            Add("0.1.0-12", @"
                f99ccc7aa () fix: whatever
                bcdc689aa () chore: something
                a3f34f0aa () fix: something
                993448aaa () feat: something
                daeb9aeaa () fix: something
                5ab0a20aa () fix: something
                0864088aa () fix: something
                bbc0251aa () feat: something
                7f27feeaa () feat: something
                85c07c8aa () feat: something
                ebce5eeaa () feat: something
                e37d110aa () chore: something");

            Add("1.0.1-3", @"
                f99ccc7aa () fix: whatever
                bcdc689aa () chore: something
                a3f34f0aa () fix: something
                993448aaa (v1.0.0) feat: something
                daeb9aeaa () fix: something
                5ab0a20aa () fix: something
                0864088aa () fix: something
                bbc0251aa () feat: something
                7f27feeaa () feat: something
                85c07c8aa () feat: something
                ebce5eeaa () feat: something
                e37d110aa () chore: something");

            Add("2.0.0-3", @"
                f99ccc7aa () fix: whatever\n\nBREAKING CHANGE: something
                bcdc689aa () chore: something
                a3f34f0aa () fix: something
                993448aaa (v1.0.0) feat: something
                daeb9aeaa () fix: something
                5ab0a20aa () fix: something
                0864088aa () fix: something
                bbc0251aa () feat: something
                7f27feeaa () feat: something
                85c07c8aa () feat: something
                ebce5eeaa () feat: something
                e37d110aa () chore: something");

            Add("2.0.0-3", @"
                f99ccc7aa () fix: whatever\n\nblah blah\n\nBREAKING CHANGE: something
                bcdc689aa () chore: something
                a3f34f0aa () fix: something
                993448aaa (v1.0.0) feat: something
                daeb9aeaa () fix: something
                5ab0a20aa () fix: something
                0864088aa () fix: something
                bbc0251aa () feat: something
                7f27feeaa () feat: something
                85c07c8aa () feat: something
                ebce5eeaa () feat: something
                e37d110aa () chore: something");
        }
    }
}
