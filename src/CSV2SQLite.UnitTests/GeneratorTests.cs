using System.IO;
using System.Text;
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
            var stream  = new MemoryStream(Encoding.UTF8.GetBytes(string.Empty));
            var streamReader = new StreamReader(stream);
            _fileWrapper.Setup(m => m.Open(It.IsAny<string>())).Returns(streamReader);

            var config = new Configuration
            {
                InputFilename = "input.csv",
                OutputFilename = "output.sql"
            };

            Assert.Throws<CsvHeaderException>(() => _generator.Generate(config));
        }
    }
}
