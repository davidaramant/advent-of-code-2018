using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace day2
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Checksum: " + ComputeChecksum(File.ReadLines("input.txt")));
            Console.WriteLine("Common Letters: " + FindSharedLetters(File.ReadLines("input.txt")));
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
                .Select(CountRepeatedCharacters)
                .Aggregate(
                    new Count(),
                    (counts, result) => counts + result.ToIncrement())
                .GetChecksum();
        }

        public static Result CountRepeatedCharacters(string input)
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

        public static int Difference(string s1, string s2)
        {
            return s1.Zip(s2, (c1, c2) => (c1, c2)).Sum(pair => pair.Item1 != pair.Item2 ? 1 : 0);
        }

        public static string GetSharedLetters(string s1, string s2)
        {
            var sb = new StringBuilder();

            foreach (var (c1, c2) in s1.Zip(s2, (c1, c2) => (c1, c2)))
            {
                if (c1 == c2)
                {
                    sb.Append(c1);
                }
            }

            return sb.ToString();
        }

        public static string FindSharedLetters(IEnumerable<string> input)
        {
            var inputArray = input.ToArray();

            for (int i = 0; i < inputArray.Length; i++)
            {
                for (int j = i + 1; j < inputArray.Length; j++)
                {
                    if (Difference(inputArray[i], inputArray[j]) == 1)
                        return GetSharedLetters(inputArray[i], inputArray[j]);
                }
            }

            return "";
        }
    }
}
