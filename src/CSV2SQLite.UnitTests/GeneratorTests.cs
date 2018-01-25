using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using CSV2SQLite.App;
using CSV2SQLite.App.Exceptions;
using CSV2SQLite.App.Interfaces;
using Moq;
using NUnit.Framework;

namespace CSV2SQLite.UnitTests
{
    [TestFixture]
    public class GeneratorTests
    {
        private SQLiteGenerator _generator;
        private Mock<IFileWrapper> _fileWrapper;

        [SetUp]
        public void Setup()
        {
            _fileWrapper = new Mock<IFileWrapper>();
            _fileWrapper.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);
            _generator = new SQLiteGenerator(_fileWrapper.Object);
        }

        [Test]
        public void CsvHeaderExceptionWillBeThrownForEmptyFile()
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(string.Empty));
            var streamReader = new StreamReader(stream);
            _fileWrapper.Setup(m => m.Open(It.IsAny<string>())).Returns(streamReader);

            var config = new Configuration
            {
                InputFilename = "input.csv",
                OutputFilename = "output.sql"
            };

            Assert.Throws<CsvHeaderException>(() => _generator.Generate(config));
        }

        [Test]
        public void HeaderDataShouldBeColumnsInTheOutput()
        {
            string[] columns = { "Column1", "Column2", "Column3" };
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(string.Join(", ", columns)));
            var streamReader = new StreamReader(stream);
            _fileWrapper.Setup(m => m.Open(It.IsAny<string>())).Returns(streamReader);

            var config = new Configuration
            {
                InputFilename = "input.csv",
                OutputFilename = "output.sql"
            };

            var output = _generator.Generate(config);

            // The data format may need to be checked or skipped here rather than just 
            // defaulting to 'text'.
            foreach (var column in columns)
            {
                var test = string.Format("{0} text", column);
                Assert.IsTrue(output.Contains(test));
            }
        }

        [Test]
        public void RowDataShouldBeInsertStatementsInTheOutput()
        {
            const string csv = "Column1, Column2, Column3\nFred, 123, ABC\nBob, 777, ZZZ";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(csv));
            var streamReader = new StreamReader(stream);
            _fileWrapper.Setup(m => m.Open(It.IsAny<string>())).Returns(streamReader);
            var config = new Configuration
            {
                InputFilename = "input.csv",
                OutputFilename = "output.sql"
            };

            var output = _generator.Generate(config);

            Assert.AreEqual(2, Regex.Matches(output, "INSERT").Count);
        }
    }
}
