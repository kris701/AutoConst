﻿using AutoConst.Producers;
using CommandLine;
using CommandLine.Text;
using ToolsSharp;

namespace AutoConst
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var parser = new Parser(with => with.HelpWriter = null);
			var parserResult = parser.ParseArguments<Options>(args);
			parserResult.WithNotParsed(errs => DisplayHelp(parserResult, errs));
			parserResult.WithParsed(Run);
		}

		public static void Run(Options opts)
		{
			opts.OutPath = DirectoryHelper.RootPath(opts.OutPath);

			ConsoleHelpers.WriteColor("Finding cs files...", ConsoleColor.Blue);
			var files = new List<string>();
			foreach (var target in opts.TargetPath)
			{
				if (target.EndsWith(".cs"))
					files.Add(target);
				else
					files.AddRange(Directory.GetFiles(target, "*.cs", SearchOption.AllDirectories));
			}
			ConsoleHelpers.WriteColor($"\tA total of {files.Count} cs files found.", ConsoleColor.Blue);
			ConsoleHelpers.WriteLineColor("Done!", ConsoleColor.Green);

			ConsoleHelpers.WriteColor("Filtering for const files...", ConsoleColor.Blue);
			var constFiles = new List<string>();
			foreach (var file in files)
			{
				var txt = File.ReadAllText(file);
				if (txt.Contains("const "))
					constFiles.Add(file);
			}
			ConsoleHelpers.WriteColor($"\tA total of {constFiles.Count} cs files with const found.", ConsoleColor.Blue);
			ConsoleHelpers.WriteLineColor("Done!", ConsoleColor.Green);

			ConsoleHelpers.WriteLineColor($"A total of {opts.Producers.Count()} producers to run.", ConsoleColor.Blue);
			var count = 1;
			foreach (var producerName in opts.Producers)
			{
				ConsoleHelpers.WriteLineColor($"\tExecuting producer {count++} out of {opts.Producers.Count()}", ConsoleColor.DarkGray);
				var producer = ProducerBuilder.GetProducer(producerName);
				var newFiles = producer.Produce(constFiles, opts.Merge);
				foreach (var file in newFiles)
					File.WriteAllText(Path.Combine(opts.OutPath, $"{file.Name}.{producer.Extension}"), file.Content);
			}
		}

		private static void HandleParseError(IEnumerable<Error> errs)
		{
			var sentenceBuilder = SentenceBuilder.Create();
			foreach (var error in errs)
				if (error is not HelpRequestedError)
					Console.WriteLine(sentenceBuilder.FormatError(error));
		}

		private static void DisplayHelp<T>(ParserResult<T> result, IEnumerable<Error> errs)
		{
			var helpText = HelpText.AutoBuild(result, h =>
			{
				h.AddEnumValuesToHelpText = true;
				return h;
			}, e => e, verbsIndex: true);
			Console.WriteLine(helpText);
			HandleParseError(errs);
		}
	}
}