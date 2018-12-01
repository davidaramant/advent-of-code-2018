using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day01
{
    public class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadLines("input.txt").Select(int.Parse).ToArray();
            Console.WriteLine("Final frequency: " + input.Sum());
            Console.WriteLine("First repeated frequency: " + FirstDuplicatedFreqency(input));
        }

        public static int FirstDuplicatedFreqency(IEnumerable<int> input)
        {
            int current = 0;
            var set = new HashSet<int> { current };
            while (true)
            {
                foreach (var change in input)
                {
                    current += change;
                    if (!set.Add(current))
                    {
                        return current;
                    }
                }
            }
        }
    }
}
