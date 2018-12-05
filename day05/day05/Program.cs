using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace day05
{
    public class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllText("input.txt").Trim();
            var part1Timer = Stopwatch.StartNew();
            Console.WriteLine("Polymer length: " + CountReactedUnits(input));
            part1Timer.Stop();
            var part2Timer = Stopwatch.StartNew();
            Console.WriteLine("Smallest polymer length: " + CountSmallestReactedUnits(input));
            part2Timer.Stop();
            Console.Out.WriteLine($"Part 1: {part1Timer.Elapsed}, Part 2: {part2Timer.Elapsed}");
            Console.ReadKey();
        }

        public static int CountReactedUnits(IEnumerable<char> input)
        {
            var polymer = new LinkedList<char>(input);

            bool Reacts(LinkedListNode<char> c1, LinkedListNode<char> c2) =>
                c1 != null &&
                c2 != null &&
                char.ToLower(c1.Value) == char.ToLower(c2.Value) &&
                char.IsUpper(c1.Value) != char.IsUpper(c2.Value);

            var current = polymer.First;

            while (current != null)
            {
                if (Reacts(current, current.Next))
                {
                    var nextToCheck = current.Previous ?? current.Next?.Next;

                    polymer.Remove(current.Next);
                    polymer.Remove(current);

                    current = nextToCheck;
                }
                else
                {
                    current = current.Next;
                }
            }

            return polymer.Count;
        }

        public static int CountSmallestReactedUnits(string input)
        {
            var allChars = new HashSet<char>(input.ToLower());

            return allChars
                    .AsParallel()
                    .Select(c => CountReactedUnits(input.Where(i => char.ToLower(i) != c)))
                    .Min();
        }
    }
}
