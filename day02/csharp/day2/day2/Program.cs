using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day2
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(ComputeChecksum(File.ReadLines("input.txt")));
        }

        public struct Result
        {
            public readonly bool ContainsDouble;
            public readonly bool ContainsTriple;

            public Result(bool containsDouble, bool containsTriple)
            {
                ContainsDouble = containsDouble;
                ContainsTriple = containsTriple;
            }

            public Count ToIncrement() => new Count(ContainsDouble ? 1 : 0, ContainsTriple ? 1 : 0);
        }

        public struct Count
        {
            public readonly int NumDoubles;
            public readonly int NumTriples;
            
            public Count(int numDoubles, int numTriples)
            {
                NumDoubles = numDoubles;
                NumTriples = numTriples;
            }

            public static Count operator +(Count c1, Count c2)
            {
                return new Count(c1.NumDoubles + c2.NumDoubles, c1.NumTriples + c2.NumTriples);
            }

            public int GetChecksum() => NumDoubles * NumTriples;
        }

        public static int ComputeChecksum(IEnumerable<string> inputs)
        {
            return inputs
                .Select(GetScore)
                .Aggregate(
                    new Count(),
                    (counts, result) => counts + result.ToIncrement())
                .GetChecksum();
        }

        public static Result GetScore(string input)
        {
            var dict = new Dictionary<char, int>();

            foreach (var c in input)
            {
                if (!dict.ContainsKey(c))
                {
                    dict[c] = 0;
                }
                dict[c]++;
            }

            return new Result(dict.Values.Any(count => count == 2), dict.Values.Any(count => count == 3));
        }
    }
}
