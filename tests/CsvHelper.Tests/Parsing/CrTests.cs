// Copyright 2009-2020 Josh Close and Contributors
// This file is a part of CsvHelper and is dual licensed under MS-PL and Apache 2.0.
// See LICENSE.txt for details or visit http://www.opensource.org/licenses/ms-pl.html for MS-PL and http://opensource.org/licenses/Apache-2.0 for Apache 2.0.
// https://github.com/JoshClose/CsvHelper
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using System.IO;
using System.Text;

namespace CsvHelper.Tests.Parsing
{
	[TestClass]
	public class CrTests
	{
		[TestMethod]
		public void SingleFieldAndSingleRowTest()
		{
			var s = new StringBuilder();
			s.Append("1\r");
			using (var reader = new StringReader(s.ToString()))
			using (var parser = new CsvParser(reader, CultureInfo.InvariantCulture))
			{
				var row = parser.Read();
				Assert.AreEqual("1", row[0]);
			}
		}

		[TestMethod]
		public void SingleFieldAndSingleRowAndRowStartsWithCRTest()
		{
			var s = new StringBuilder();
			s.Append("\r1\r");
			using (var reader = new StringReader(s.ToString()))
			using (var parser = new CsvParser(reader, CultureInfo.InvariantCulture))
			{
				var row = parser.Read(); // null
				Assert.AreEqual("1", row[0]);
				// row = parser.Read(); // still null
			}
		}

		[TestMethod]
		public void SingleFieldAndSingleRowAndRowStartsWithCRNLTest()
		{
			var s = new StringBuilder();
			s.Append("\r\n1\r\n");
			using (var reader = new StringReader(s.ToString()))
			using (var parser = new CsvParser(reader, CultureInfo.InvariantCulture))
			{
				var row = parser.Read();
				Assert.AreEqual("1", row[0]);
			}
		}

		[TestMethod]
		public void SingleFieldAndSingleRowAndRowStartsWithNLTest()
		{
			var s = new StringBuilder();
			s.Append("\n1\n");
			using (var reader = new StringReader(s.ToString()))
			using (var parser = new CsvParser(reader, CultureInfo.InvariantCulture))
			{
				var row = parser.Read();
				Assert.AreEqual("1", row[0]);
			}
		}

		[TestMethod]
		public void SingleFieldAndSingleRowAndFieldIsQuotedTest()
		{
			var s = new StringBuilder();
			s.Append("\"1\"\r");
			using (var reader = new StringReader(s.ToString()))
			using (var parser = new CsvParser(reader, CultureInfo.InvariantCulture))
			{
				var row = parser.Read();
				Assert.AreEqual("1", row[0]);
			}
		}

		[TestMethod]
		public void SingleFieldAndMultipleRowsAndFirstFieldInFirstRowIsQuotedAndNoLineEndingTest()
		{
			var s = new StringBuilder();
			s.Append("\"1\"\r");
			s.Append("2");
			using (var reader = new StringReader(s.ToString()))
			using (var parser = new CsvParser(reader, CultureInfo.InvariantCulture))
			{
				var row = parser.Read();
				Assert.AreEqual("1", row[0]);

				row = parser.Read();
				Assert.AreEqual("2", row[0]);
			}
		}

		[TestMethod]
		public void SingleFieldAndMultipleRowsAndFirstFieldInFirstRowIsQuotedAndHasLineEndingTest()
		{
			var s = new StringBuilder();
			s.Append("\"1\"\r");
			s.Append("2\r");
			using (var reader = new StringReader(s.ToString()))
			using (var parser = new CsvParser(reader, CultureInfo.InvariantCulture))
			{
				var row = parser.Read();
				Assert.AreEqual("1", row[0]);

				row = parser.Read();
				Assert.AreEqual("2", row[0]);
			}
		}

		[TestMethod]
		public void SingleFieldAndMultipleRowsTest()
		{
			var s = new StringBuilder();
			s.Append("1\r");
			s.Append("2\r");
			using (var reader = new StringReader(s.ToString()))
			using (var parser = new CsvParser(reader, CultureInfo.InvariantCulture))
			{
				var row = parser.Read();
				Assert.AreEqual("1", row[0]);

				row = parser.Read();
				Assert.AreEqual("2", row[0]);
			}
		}

		[TestMethod]
		public void SingleFieldAndMultipleRowsAndLastRowHasNoLineEndingTest()
		{
			var s = new StringBuilder();
			s.Append("1\r");
			s.Append("2");
			using (var reader = new StringReader(s.ToString()))
			using (var parser = new CsvParser(reader, CultureInfo.InvariantCulture))
			{
				var row = parser.Read();
				Assert.AreEqual("1", row[0]);

				row = parser.Read();
				Assert.AreEqual("2", row[0]);
			}
		}

