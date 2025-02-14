using AutoConst.Models;

namespace AutoConst.Producers
{
	public interface IProducer
	{
		public string Name { get; }
		public string Extension { get; }
		public List<ResultFile> Produce(List<string> files, bool merge);
	}
}
