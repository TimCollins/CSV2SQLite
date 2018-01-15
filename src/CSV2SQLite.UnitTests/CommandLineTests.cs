using System;
using System.IO;
using System.Net.Configuration;
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

            var ex = Assert.Throws<ArgumentException>(() => _generator.ValidateCommandLine(args));
            Assert.AreEqual(ex.Message, "No arguments provided");
        }

        [Test]
        public void CallToMissingFileShouldFail()
        {
            _fileWrapper.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);
            _generator = new SQLiteGenerator(_fileWrapper.Object);
            var args = new[]
            {
                "missing.csv"
            };
            var expected = string.Format("{0} was not found", args[0]);
            var ex = Assert.Throws<FileNotFoundException>(() => _generator.ValidateCommandLine(args));

            Assert.AreEqual(ex.Message, expected);
        }

        //[Test]
        //public void CallWithOneParameterShouldSucceed()
        //{
        //    var args = new[] {"input.csv"};
        //    Assert.IsTrue(_generator.ValidateCommandLine(args));
        //}

        //[Test]
        //public void CallWithTwoParametersShouldSucceed()
        //{
        //    var args = new[]
        //    {
        //        "input.csv",
        //        "output.sql"
        //    };

        //    Assert.IsTrue(_generator.ValidateCommandLine(args));
        //}

        //[Test]
        //public void CallWithThreeParametersShouldSucceed()
        //{
        //    var args = new[]
        //    {
        //        "input.csv",
        //        "output.sql",
        //        "config.json"
        //    };

        //    Assert.IsTrue(_generator.ValidateCommandLine(args));
        //}

        [Test]
        public void CallWithFourParametersShouldFail()
        {
            var args = new[]
            {
                "input.csv",
                "output.sql",
                "config.json",
                "other.txt"
            };

            var ex = Assert.Throws<ArgumentException>(() => _generator.ValidateCommandLine(args));
            Assert.AreEqual(ex.Message, "Too many arguments provided");
        }
    }
}
