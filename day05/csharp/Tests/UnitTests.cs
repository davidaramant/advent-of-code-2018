using NUnit.Framework;

namespace Tests
{
    public class UnitTests
    {
        [TestCase("aA",0)]
        [TestCase("aBbA",0)]
        [TestCase("abAB",4)]
        [TestCase("aabAAB",6)]
        [TestCase("dabAcCaCBAcCcaDA",10)]
        public void ShouldCountReducedPolymer(string input, int resultCount)
        {
            Assert.That(day05.Program.CountReactedUnits(input), Is.EqualTo(resultCount));
        }
    }
}