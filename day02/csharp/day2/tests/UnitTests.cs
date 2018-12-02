using NUnit.Framework;
using day2;

namespace Tests
{
    public class UnitTests
    {
        [Test]
        public void ShouldComputeChecksum()
        {
            var inputs = new[]
            {
                "abcdef",
                "bababc",
                "abbcde",
                "abcccd",
                "aabcdd",
                "abcdee",
                "ababab",
            };

            Assert.That(Program.ComputeChecksum(inputs), Is.EqualTo(12));
        }

        [TestCase("abcdef", false)]
        [TestCase("bababc", true)]
        [TestCase("abbcde", true)]
        [TestCase("abcccd", false)]
        [TestCase("aabcdd", true)]
        [TestCase("abcdee", true)]
        [TestCase("ababab", false)]
        public void ShouldDetermineIfItContainsADouble(string input, bool expectedResult)
        {
            var containsDouble = Program.GetScore(input).ContainsDouble;
            Assert.That(containsDouble, Is.EqualTo(expectedResult));
        }

        [TestCase("abcdef", false)]
        [TestCase("bababc", true)]
        [TestCase("abbcde", false)]
        [TestCase("abcccd", true)]
        [TestCase("aabcdd", false)]
        [TestCase("abcdee", false)]
        [TestCase("ababab", true)]
        public void ShouldDetermineIfItContainsATriple(string input, bool expectedResult)
        {
            var containsTriple = Program.GetScore(input).ContainsTriple;
            Assert.That(containsTriple, Is.EqualTo(expectedResult));
        }
    }
}