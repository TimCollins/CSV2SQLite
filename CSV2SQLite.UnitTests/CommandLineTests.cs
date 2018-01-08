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
        public void Something()
        {
            var args = new string[0];

            Assert.IsFalse(_generator.IsValidCommandLine(args));
        }
    }
}
