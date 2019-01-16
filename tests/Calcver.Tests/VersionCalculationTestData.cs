using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Calcver.Tests {
    public class VersionCalculationTestData : TheoryData<string,string,string>{
        public VersionCalculationTestData()
        {
            var scenario1 = @"
                f99ccc73a () feat: whatever\n\nBREAKING CHANGE: something
                f29ccc73a () feat: whatever\n\nnormalshit
                b3f34f0aa (v1.0.0) fix: something
                f99ccc72a () fix: whatever\n\nblah blah\n\nBREAKING CHANGE: something
                f99ccc71a () fix: whatever
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
                e37d110aa () chore: something";

            Add(scenario1, "2.0.0-2", "f99ccc73a");
            Add(scenario1, "1.1.0-1", "f29ccc73a");
            Add(scenario1, "1.0.0", "b3f34f0aa");
            Add(scenario1, "1.0.0-2", "f99ccc72a");
            Add(scenario1, "0.2.1", "f99ccc7aa");
            Add(scenario1, "0.2.2-1", "f99ccc71a");
            Add(scenario1, "0.2.0", "bcdc689aa");
            Add(scenario1, "0.2.0-2", "a3f34f0aa");
            Add(scenario1, "0.2.0-1", "993448aaa");
            Add(scenario1, "0.1.1", "daeb9aeaa");
            Add(scenario1, "0.1.0", "5ab0a20aa");
            Add(scenario1, "0.1.0-6", "0864088aa");
            Add(scenario1, "0.0.1-1", "e37d110aa");
        }
    }
}
