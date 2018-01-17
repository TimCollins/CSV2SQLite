using CSV2SQLite.App;
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
    }
}
