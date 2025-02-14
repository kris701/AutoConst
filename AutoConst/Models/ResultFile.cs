namespace AutoConst.Models
{
	public class ResultFile
	{
		public string Name { get; set; }
		public string Content { get; set; }

		public ResultFile(string name, string content)
		{
			Name = name;
			Content = content;
		}
	}
}
