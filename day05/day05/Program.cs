using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day05
{
    public class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllText("input.txt").Trim();
            Console.WriteLine("Polymer length: " + CountReactedUnits(input));
            Console.WriteLine("Smallest polymer length: " + CountSmallestReactedUnits(input));
            Console.ReadKey();
        }

        public static int CountReactedUnits(string input)
        {
            var polymer = new LinkedList<char>(input);

            bool Reacts(LinkedListNode<char> c1, LinkedListNode<char> c2) =>
                c1 != null &&
                c2 != null &&
                char.ToLowerInvariant(c1.Value) == char.ToLowerInvariant(c2.Value) &&
                char.IsUpper(c1.Value) != char.IsUpper(c2.Value);

            bool madeChange;
            do
            {
                madeChange = false;
                var current = polymer.First;

                while (current != null)
                {
                    if (Reacts(current.Previous, current))
                    {
                        polymer.Remove(current.Previous);
                        polymer.Remove(current);
                        madeChange = true;
                    }

                    current = current.Next;
                }

            } while (madeChange);

            return polymer.Count;
        }

        public static int CountSmallestReactedUnits(string input)
        {
            var allChars = new HashSet<char>(input.ToLowerInvariant());

            return allChars
                    .AsParallel()
                    .Select(c => CountReactedUnits(input.Replace($"{c}", "").Replace($"{char.ToUpper(c)}","")))
                    .Min();
        }
    }
}
