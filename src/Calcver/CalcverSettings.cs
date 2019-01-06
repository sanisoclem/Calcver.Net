using System.Text.RegularExpressions;

namespace Calcver
{
    public class CalcverSettings
    {
        public static readonly Regex MajorBumpRegex = new Regex("BREAKING CHANGE:");
        public static readonly Regex MinorBumpRegex = new Regex("^feat");
        public static readonly Regex PatchBumpRegex = new Regex("^fix");

        public string PrereleaseSuffix { get; set; }
    }
}