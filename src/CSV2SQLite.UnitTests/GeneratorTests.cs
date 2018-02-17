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

        private StreamReader GetFakeStreamReader()
        {
            const string csv = "Column1, Column2, Column3\nFred, 123, ABC\nBob, 777, ZZZ";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(csv));
            return new StreamReader(stream);
        }

        private StreamReader GetFakeStreamReaderWithApostrophes()
        {
            const string csv = "Name, Sex, Bodyweight\nHomer O'Simpson,M,106kg\nGemma O'Rourke,F,65kg";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(csv));
            return new StreamReader(stream);
        }

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
            _fileWrapper.Setup(m => m.Open(It.IsAny<string>())).Returns(GetFakeStreamReader());
            var config = new Configuration
            {
                InputFilename = "input.csv",
                OutputFilename = "output.sql"
            };

            var output = _generator.Generate(config);

            Assert.AreEqual(2, Regex.Matches(output, "INSERT").Count);
        }

        [Test]
        public void WildcardShouldReturnAllCSVFilesInFolder()
        {
            var config = new Configuration
            {
                InputFilename = "*.csv",
                OutputFilename = "output.sql"
            };

            _fileWrapper.Setup(m => m.GetFiles(It.IsAny<string>(), "*.csv")).Returns(new[]
            {
                "one.csv",
                "two.csv",
                "three.csv"
            });

            _fileWrapper.Setup(m => m.Open("one.csv")).Returns(GetFakeStreamReader());
            _fileWrapper.Setup(m => m.Open("two.csv")).Returns(GetFakeStreamReader());
            _fileWrapper.Setup(m => m.Open("three.csv")).Returns(GetFakeStreamReader());

            var output = _generator.Generate(config);
        }

        [Test]
        public void ApostrophesShouldBeEscaped()
        {
            _fileWrapper.Setup(m => m.Open(It.IsAny<string>())).Returns(GetFakeStreamReaderWithApostrophes());
            var config = new Configuration
            {
                InputFilename = "input.csv",
                OutputFilename = "output.sql"
            };

            var output = _generator.Generate(config);

            // The test data has two names with apostrophes in it.
            // Including the string start and end apostrophes there should be 16
            Assert.AreEqual(16, Regex.Matches(output, "'").Count);
        }

        [Test]
        public void SpacesShouldBeStripped()
        {
            _fileWrapper.Setup(m => m.Open(It.IsAny<string>())).Returns(GetFakeStreamReader());
            var config = new Configuration
            {
                InputFilename = "input.csv",
                OutputFilename = "output.sql"
            };

            var output = _generator.Generate(config);
            var lines = output.Split('\n');

            foreach (var line in lines)
            {
                if (line.StartsWith("VALUES"))
                {
                    Assert.IsFalse(line.Contains("' "));
                }
            }
        }
    }
}
