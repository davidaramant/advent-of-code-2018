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
            var containsDouble = Program.CountRepeatedCharacters(input).ContainsDouble;
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
            var containsTriple = Program.CountRepeatedCharacters(input).ContainsTriple;
            Assert.That(containsTriple, Is.EqualTo(expectedResult));
        }

        [TestCase("abcde", "axcye", 2)]
        [TestCase("fghij", "fguij", 1)]
        public void ShouldDetermineDifference(string s1, string s2, int expectedDiff)
        {
            Assert.That(Program.Difference(s1, s2), Is.EqualTo(expectedDiff));
        }

        [Test]
        public void ShouldGetSharedLetters()
        {
            Assert.That(Program.GetSharedLetters("fghij","fguij"),Is.EqualTo("fgij"));
        }

        [Test]
        public void ShouldDetermineSharedLettersOfDiff1Strings()
        {
            var input = new[]
            {
                "abcde",
                "fghij",
                "klmno",
                "pqrst",
                "fguij",
                "axcye",
                "wvxyz",
            };

            Assert.That(Program.FindSharedLetters(input), Is.EqualTo("fgij"));
        }
    }
}