using CommandLine;

namespace AutoConst
{
	public class Options
	{
		[Option('t', "target", Required = true, HelpText = "Either a set of individual files or paths to search for cs files in.")]
		public IEnumerable<string> TargetPath { get; set; } = new List<string>();
		[Option('p', "producers", Required = true, HelpText = "List of producers to make code for.")]
		public IEnumerable<string> Producers { get; set; } = new List<string>();

		[Option('o', "output", Required = false, HelpText = "Path to output resulting files to.")]
		public string OutPath { get; set; } = "";
		[Option('m', "merge", Required = false, HelpText = "If const files of the same class name should be merged together.")]
		public bool Merge { get; set; } = false;
	}
}