		[TestMethod]
		public void SingleFieldAndSecondRowIsQuotedAndLastRowHasNoLineEndingTest()
		{
			var s = new StringBuilder();
			s.Append("1\r");
			s.Append("\"2\"");
			using (var reader = new StringReader(s.ToString()))
			using (var parser = new CsvParser(reader, CultureInfo.InvariantCulture))
			{
				var row = parser.Read();
				Assert.AreEqual("1", row[0]);

				row = parser.Read();
				Assert.AreEqual("2", row[0]);
			}
		}

		[TestMethod]
		public void MultipleFieldsAndSingleRowAndLastRowHasNoLineEndingTest()
		{
			var s = new StringBuilder();
			s.Append("1,2\r");
			using (var reader = new StringReader(s.ToString()))
			using (var parser = new CsvParser(reader, CultureInfo.InvariantCulture))
			{
				parser.Configuration.Delimiter = ",";
				var row = parser.Read();
				Assert.AreEqual("1", row[0]);
				Assert.AreEqual("2", row[1]);
			}
		}

		[TestMethod]
		public void MultipleFieldsAndMultipleRowsAndLastRowHasNoLineEndingTest()
		{
			var s = new StringBuilder();
			s.Append("1,2\r");
			s.Append("3,4");
			using (var reader = new StringReader(s.ToString()))
			using (var parser = new CsvParser(reader, CultureInfo.InvariantCulture))
			{
				parser.Configuration.Delimiter = ",";
				var row = parser.Read();
				Assert.AreEqual("1", row[0]);
				Assert.AreEqual("2", row[1]);

				row = parser.Read();
				Assert.AreEqual("3", row[0]);
				Assert.AreEqual("4", row[1]);
			}
		}

		[TestMethod]
		public void MultipleFieldsAndMultipleRowsAndLastRowHasLineEndingTest()
		{
			var s = new StringBuilder();
			s.Append("1,2\r");
			s.Append("3,4\r");
			using (var reader = new StringReader(s.ToString()))
			using (var parser = new CsvParser(reader, CultureInfo.InvariantCulture))
			{
				parser.Configuration.Delimiter = ",";
				var row = parser.Read();
				Assert.AreEqual("1", row[0]);
				Assert.AreEqual("2", row[1]);

				row = parser.Read();
				Assert.AreEqual("3", row[0]);
				Assert.AreEqual("4", row[1]);
			}
		}

		//[TestMethod]
		//public void MultipleFieldsAndMultipleRowsAndDataAddedAfterReadTest()
		//{
		//	using (MemoryStream s = new MemoryStream())
		//	using (var reader = new StreamReader(s, Encoding.ASCII))
		//	using (var parser = new CsvParser(reader, CultureInfo.InvariantCulture))
		//	{
		//		parser.Configuration.Delimiter = ",";

		//		var row1 = Encoding.ASCII.GetBytes("1,2");
		//		s.Write(row1, 0, row1.Length);
		//		s.Position = 0;

		//		var row = parser.Read();
		//		Assert.AreEqual("1", row[0]);
		//		Assert.AreEqual("2", row[1]);

		//		var row2 = Encoding.ASCII.GetBytes("\r3,4\r");
		//		s.Write(row2, 0, row2.Length);
		//		s.Position = row1.Length;

		//		row = parser.Read();
		//		Assert.AreEqual("3", row[0]);
		//		Assert.AreEqual("4", row[1]);
		//	}
		//}
		
		[TestMethod]
		public void MultipleFieldsAndSingleRowAndRowStartsWithCRTest()
		{
			var s = new StringBuilder();
			s.Append("\r1,2\r");
			using (var reader = new StringReader(s.ToString()))
			using (var parser = new CsvParser(reader, CultureInfo.InvariantCulture))
			{
				parser.Configuration.Delimiter = ",";

				var row = parser.Read(); // ["","2"] should be ["1","2"]
				Assert.AreEqual("1", row[0]);
				Assert.AreEqual("2", row[1]);
			}
		}

		[TestMethod]
		public void TwoCRsChillinTest()
		{
			var s = new StringBuilder();
			s.Append("\r\r");
			using (var reader = new StringReader(s.ToString()))
			using (var parser = new CsvParser(reader, CultureInfo.InvariantCulture))
			{
				parser.Configuration.IgnoreBlankLines = false;

				var row = parser.Read();
				Assert.AreEqual(0, row.Length);
				row = parser.Read();
				Assert.AreEqual(0, row.Length);
				row = parser.Read();
				Assert.IsNull(row);
			}
		}

		[TestMethod]
		public void MultipleFieldsAndMultipleRowsAndRowStartsWithCRTest()
		{
			var s = new StringBuilder();
			s.Append("1,2\r");
			s.Append("\r3,4\r");
			using (var reader = new StringReader(s.ToString()))
			using (var parser = new CsvParser(reader, CultureInfo.InvariantCulture))
			{
				parser.Configuration.Delimiter = ",";

				var row = parser.Read(); // ["1","2"]
				Assert.AreEqual("1", row[0]);
				Assert.AreEqual("2", row[1]);

				row = parser.Read(); // ["","4"] should be ["3","4"]
				Assert.AreEqual("3", row[0]);
				Assert.AreEqual("4", row[1]);
			}
		}

