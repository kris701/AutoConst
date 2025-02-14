using AutoConst.Models;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

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

		public List<ResultFile> Produce(List<string> files, bool merge)
		{
			if (merge)
				return ProduceMerge(files);
			else
				return ProduceIndividually(files);
		}

		private List<ResultFile> ProduceIndividually(List<string> files)
		{
			var newFiles = new List<ResultFile>();

			foreach (var file in files)
			{
				var sb = new StringBuilder();
				var text = File.ReadAllText(file);

				var nameMatch = _nameRegex.Match(text);
				var name = nameMatch.Groups[1].Value.Replace("\r", "").Replace("\n", "").Trim();

				sb.AppendLine("// This document is auto generated!");
				sb.AppendLine($"export const {name} = {{");

				var matches = _constRegex.Matches(text);
				foreach (Match match in matches)
				{
					var propNameMatch = _propertyNameRegex.Match(match.Groups[1].Value);
					var propValueMatch = _propertyValueRegex.Match(match.Groups[1].Value);
					sb.AppendLine($"\t{propNameMatch.Groups[1].Value.Trim()}: {propValueMatch.Groups[1].Value.Trim()},");
				}
				sb.AppendLine("}");

				newFiles.Add(new ResultFile(name, sb.ToString()));
			}

			return newFiles;
		}

		private List<ResultFile> ProduceMerge(List<string> files)
		{
			var newFiles = new List<ResultFile>();

			var classDict = new Dictionary<string, List<string>>();

			foreach (var file in files)
			{
				var fileInfo = new FileInfo(file);
				var text = File.ReadAllText(file);

				var nameMatch = _nameRegex.Match(text);
				var name = nameMatch.Groups[1].Value.Replace("\r", "").Replace("\n", "").Trim();
				if (classDict.ContainsKey(name))
					classDict[name].Add(file);
				else
					classDict.Add(name, new List<string>() { file });
			}

			foreach(var key in classDict.Keys)
			{
				var sb = new StringBuilder();
				sb.AppendLine("// This document is auto generated!");
				sb.AppendLine($"export const {key} = {{");

				foreach(var file in classDict[key])
				{
					var text = File.ReadAllText(file);
					var matches = _constRegex.Matches(text);
					foreach (Match match in matches)
					{
						var propNameMatch = _propertyNameRegex.Match(match.Groups[1].Value);
						var propValueMatch = _propertyValueRegex.Match(match.Groups[1].Value);
						sb.AppendLine($"\t{propNameMatch.Groups[1].Value.Trim()}: {propValueMatch.Groups[1].Value.Trim()},");
					}
				}
				sb.AppendLine("}");

				newFiles.Add(new ResultFile(key, sb.ToString()));
			}

			return newFiles;
		}
	}
}
