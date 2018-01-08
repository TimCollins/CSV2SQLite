using CSV2SQLite.App;
using NUnit.Framework;

namespace CSV2SQLite.UnitTests
{
    [TestFixture]
    public class CommandLineTests
    {
        private SQLiteGenerator _generator;

        [SetUp]
        public void Setup()
        {
            _generator = new SQLiteGenerator();
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

        [Test]
        public void FirstParameterShouldPointToAFile()
        {
            
        }
    }
}
