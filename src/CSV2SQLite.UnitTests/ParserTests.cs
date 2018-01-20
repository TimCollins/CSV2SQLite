using System.Collections.Generic;
using CSV2SQLite.App.Exceptions;
using CSV2SQLite.App.Interfaces;
using CSV2SQLite.App.Parser;
using Moq;
using NUnit.Framework;

namespace CSV2SQLite.UnitTests
{
    [TestFixture]
    public class ParserTests
    {
        [Test]
        public void ParserRequiresStringArrayParam()
        {
            var args = new[]
            {
                "hello"
            };

            var parser = new Parser(args);

            Assert.IsNotNull(parser);
        }

        [Test]
        public void ShouldShowHelpText()
        {
            var source = new List<string>
            {
                "/?", "-?", "--help"
            };

            foreach (var s in source)
            {
                var args = new[] { s };
                var parser = new Parser(args);
                var options = parser.Parse();

                Assert.IsTrue(options.ShowHelpScreen);
            }
        }

        [Test]
        public void ShouldSetConfigFile()
        {
            var wrapper = new Mock<IFileWrapper>();
            wrapper.Setup(w => w.Exists(It.IsAny<string>())).Returns(true);

            const string config = "custom.json";
            var args = new[]
            {
                "-c",
                config
            };

            var parser = new Parser(args, wrapper.Object);
            var options = parser.Parse();

            Assert.IsTrue(options.UseCustomConfig);
            Assert.AreEqual(config, options.CustomConfig);
        }

        [Test]
        public void ShouldThrowExceptionIfNoConfigFileSpecified()
        {
            var args = new[]
            {
                "-c"
            };

            var parser = new Parser(args);

            Assert.Throws<InvalidCommandLineException>(() => parser.Parse());
        }

        [Test]
        public void ShouldSetOutputFile()
        {
            var wrapper = new Mock<IFileWrapper>();
            wrapper.Setup(w => w.Exists(It.IsAny<string>())).Returns(true);

            const string output = "custom.sqlite";
            var args = new[]
            {
                "-o",
                output
            };

            var parser = new Parser(args, wrapper.Object);
            var options = parser.Parse();

            Assert.IsTrue(options.UseCustomOutputFile);
            Assert.AreEqual(output, options.CustomOutputFile);
        }

        [Test]
        public void ShouldThrowExceptionIfNoOutputFileSpecified()
        {
            var args = new[]
            {
                "-o"
            };

            var parser = new Parser(args);

            Assert.Throws<InvalidCommandLineException>(() => parser.Parse());
        }

        [Test]
        public void ShouldThrowExceptionIfConfigFileSpecifiedButNotFound()
        {
            var wrapper = new Mock<IFileWrapper>();
            wrapper.Setup(w => w.Exists(It.IsAny<string>())).Returns(false);
            const string config = "custom.json";
            var args = new[]
            {
                "-c",
                config
            };

            var parser = new Parser(args, wrapper.Object);

            Assert.Throws<InvalidCommandLineException>(() => parser.Parse());
        }

        [Test]
        public void ShouldSupportMultipleParameters()
        {
            var wrapper = new Mock<IFileWrapper>();
            wrapper.Setup(w => w.Exists(It.IsAny<string>())).Returns(true);

            const string config = "custom.json";
            const string output = "custom.sqlite";
            var args = new[]
            {
                "-c",
                config,
                "-o",
                output
            };

            var parser = new Parser(args, wrapper.Object);
            var options = parser.Parse();

            Assert.IsTrue(options.UseCustomConfig);
            Assert.AreEqual(config, options.CustomConfig);

            Assert.IsTrue(options.UseCustomOutputFile);
            Assert.AreEqual(output, options.CustomOutputFile);
        }

        [Test]
        public void ForwardSlashesShouldBeSupported()
        {
            var wrapper = new Mock<IFileWrapper>();
            wrapper.Setup(w => w.Exists(It.IsAny<string>())).Returns(true);

            const string config = "custom.json";
            const string output = "custom.sqlite";
            var args = new[]
            {
                "/c",
                config,
                "/o",
                output
            };

            var parser = new Parser(args, wrapper.Object);
            var options = parser.Parse();

            Assert.IsTrue(options.UseCustomConfig);
            Assert.AreEqual(config, options.CustomConfig);

            Assert.IsTrue(options.UseCustomOutputFile);
            Assert.AreEqual(output, options.CustomOutputFile);
        }
    }
}
