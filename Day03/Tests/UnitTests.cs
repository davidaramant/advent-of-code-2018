using System.Linq;
using NUnit.Framework;
using Day03;

namespace Tests
{
    public class UnitTests
    {
        [Test]
        public void ShouldFigureOutOverlapOfExampleProgram()
        {
            var claims = new[]
            {
                "#1 @ 1,3: 4x4",
                "#2 @ 3,1: 4x4",
                "#3 @ 5,5: 2x2",
            }.Select(Claim.Parse).ToArray();

            Assert.That(Program.FindClaimThatDoesNotOverlap(claims), Is.EqualTo(3));
        }
    }
}