namespace AutoConst.Producers
{
	public static class ProducerBuilder
	{
		private static readonly Dictionary<string, Func<IProducer>> _producers = new Dictionary<string, Func<IProducer>>()
		{
			{ "TypeScriptProducer", () => new TypeScriptProducer() },
		};

		public static IProducer GetProducer(string name) => _producers[name]();
	}
}
