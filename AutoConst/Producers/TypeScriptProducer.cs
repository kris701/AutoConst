using AutoConst.Models;
using System.Text;
using System.Text.RegularExpressions;

namespace AutoConst.Producers
{
	public class TypeScriptProducer : IProducer
	{
		public string Name { get; } = "TypeScriptProducer";
		public string Extension { get; } = "ts";

		private static readonly Regex _nameRegex = new Regex("class (.*)", RegexOptions.Compiled);
		private static readonly Regex _constRegex = new Regex("const (.*?);", RegexOptions.Compiled);
		private static readonly Regex _propertyNameRegex = new Regex(" (.*?)=", RegexOptions.Compiled);
		private static readonly Regex _propertyValueRegex = new Regex("=(.*)", RegexOptions.Compiled);

		public List<ResultFile> Produce(List<string> files)
		{
			var newFiles = new List<ResultFile>();

			foreach (var file in files)
			{
				var fileInfo = new FileInfo(file);
				var sb = new StringBuilder();
				var text = File.ReadAllText(file);

				var nameMatch = _nameRegex.Match(text);

				sb.AppendLine("// This document is auto generated!");
				sb.AppendLine($"export const {nameMatch.Groups[1].Value.Replace("\r", "").Replace("\n", "")} = {{");

				var matches = _constRegex.Matches(text);
				foreach (Match match in matches)
				{
					var propNameMatch = _propertyNameRegex.Match(match.Groups[1].Value);
					var propValueMatch = _propertyValueRegex.Match(match.Groups[1].Value);
					sb.AppendLine($"\t{propNameMatch.Groups[1].Value.Trim()}: {propValueMatch.Groups[1].Value.Trim()},");
				}
				sb.AppendLine("}");

				newFiles.Add(new ResultFile(fileInfo.Name, sb.ToString()));
			}

			return newFiles;
		}
	}
}
