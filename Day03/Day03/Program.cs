using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

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

    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");

            Console.Out.WriteLine("Square inches covered 2 times or more: " + FindAreaCoveredAtLeastTwice(lines));

            Console.ReadKey();
        }

        private static int FindAreaCoveredAtLeastTwice(IEnumerable<string> lines)
        {
            var allCoveredPoints = lines
                    .AsParallel()
                    .Select(Claim.Parse)
                    .SelectMany(c => c.GetPointsInArea());

            var counter = new Dictionary<Point, int>();
            foreach (var p in allCoveredPoints)
            {
                counter.Increment(p);
            }

            return counter.Values.Count(c => c >= 2);
        }
    }

    sealed class Claim
    {
        public int Id { get; }
        public Rectangle Area { get; }

        public Claim(int id, Rectangle area)
        {
            Id = id;
            Area = area;
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
            foreach (var x in Enumerable.Range(Area.Left, Area.Width))
            {
                foreach (var y in Enumerable.Range(Area.Top, Area.Height))
                {
                    yield return new Point(x, y);
                }
            }
        }
    }
}
