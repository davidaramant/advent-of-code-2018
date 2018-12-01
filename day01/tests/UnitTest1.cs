using System.Linq;
using NUnit.Framework;
using day01;

namespace Tests
{
    public class Tests
    {
        [TestCase("+1, -1", 0)]
        [TestCase("+3, +3, +4, -2, -4", 10)]
        [TestCase("-6, +3, +8, +5, -6", 5)]
        [TestCase("+7, +7, -2, -7, -4", 14)]
        public void CheckFindingFirstRepeatedFreqency(string input, int answer)
        {
            var sequence = input.Split(',').Select(int.Parse);

            Assert.That(Program.FirstDuplicatedFreqency(sequence), Is.EqualTo(answer));
        }
    }
}