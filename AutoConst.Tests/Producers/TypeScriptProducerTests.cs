using AutoConst.Producers;

namespace AutoConst.Tests.Producers
{
	[TestClass]
	public class TypeScriptProducerTests
	{
		[TestMethod]
		[DataRow("TestFiles/testinput.cs_", "TestFiles/expected1.ts")]
		public void Can_FindAllConstLines(string inputFile, string expectedFile)
		{
			// ARRANGE
			var producer = new TypeScriptProducer();

			// ACT
			var result = producer.Produce(new List<string>() { inputFile });

			// ASSERT
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(File.ReadAllText(expectedFile), result[0].Content);
		}
	}
}
