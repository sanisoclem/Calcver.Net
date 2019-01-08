using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Calcver.Git.MsBuild
{
    public class GitCalcverTask : Task
    {
        [Output]
        public string CalculatedVersion { get; set; }

        public string PrereleaseSuffix { get; set; }
        public string RepositoryPath { get; set; }

        public override bool Execute() {

            try {
                var settings = new CalcverSettings {
                    PrereleaseSuffix = PrereleaseSuffix
                };
                using (var repo = new GitRepository(RepositoryPath)) {
                    CalculatedVersion =  repo.GetVersion(settings).ToString();
                }
                return true;
            }
            catch(Exception ex) {
                Log.LogErrorFromException(ex);
                return false;
            }
        }
    }
}
