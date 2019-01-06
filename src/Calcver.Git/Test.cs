using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calcver.Git
{
    class Test
    {
        public void tesTest() {
            using (var repo = new GitRepository(null)) {

                repo.GetVersion();
                repo.GetChangeLog(repo.GetTags().Last()); // -- gets the changelog for last tag
                repo.GetChangeLog(); // -- gets changelog for HEAD
            }
        }
    }
}
