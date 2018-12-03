using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Day03
{
    static class DictionaryExtensions
    {
        public static void Increment(this Dictionary<Point, int> counter, Point p)
        {
            if (counter.ContainsKey(p))
            {
                counter[p]++;
            }
            else
            {
                counter[p] = 1;
            }
        }
    }

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
            var counter = new Dictionary<Point, int>();
            foreach (var p in claims.SelectMany(c => c.GetPointsInArea()))
            {
                counter.Increment(p);
            }

            return counter.Values.Count(c => c >= 2);
        }

        public static int FindClaimThatDoesNotOverlap(Claim[] claims)
        {
            List<bool> overlappingIds = Enumerable.Repeat(false, claims.Length).ToList();
            Parallel.For(0, claims.Length - 1, i =>
            {
                for (int j = i + 1; j < claims.Length; j++)
                {
                    var intersects = claims[i].Intersects(claims[j]);
                    overlappingIds[i] |= intersects;
                    overlappingIds[j] |= intersects;
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
