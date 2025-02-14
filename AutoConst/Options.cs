﻿using CommandLine;

namespace AutoConst
{
	public class Options
	{
		[Option('t', "target", Required = true, HelpText = "Path to search for cs files in.")]
		public string TargetPath { get; set; } = "";
		[Option('p', "producers", Required = true, HelpText = "List of producers to make code for.")]
		public IEnumerable<string> Producers { get; set; } = new List<string>();

		[Option('o', "output", Required = false, HelpText = "Path to output resulting files to.")]
		public string OutPath { get; set; } = "";
	}
}