		[TestMethod]
		public void MultipleFieldsAndSingleRowAndRowStartsWithCRNLTest()
		{
			var s = new StringBuilder();
			s.Append("\r\n1,2\r\n");
			using (var reader = new StringReader(s.ToString()))
			using (var parser = new CsvParser(reader, CultureInfo.InvariantCulture))
			{
				parser.Configuration.Delimiter = ",";

				var row = parser.Read();
				Assert.AreEqual("1", row[0]);
				Assert.AreEqual("2", row[1]);
			}
		}

		[TestMethod]
		public void MultipleFieldsAndMultipleRowsAndRowStartsWithCRNLTest()
		{
			var s = new StringBuilder();
			s.Append("1,2\r\n");
			s.Append("\r\n3,4\r\n");
			using (var reader = new StringReader(s.ToString()))
			using (var parser = new CsvParser(reader, CultureInfo.InvariantCulture))
			{
				parser.Configuration.Delimiter = ",";

				var row = parser.Read();
				Assert.AreEqual("1", row[0]);
				Assert.AreEqual("2", row[1]);

				row = parser.Read();
				Assert.AreEqual("3", row[0]);
				Assert.AreEqual("4", row[1]);
			}
		}

		[TestMethod]
		public void MultipleFieldsAndSingleRowAndRowStartsWithNLTest()
		{
			var s = new StringBuilder();
			s.Append("\n1,2\n");
			using (var reader = new StringReader(s.ToString()))
			using (var parser = new CsvParser(reader, CultureInfo.InvariantCulture))
			{
				parser.Configuration.Delimiter = ",";

				var row = parser.Read();
				Assert.AreEqual("1", row[0]);
				Assert.AreEqual("2", row[1]);
			}
		}

		[TestMethod]
		public void MultipleFieldsAndMultipleRowsAndRowStartsWithNLTest()
		{
			var s = new StringBuilder();
			s.Append("1,2\n");
			s.Append("\n3,4\n");
			using (var reader = new StringReader(s.ToString()))
			using (var parser = new CsvParser(reader, CultureInfo.InvariantCulture))
			{
				parser.Configuration.Delimiter = ",";

				var row = parser.Read();
				Assert.AreEqual("1", row[0]);
				Assert.AreEqual("2", row[1]);

				row = parser.Read();
				Assert.AreEqual("3", row[0]);
				Assert.AreEqual("4", row[1]);
			}
		}

		[TestMethod]
		public void MultipleFieldsAndMultipleRowsAndLastFieldInFirstRowIsQuotedAndLastRowHasLineEndingTest()
		{
			var s = new StringBuilder();
			s.Append("1,\"2\"\r");
			s.Append("3,4\r");
			using (var reader = new StringReader(s.ToString()))
			using (var parser = new CsvParser(reader, CultureInfo.InvariantCulture))
			{
				parser.Configuration.Delimiter = ",";
				var row = parser.Read();
				Assert.AreEqual("1", row[0]);
				Assert.AreEqual("2", row[1]);

				row = parser.Read();
				Assert.AreEqual("3", row[0]);
				Assert.AreEqual("4", row[1]);
			}
		}

		[TestMethod]
		public void MultipleFieldsAndMultipleRowsAndSecondRowFirstFieldIsQuotedAndLastRowHasLineEndingTest()
		{
			var s = new StringBuilder();
			s.Append("1,2\r");
			s.Append("\"3\",4\r");
			using (var reader = new StringReader(s.ToString()))
			using (var parser = new CsvParser(reader, CultureInfo.InvariantCulture))
			{
				parser.Configuration.Delimiter = ",";
				var row = parser.Read();
				Assert.AreEqual("1", row[0]);
				Assert.AreEqual("2", row[1]);

				row = parser.Read();
				Assert.AreEqual("3", row[0]);
				Assert.AreEqual("4", row[1]);
			}
		}

		[TestMethod]
		public void MultipleFieldsAndMultipleRowsAndAllFieldsQuotedAndHasLineEndingTest()
		{
			var s = new StringBuilder();
			s.Append("\"1\",\"2\"\r");
			s.Append("\"3\",\"4\"\r");
			using (var reader = new StringReader(s.ToString()))
			using (var parser = new CsvParser(reader, CultureInfo.InvariantCulture))
			{
				parser.Configuration.Delimiter = ",";
				var row = parser.Read();
				Assert.AreEqual("1", row[0]);
				Assert.AreEqual("2", row[1]);

				row = parser.Read();
				Assert.AreEqual("3", row[0]);
				Assert.AreEqual("4", row[1]);
			}
		}
	}
}
