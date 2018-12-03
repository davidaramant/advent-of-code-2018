using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Day03
{
    public class Program
    {
        static void Main(string[] args)
        {
            var claims = File.ReadAllLines("input.txt").AsParallel().Select(Claim.Parse).ToArray();

            Console.Out.WriteLine("Square inches covered 2 times or more: " + FindAreaCoveredAtLeastTwice(claims));
            Console.Out.WriteLine("Claim that doesn't overlap: " + FindClaimThatDoesNotOverlap(claims));

            Console.ReadKey();
        }

        public static int FindAreaCoveredAtLeastTwice(IEnumerable<Claim> claims)
        {
            var counter = new ConcurrentDictionary<Point, int>();
            Parallel.ForEach(claims.SelectMany(c => c.GetPointsInArea()),
                p => { counter.AddOrUpdate(p, 1, (id, count) => count + 1); });

            return counter.Values.Count(c => c >= 2);
        }

        public static int FindClaimThatDoesNotOverlap(Claim[] claims)
        {
            List<bool> overlappingIds = Enumerable.Repeat(false, claims.Length).ToList();
            var pairsToCheck = 
                Enumerable.Range(0, claims.Length - 1)
                .SelectMany(i => Enumerable.Range(i + 1, claims.Length - (i + 1)).Select(j => (i, j)));

            Parallel.ForEach(pairsToCheck, pair =>
            {
                var (i, j) = pair;
                if (claims[i].Intersects(claims[j]))
                {
                    overlappingIds[i] = true;
                    overlappingIds[j] = true;
                }
            });

            return overlappingIds.IndexOf(false) + 1;
        }
    }

    public sealed class Claim
    {
        private readonly int _id;
        private readonly Rectangle _area;

        public Claim(int id, Rectangle area)
        {
            _id = id;
            _area = area;
        }

        public static Claim Parse(string input)
        {
            var regex = new Regex(@"#(\d+) @ (\d+),(\d+): (\d+)x(\d+)", RegexOptions.Compiled);
            var match = regex.Match(input);
            if (!match.Success)
            {
                throw new Exception("What is this line? : " + input);
            }

            var id = int.Parse(match.Groups[1].Value);
            var topX = int.Parse(match.Groups[2].Value);
            var topY = int.Parse(match.Groups[3].Value);
            var width = int.Parse(match.Groups[4].Value);
            var height = int.Parse(match.Groups[5].Value);

            return new Claim(id, new Rectangle(new Point(topX, topY), new Size(width, height)));
        }

        public IEnumerable<Point> GetPointsInArea()
        {
            foreach (var x in Enumerable.Range(_area.Left, _area.Width))
            {
                foreach (var y in Enumerable.Range(_area.Top, _area.Height))
                {
                    yield return new Point(x, y);
                }
            }
        }

        public bool Intersects(Claim other) => _area.IntersectsWith(other._area);
    }
}
