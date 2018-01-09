using CSV2SQLite.App;
using CSV2SQLite.App.Interfaces;
using Moq;
using NUnit.Framework;

namespace CSV2SQLite.UnitTests
{
    [TestFixture]
    public class CommandLineTests
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
        public void CallWithNoParametersShouldFail()
        {
            var args = new string[0];

            Assert.IsFalse(_generator.IsValidCommandLine(args));
        }

        [Test]
        public void CallWithOneParameterShouldSucceed()
        {
            var args = new[] {"input.csv"};
            Assert.IsTrue(_generator.IsValidCommandLine(args));
        }
        
        [Test]
        public void CallWithTwoParametersShouldSucceed()
        {
            var args = new[]
            {
                "input.csv",
                "output.sql"
            };

            Assert.IsTrue(_generator.IsValidCommandLine(args));
        }

        [Test]
        public void CallWithThreeParametersShouldSucceed()
        {
            var args = new[]
            {
                "input.csv",
                "output.sql",
                "config.json"
            };

            Assert.IsTrue(_generator.IsValidCommandLine(args));
        }
    }
}
