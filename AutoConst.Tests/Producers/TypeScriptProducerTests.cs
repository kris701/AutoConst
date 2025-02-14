using AutoConst.Producers;

namespace AutoConst.Tests.Producers
{
	[TestClass]
	public class TypeScriptProducerTests
	{
		[TestMethod]
		[DataRow("TestFiles/testinput1.cs_", "TestFiles/expected1.ts")]
		public void Can_ConvertSingle(string inputFile, string expectedFile)
		{
			// ARRANGE
			var producer = new TypeScriptProducer();

			// ACT
			var result = producer.Produce(new List<string>() { inputFile }, false);

			// ASSERT
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(File.ReadAllText(expectedFile), result[0].Content);
		}

		[TestMethod]
		[DataRow("TestFiles/expected2.ts", "TestFiles/testinput1.cs_", "TestFiles/testinput2.cs_")]
		public void Can_ConvertMultipleMerge(string expectedFile, params string[] inputFiles)
		{
			// ARRANGE
			var producer = new TypeScriptProducer();

			// ACT
			var result = producer.Produce(inputFiles.ToList(), true);

			// ASSERT
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(File.ReadAllText(expectedFile), result[0].Content);
		}

		[TestMethod]
		[DataRow("TestFiles/testinput1.cs_", "TestFiles/testinput2.cs_")]
		public void Can_ConvertMultipleNoMerge(params string[] inputFiles)
		{
			// ARRANGE
			var producer = new TypeScriptProducer();

			// ACT
			var result = producer.Produce(inputFiles.ToList(), false);

			// ASSERT
			Assert.AreEqual(2, result.Count);
		}
	}
}
