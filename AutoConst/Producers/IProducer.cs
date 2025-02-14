using AutoConst.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoConst.Producers
{
	public interface IProducer
	{
		public string Name { get; }
		public string Extension { get; }
		public List<ResultFile> Produce(List<string> files);
	}
}
